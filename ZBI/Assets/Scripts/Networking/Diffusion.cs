using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Diffusion : MonoBehaviour
{
    // README
    //This is an example, there will probably need to be a bunch of changes.
    //I have temporarely moved this functionality to the NetworkedGameLogic class because I needed acess to the data there for now
    //Feel free to use this class for the final implementation
    //You can always ask me the reasoning -Rune

    public string puppyUrl = "https://i.pinimg.com/originals/c1/81/dc/c181dc51de2b255351e639bff4c3ebec.jpg";

    Sprite _latestSprite;

    public void Download(string url)
    {
        StartCoroutine(LoadFromWeb(url));
    }

    IEnumerator LoadFromWeb(string url)
    {
        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        yield return wr.SendWebRequest();
        if (wr.result == UnityWebRequest.Result.Success)
        {
            Texture2D t = texDl.texture;
            _latestSprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height),
                Vector2.zero, 1f);
        }
    }
}
