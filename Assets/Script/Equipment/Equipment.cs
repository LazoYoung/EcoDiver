using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Script.Equipment
{
    public class Equipment : MonoBehaviour
    {
        [SerializeField] private ActionBasedController rightController;
        [SerializeField] private Tool defaultTool;
        private Tool _activeTool;

        public Tool GetActiveTool()
        {
            return _activeTool;
        }
        
        public void Equip(Tool tool)
        {
            _activeTool = tool;
            DetachModel();
            AttachModel();
            Debug.Log($"Equip: {tool.Identifier}");
        }

        private void AttachModel()
        {
            var parent = rightController.modelParent;
            var prefab = _activeTool.ForearmPrefab;

            if (prefab)
            {
                var obj = Instantiate(prefab, parent);
                rightController.model = obj.transform;
                obj.SetActive(true);
            }
        }

        private void DetachModel()
        {
            var model = rightController.model;

            if (model)
            {
                Destroy(model.gameObject);
                rightController.model = null;                
            }
        }

        private void Start()
        {
            if (rightController == null)
            {
                rightController = FindObjectOfType<ActionBasedController>();
            }

            if (rightController != null)
            {
                rightController.modelPrefab = null;
            }
            else
            {
                Debug.LogError("ActionBasedController not found! Equipment is disabled.");
                enabled = false;
                return;
            }

            if (defaultTool)
            {
                Equip(defaultTool);
            }
        }
    }
}
