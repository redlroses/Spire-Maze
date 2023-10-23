using System;
using UnityEngine;
using UnityEngine.Networking;

namespace CodeBase.Tools
{
    public class ImageLoader
    {
        public void LoadImage(string imageUrl, Action<Sprite> successCallback, Action errorCallback)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);

            www.SendWebRequest().completed += _ =>
            {
                if (www.result is UnityWebRequest.Result.ConnectionError
                    or UnityWebRequest.Result.ProtocolError
                    or UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.LogError("Error image loading: " + www.error);
                    errorCallback.Invoke();
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f));
                    successCallback.Invoke(sprite);
                }

                www.Dispose();
            };
        }
    }
}