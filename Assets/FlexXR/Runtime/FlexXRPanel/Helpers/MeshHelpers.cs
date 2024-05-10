using UnityEngine;

namespace FlexXR.Runtime.FlexXRPanel.Helpers
{
    public static class MeshHelpers
    {
        public static Mesh CreateUnitMesh(int horizontalSegments, int verticalSegments)
        {
            var mesh = new Mesh();
            
            var vertices = new Vector3[(horizontalSegments + 1) * (verticalSegments + 1)];
            var indices  = new int[horizontalSegments * verticalSegments * 6];
            
            var horizontalStep = 1f / horizontalSegments;
            var verticalStep   = 1f / verticalSegments;
            
            for (var y = 0; y <= verticalSegments; y++)
            {
                for (var x = 0; x <= horizontalSegments; x++)
                {
                    var index = x + y * (horizontalSegments + 1);
                    vertices[index] = new Vector3(x * horizontalStep, y * verticalStep, 0);
                }
            }

            for (var y = 0; y < verticalSegments; y++)
            {
                for (var x = 0; x < horizontalSegments; x++)
                {
                    var index = x + y * horizontalSegments;
                    indices[index * 6 + 0] = x + y * (horizontalSegments + 1);
                    indices[index * 6 + 1] = x + (y + 1) * (horizontalSegments + 1);
                    indices[index * 6 + 2] = x + 1 + y * (horizontalSegments + 1);
                    indices[index * 6 + 3] = x + 1 + y * (horizontalSegments + 1);
                    indices[index * 6 + 4] = x + (y + 1) * (horizontalSegments + 1);
                    indices[index * 6 + 5] = x + 1 + (y + 1) * (horizontalSegments + 1);
                }
            }

            mesh.vertices  = vertices;
            mesh.triangles = indices;

            return mesh;
        }
        
        public static Vector3[] Shift(in Vector3[] vertices, in Vector3 shift)
        {
            var newVertices = new Vector3[vertices.Length];
            for (var i = 0; i < vertices.Length; i++) newVertices[i] = vertices[i] + shift;
            return newVertices;
        }
        
        public static Vector3[] Stretch(in Vector3[] vertices, in float width, in float height, in float depth)
        {
            var newVertices = new Vector3[vertices.Length];
            for (var i = 0; i < vertices.Length; i++)
            {
                newVertices[i].x = vertices[i].x * width;
                newVertices[i].y = vertices[i].y * height;
                newVertices[i].z = vertices[i].z * depth;
            }
            return newVertices;
        }
        
        public static Vector2[] MeshUVs(in Vector3[] vertices, in RectBoundsFloat referenceUVBounds)
        {
            var uvs = new Vector2[vertices.Length];
            for (var i = 0; i < uvs.Length; i++)
            {
                uvs[i].x = (vertices[i].x - referenceUVBounds.xMin) / referenceUVBounds.Width;
                uvs[i].y = (vertices[i].y - referenceUVBounds.yMin) /referenceUVBounds.Height;
            }
            return uvs;
        }
        
        public static Vector3[] BendIntoSemiCylinder(in Vector3[] vertices, in float radius, in RectBoundsFloat referenceBounds)
        {
            var newVertices = new Vector3[vertices.Length];
            
            for (var i = 0; i < vertices.Length; i++)
            {
                var s     = vertices[i].x - referenceBounds.Center.x;
                var theta = s / radius;
                newVertices[i].x = referenceBounds.Center.x + radius * Mathf.Sin(theta);
                newVertices[i].y = vertices[i].y;
                newVertices[i].z = radius * (Mathf.Cos(theta) - 1);
            }
            
            return newVertices;
        }
    }
}