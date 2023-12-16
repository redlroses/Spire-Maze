using CodeBase.Services;
using CodeBase.Services.SaveLoad;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    public class Tools
    {
        [MenuItem("Tools/Save")]
        private static void Save()
        {
            AllServices.Container.Single<ISaveLoadService>().SaveProgress();
        }

        [MenuItem("Tools/ClearPrefs")]
        private static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}