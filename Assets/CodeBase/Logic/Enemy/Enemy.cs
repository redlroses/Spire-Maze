using UnityEngine;
using NTC.Global.Cache;
using CodeBase.Logic.Movement;
using CodeBase.Tools;

namespace CodeBase.Logic.Enemy
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Jumper))]
    public class Enemy : MonoCache, IEnemy
    {
        [SerializeField]
        [RequireInterface(typeof(Mover))] private MonoCache _mover;

        private

        private void Initialize()
        {

        }
    }
}