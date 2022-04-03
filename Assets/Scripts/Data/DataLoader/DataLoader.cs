using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace TestApp.Data
{
    public class DataLoader
    {
        public LoadHandler<Sprite> LoadSpriteAsync(string url)
        {
            var handler = new LoadHandler<Sprite>();
            Game.Instance.StartCoroutine(LoadSprite(url, handler));
            return handler;
        }

        private IEnumerator LoadSprite(string url, LoadHandler<Sprite> handler)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            www.SendWebRequest();

            while (!www.isDone)
            {
                handler.SetProgress(www.downloadProgress);
                yield return null;
            }

            if (www.result != UnityWebRequest.Result.Success)
            {
                handler.SetError(new Exception(www.error));
                yield break;
            }

            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite = ConvertTex2DToSprite(texture);
            handler.SetSuccess(sprite);
        }

        private Sprite ConvertTex2DToSprite(Texture2D texture) =>
            Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}