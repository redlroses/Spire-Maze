using NTC.Global.Cache;

namespace CodeBase.Logic.Portal
{
    public class Teleporter : MonoCache, ITeleporter
    {
        public void Teleport(Portal portal)
        {
            portal.SetRecipet();
            transform.position = portal.transform.position;
            transform.rotation = portal.transform.rotation;
        }
    }
}
