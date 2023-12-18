using UnityEngine;

namespace CodeBase.Logic
{
    public interface IGameObject
    {
        GameObject GameObject => (this as MonoBehaviour)!.gameObject;
    }
}