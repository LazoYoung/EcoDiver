using UnityEngine.UIElements;

namespace FlexXR.Runtime.FlexXRPanel
{
    public class FlexXRPanelElements
    {
        private readonly UIDocument _document;
        internal UIDocument Document => _document;

        internal VisualElement RootVisualElement => Document.rootVisualElement;

        private readonly VisualElement _flexXRContentElement;
        public VisualElement FlexXRContentElement => _flexXRContentElement;
        
        internal FlexXRPanelElements(UIDocument document)
        {
            _document             =   document;
            _flexXRContentElement =   document.rootVisualElement.Q<FlexXRContainer>();
            _flexXRContentElement ??= RootVisualElement;
        }
    }
}
