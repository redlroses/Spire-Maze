﻿using UnityEngine;

namespace CodeBase.Logic.Portal
{
    public interface ITeleportable
    {
        public Vector3 Forward { get; }
        public void Teleportation(Vector3 position, Vector3 rotation);
    }
}