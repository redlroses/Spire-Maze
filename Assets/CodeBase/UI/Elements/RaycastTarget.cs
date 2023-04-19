using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class RaycastTarget : Graphic
    {
        public override void SetMaterialDirty() { return; }
        public override void SetVerticesDirty() { return; }
    }
}