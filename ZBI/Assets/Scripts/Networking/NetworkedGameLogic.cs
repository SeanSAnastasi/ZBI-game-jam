using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkedGameLogic : PunSingleton<NetworkedGameLogic>
{
    [HideInInspector]
    public Player[] activePlayers;
    [HideInInspector]
    public Dictionary<string, string> questions;
    [HideInInspector]
    public int currentQuestionIndex;
    [HideInInspector]
    public Dictionary<string, string> prompts;
    [HideInInspector]
    public Dictionary<string, Sprite> GeneratedSprites;
    [HideInInspector]
    public Dictionary<string, int> Scores;

    public static NetworkedGameLogic Instance { get; private set; }
    public string puppyUrl = "https://i.pinimg.com/originals/c1/81/dc/c181dc51de2b255351e639bff4c3ebec.jpg";

    private GameLogic gameLogic;

    #region Public Methods
    public void StartGame()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        activePlayers = PhotonNetwork.PlayerList;
        questions = new Dictionary<string, string>();
        currentQuestionIndex = 0;
        prompts = new Dictionary<string, string>();
        Debug.Log("Starting game!");


        PhotonNetwork.CurrentRoom.IsOpen = false;
        LoadGameScene("GameScene");
    }

    public KeyValuePair<string, string> GetCurrentQuestion()
    {
        KeyValuePair<string, string> currentQuestion = questions.ToArray()[currentQuestionIndex];
        return currentQuestion;
    }
    public bool IsGenerationDone()
    {
        return GeneratedSprites.Count >= activePlayers.Length;
    }

    public void SendQuestionToMaster(string question)
    {
        photonView.RPC("ReceiveQuestionFromClient", RpcTarget.MasterClient, question, PhotonNetwork.LocalPlayer);
    }

    public void SendPromptsToMaster(string prompt)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("ReceivePromptFromClient", RpcTarget.MasterClient, prompt, PhotonNetwork.LocalPlayer);
    }

    public void SendScoreToMaster(int Score)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("ReceiveScoreFromClient", RpcTarget.MasterClient, Score, PhotonNetwork.LocalPlayer);
    }

    public void LoadGameScene(string sceneName)
    {
        photonView.RPC("LoadScenePerUser", RpcTarget.All, sceneName);
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        for (int i = 0; i < activePlayers.Length; i++)
        {
            if (activePlayers[i] == otherPlayer)
            {
                activePlayers[i] = activePlayers[i + 1];
            }
            Array.Resize(ref activePlayers, activePlayers.Length - 1);
        }
    }

    #endregion

    #region Private Methods

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Network Manager instance in the scene. Destroying the newest one");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void GenerateSprite(string prompt)
    {

    }

    public void Download(string prompt, Image image)
    {
        FindObjectOfType<Diffusion>().Download(prompt, image);
    }

    IEnumerator LoadFromWeb(string url, Player player, Image image)
    {
        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        yield return wr.SendWebRequest();
        if (wr.result == UnityWebRequest.Result.Success)
        {
            Texture2D t = texDl.texture;
            Sprite sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height),
                Vector2.zero, 1f);
            image.sprite = sprite;
            GeneratedSprites.Add(player.NickName, sprite);
        }
    }

    [PunRPC]
    private void ReceiveQuestionFromClient(string question, Player client)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        questions.Add(client.NickName, question);

        if (activePlayers.Length <= questions.Count)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SyncQuestions", RpcTarget.All, questions);

            //FindObjectOfType<GameLogic>().OnAllQuestionsReady();
        }
    }

    [PunRPC]
    private void ReceivePromptFromClient(string prompt, Player client)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        prompts.Add(client.NickName, prompt);

        //TODO: save prompts, generate images, sync everything to all clients

        if (activePlayers.Length <= prompts.Count)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SyncPrompts", RpcTarget.All, questions);
        }
    }

    [PunRPC]
    private void ReceiveScoreFromClient(int score, Player client)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Scores.Add(client.NickName, score);

        //TODO: Sync score with all clients

        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("SyncScores", RpcTarget.All, Scores);
    }

    [PunRPC]
    private void SyncQuestions(Dictionary<string, string> questions)
    {
        this.questions = questions;

        FindObjectOfType<GameLogic>().OnAllQuestionsReady();
    }

    [PunRPC]
    private void SyncPrompts(Dictionary<string, string> prompts)
    {
        this.prompts = prompts;
        FindObjectOfType<GameLogic>().OnAllPromptsReady();
    }

    [PunRPC]
    private void SyncSprites(Dictionary<string, Sprite> GeneratedImages)
    {
        this.GeneratedSprites = GeneratedImages;
    }

    [PunRPC]
    private void SyncScores(Dictionary<string, int> Scores)
    {
        this.Scores = Scores;
    }

    [PunRPC]
    private void LoadScenePerUser(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    #endregion
}
