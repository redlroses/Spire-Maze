using UnityEngine;

namespace CodeBase.Infrastructure.FileRead
{
    public class Reader
    {
        protected string ReadText(string path)
        {
            string textFile = Resources.Load(path).ToString();
            return textFile;
        }

        protected TFile ReadJson<TFile>(string path) where TFile : MonoBehaviour
        {
            string resource = Resources.Load(path).ToString();
            TFile result = JsonUtility.FromJson<TFile>(resource);
            return result;
        }
    }
}
