using System;
using FlexXR.Runtime.FlexXRPanel.Helpers;
using UnityEngine;
using UnityEngine.UIElements;
using static FlexXR.Runtime.FlexXRPanel.Helpers.MeshHelpers;

namespace FlexXR.Runtime.FlexXRPanel
{
    public enum FittingMode
    {
        None,
        FillRenderTexture,
        CropToFlexXRContent
    }
    
    public enum MeshShape
    {
        None,
        Flat,
        Cylindrical
    }
    
    internal class FlexXRPanelMesh
    {
        private readonly MeshFilter   _filter;
        private readonly MeshCollider _collider;
        
        private Mesh Mesh
        {
            get => _filter.sharedMesh;  // ! `sharedMesh` gets references to the mesh while `mesh` returns a copy.
            set
            {
                _filter.mesh  = value;
                Vertices = value.vertices;
            }
        }
        
        private Vector3[] Vertices
        {
            set
            {
                Mesh.vertices             = value;
                _vertexDependentsAreStale = true;
            }
        }
        private Vector3[] CopyVertices() => Mesh.vertices;

        private bool _vertexDependentsAreStale;
        internal void UpdateVertexDependenciesIfStale()
        {
            if (!_vertexDependentsAreStale) return;
            Mesh.RecalculateNormals();
            Mesh.RecalculateBounds();
            _collider.sharedMesh      = Mesh; // ! Otherwise somehow the vertices aren't shared.
            _vertexDependentsAreStale = false;
        }

        internal FlexXRPanelMesh(FlexXRPanelElements flexXRPanelElements, MeshFilter meshFilter, int horizontalSegments)
        {
            _flexXRPanelElements = flexXRPanelElements;
            _filter     = meshFilter;
            _collider   = meshFilter.transform.GetComponent<MeshCollider>();
            
            Remesh(horizontalSegments);

            _maxElementBounds.CopyFrom(new RectBoundsInt
            {   // * The maximum texture size and GUI mesh sizes as well as the mapping between them is set by the original RenderTexture and mesh game object.
                xMin = 0,
                xMax = Document.panelSettings.targetTexture.width,
                yMin = 0,
                yMax = Document.panelSettings.targetTexture.height
            });

            UpdateMaxMeshBounds();
        }

        private void UpdateMaxMeshBounds()
        {
            _maxMeshBounds.CopyFrom(new RectBoundsFloat
            {
                xMin = _maxElementBounds.xMin * ScreenToWorldScale,
                xMax = _maxElementBounds.xMax * ScreenToWorldScale,
                yMin = _maxElementBounds.yMin * ScreenToWorldScale,
                yMax = _maxElementBounds.yMax * ScreenToWorldScale
            });
        }

        private void Remesh(int horizontalSegments)
        {
            Mesh         = CreateUnitMesh(horizontalSegments, 1);
            Mesh.name    = "FlexXR Mesh";

            _unitMeshVertices = CopyVertices();
            FlatMeshVertices  = CopyVertices();
            Shape             = MeshShape.Flat;
        }
        
        
        private readonly RectBoundsFloat _targetMeshBounds  = new();
        private readonly RectBoundsFloat _maxMeshBounds     = new();
        private readonly RectBoundsFloat _lastFitMeshBounds = new();
        
        private float _screenToWorldScale = 0.001f;

        internal float ScreenToWorldScale
        {
            get => _screenToWorldScale;
            set
            {
                if (Math.Abs(value - _screenToWorldScale) < float.Epsilon) return;
                _screenToWorldScale = value;
                UpdateMaxMeshBounds();
                CropMeshToGUIBounds();
            }
        }
        
        internal void ResetMeshToFillRenderTexture(bool forceRefit = false)
        {
            if (!forceRefit && _lastFitMeshBounds is { } && _targetMeshBounds.Equals(_lastFitMeshBounds)) return;
            
            _targetMeshBounds.CopyFrom(_maxMeshBounds);
            if (_targetMeshBounds.Equals(_lastFitMeshBounds)) return;
            FitVerticesAccordingToMeshBounds();
            _lastFitElementBounds.CopyFrom(_maxElementBounds);
        }

        private void FitVerticesAccordingToMeshBounds()
        {
            var vertices = _unitMeshVertices;
            vertices = Stretch(vertices, _targetMeshBounds.Width, _targetMeshBounds.Height, 1);
            vertices = Shift(vertices, new Vector3(_targetMeshBounds.xMin, _targetMeshBounds.yMin, 0));
            
            Vertices         = vertices;
            FlatMeshVertices = CopyVertices();

            if (Shape == MeshShape.Cylindrical) vertices = CurveMeshToCylinder();
            
            // ! Need another shift for the final position to make sense. Probably there's an upstream error that could be fixed to clean this up.
            Vertices = Shift(vertices, new Vector3(-_maxMeshBounds.Center.x, -_maxMeshBounds.Center.y, 0));
            
            _lastFitMeshBounds.CopyFrom(_targetMeshBounds);
        }
        
        
        #region Fit to UIElements

        internal RectBoundsInt ElementBounds        { get; } = new();
        private  UIDocument    Document             => _flexXRPanelElements.Document;
        private  VisualElement FlexXRContentElement => _flexXRPanelElements.FlexXRContentElement;

        private readonly FlexXRPanelElements _flexXRPanelElements;
        private readonly RectBoundsInt       _maxElementBounds     = new();
        private readonly RectBoundsInt       _lastFitElementBounds = new();

        internal void CropMeshToGUIBounds(bool forceRefit = false)
        {
            if (float.IsNaN(Document.rootVisualElement.contentRect.xMin)) return; // The original panel isn't rendered yet

            ElementBounds.CopyFrom(new RectBoundsFloat(FlexXRContentElement.worldBound).Scale(Document.panelSettings.scale).ToRectBoundsInt());

            if (!forceRefit && _lastFitElementBounds is { } && ElementBounds.Equals(_lastFitElementBounds)) return;

            _targetMeshBounds.xMin = ElementBounds.xMin * ScreenToWorldScale;
            _targetMeshBounds.xMax = ElementBounds.xMax * ScreenToWorldScale;
            _targetMeshBounds.yMin = _maxMeshBounds.yMax - ElementBounds.yMax * ScreenToWorldScale; // ! Y is inverted between pixel and world space.
            _targetMeshBounds.yMax = _maxMeshBounds.yMax - ElementBounds.yMin * ScreenToWorldScale; // ! Y is inverted between pixel and world space.
            _targetMeshBounds.CopyFrom(_targetMeshBounds.Limit(_maxMeshBounds));

            if (_targetMeshBounds.Equals(_lastFitMeshBounds)) return;
            FitVerticesAccordingToMeshBounds();
            _lastFitMeshBounds.CopyFrom(_targetMeshBounds);
            _lastFitElementBounds.CopyFrom(ElementBounds);
        }

        #endregion
        
        
        #region Mesh shape (e.g. flat or cylindrical)
        private Vector3[] _unitMeshVertices;
        
        private Vector3[] _flatMeshVertices;
        private Vector3[] FlatMeshVertices
        {
            get => _flatMeshVertices;
            set
            {
                _flatMeshVertices = value;
                Mesh.uv           = MeshUVs(_flatMeshVertices, _maxMeshBounds);
            }
        }

        private MeshShape _shape = MeshShape.None;
        internal MeshShape Shape
        {
            get => _shape;
            set
            {
                if (value == _shape) return;
                _shape = value;
                switch (value)
                {
                    case MeshShape.None:
                    default:
                        break;
                    case MeshShape.Flat:
                        FlattenMesh();
                        break;
                    case MeshShape.Cylindrical:
                        CurveMeshToCylinder();
                        break;
                }
            }
        }

        private const float MinimumCurvatureRadius = 0.01f;
        private       float _curvatureRadius;
        internal float CurvatureRadius
        {
            get => _curvatureRadius;
            set
            {
                var newRadius = (value > MinimumCurvatureRadius) ? value : MinimumCurvatureRadius;
                if (Math.Abs(newRadius - CurvatureRadius) < float.Epsilon) return;
                _curvatureRadius = newRadius;
                CurveMeshToCylinder();
            }
        }
        
        private int _horizontalSegments;
        internal int HorizontalSegments
        {
            get => _horizontalSegments;
            set
            {
                if (value == _horizontalSegments) return;
                _horizontalSegments = value;
                Remesh(_horizontalSegments);
                switch (Shape)
                {
                    case MeshShape.None:
                    case MeshShape.Flat:
                        break;
                    case MeshShape.Cylindrical:
                        CurveMeshToCylinder();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private void FlattenMesh() => Vertices = FlatMeshVertices;
        private Vector3[] CurveMeshToCylinder()
        {
            var vertices = BendIntoSemiCylinder(FlatMeshVertices, CurvatureRadius, _maxMeshBounds);
            Vertices = vertices;
            return vertices;
        }
        #endregion
    }
}
