using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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

    string pred_endpoint = "https://api.replicate.com/v1/predictions";
    string api_key = "411758a7bd9e0c142c9c56a4a2fea3d7833c0195";
    string model_version = "a9758cbfbd5f3c2094457d996681af52552901775aa2d6dd0b17fd15df959bef";

    Sprite _latestSprite;

    public void Download(string prompt, Image image)
    {
        DoDaGenerate(prompt, image);
        //StartCoroutine(LoadFromWeb(url));
    }



    public void DoDaGenerate(string prompt, Image image)
    {
        string url = pred_endpoint;
        string json_input = "";
        //JsonUtility.FromJson<jsonInput>(json_input);
        json_input = "{\"version\": \"a9758cbfbd5f3c2094457d996681af52552901775aa2d6dd0b17fd15df959bef\", \"input\":{\"prompt\": \""+ prompt +"\", \"num_diffusion_steps\": \"1\"}}";
        //json_input = JsonUtility.ToJson(new jsonInput());
        string headers = "Token " + api_key;

        Debug.Log(json_input);
        StartCoroutine(Post(url, json_input, headers, image));

    }

    IEnumerator Post(string url, string bodyJsonString, string headers, Image image)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Authorization", headers);
        request.SetRequestHeader("Content-Type", "application/json");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        Debug.Log(request.result);
        Debug.Log(request.downloadHandler.text);
        Debug.Log("Status Code: " + request.responseCode);

        //ApiResponse response = ApiResponse.CreateFromJSON(request.downloadHandler.text);
        Regex regex = new Regex("https.*?,");
        MatchCollection matches = regex.Matches(request.downloadHandler.text);
        Debug.Log(matches.Count);
        Debug.Log(matches[0].Value.Substring(0, matches[0].Value.Length-2));

        string url2 = matches[0].Value.Substring(0, matches[0].Value.Length - 2);
        var request2 = new UnityWebRequest(url2, "GET");
        request2.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request2.SetRequestHeader("Authorization", headers);
        request2.SetRequestHeader("Content-Type", "application/json");
        //request2.SetRequestHeader("Transfer-Encoding", "chunked");
        yield return request2.SendWebRequest();
        Debug.Log(request2.result);
        Debug.Log(request2.downloadHandler.text);
        Debug.Log("Status Code: " + request2.responseCode);
        int sendcounter = 0;
        var request3 = new UnityWebRequest(url2, "GET");
        request3.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request3.SetRequestHeader("Authorization", headers); ;
        while (request2.downloadHandler.text.Contains("processing") || request2.downloadHandler.text.Contains("starting"))
        {
            sendcounter++;
            if (sendcounter % 1000 == 0)
            {
                request3 = new UnityWebRequest(url2, "GET");
                request3.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request3.SetRequestHeader("Authorization", headers);
                request3.SetRequestHeader("Content-Type", "application/json");
                yield return request3.SendWebRequest();
            }
            Debug.Log(request3.downloadHandler.text);
            yield return null;
            if (request3.downloadHandler.text.Contains("succeeded")) break;
        }

        regex = new Regex("https(.|\n)*?,");
        matches = regex.Matches(request3.downloadHandler.text);
        Debug.Log(matches.Count);
        Debug.Log(matches[2].Value.Substring(0, matches[0].Value.Length - 3));
        string[] parts = request3.downloadHandler.text.Split(new string[] { "[" }, StringSplitOptions.None);
        Debug.Log(parts.Length);
        Debug.Log(parts[1]);

        string maybeurl = parts[1].Split("]")[0];
        string imageurl = maybeurl.Substring(1, maybeurl.Length-2);
        Debug.Log(imageurl);

        StartCoroutine(LoadFromWeb(imageurl, image));
        //yield return imageurl;
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", "myData");

        using (UnityWebRequest www = UnityWebRequest.Post("https://www.my-server.com/myform", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }

    IEnumerator LoadFromWeb(string url, Image image)
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
            image.sprite = _latestSprite;
        }
    }
    public class jsonInput
    {
        public string version = "a9758cbfbd5f3c2094457d996681af52552901775aa2d6dd0b17fd15df959bef";
        public string input = JsonUtility.ToJson(new Input());
    }
    public class Input
    {
        public string prompt = "bananna";
        public int num_diffusion_steps = 50;
    }

    public class ApiResponse
    {
        public string id;
        public string version;
        public urls urls;

        public static ApiResponse CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ApiResponse>(jsonString);
        }
    }

    public class urls
    {
        public string get;
        public string cancel;
    }
}




