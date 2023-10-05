using UnityEngine;

namespace CodeBase.Logic
{
    public interface IHeroLocator
    {
        Transform Location { get; }
    }
}