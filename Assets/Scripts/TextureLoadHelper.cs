using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace CCGTestTask
{
    public class TextureLoadHelper : MonoBehaviour
    {
        [SerializeField] private string _imageURL;

        public IEnumerator LoadCardArtIMage(RawImage _cardArtImage)
        {
            UnityWebRequest url = UnityWebRequestTexture.GetTexture(_imageURL);

            yield return url.SendWebRequest();

            if (url.error == null)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(url);
                _cardArtImage.texture = texture;
            }
            else
            {
                Debug.Log("Connection Error");
            }
        }
    }
}