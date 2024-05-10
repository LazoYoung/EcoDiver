using UnityEngine.UIElements;

namespace FlexXR.Runtime.FlexXRPanel
{
    public class FlexXRContainer : VisualElement
    {
        [UnityEngine.Scripting.Preserve]
        public new class UxmlFactory : UxmlFactory<FlexXRContainer, UxmlTraits>
        {
        }

        [UnityEngine.Scripting.Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }

        public FlexXRContainer()
        {
        }
    }
}