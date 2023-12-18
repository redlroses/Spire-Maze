using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.Tools
{
    [ExecuteInEditMode]
    public class DeleteUnused : MonoBehaviour
    {
        private readonly List<GameObject> _toDelete = new List<GameObject>();

        [Button]
        private void Delete()
        {
            DeleteDisabled(transform);

            foreach (GameObject o in _toDelete)
            {
                DestroyImmediate(o);
            }
        }

        private void DeleteDisabled(Transform parent)
        {
            foreach (Transform child in parent)
            {
                DeleteDisabled(child);
            }

            if (parent.gameObject.activeSelf == false)
                _toDelete.Add(parent.gameObject);
        }
    }
}