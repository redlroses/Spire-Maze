using UnityEngine;

namespace CodeBase.Logic
{
    public interface IGameObject
    {
        GameObject GameObject => ((MonoBehaviour)this).gameObject;
    }
}