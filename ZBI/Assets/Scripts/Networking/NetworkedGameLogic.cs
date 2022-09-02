using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkedGameLogic : PunSingleton<NetworkedGameLogic>
{
    [HideInInspector]
    public Player[] activePlayers;
    [HideInInspector]
    public SortedDictionary<Player, string> questions;
    [HideInInspector]
    public int currentQuestionIndex;
    [HideInInspector]
    public SortedDictionary<Player, string> prompts;
    [HideInInspector]
    public SortedDictionary<Player, Sprite> GeneratedSprites;
    [HideInInspector]
    public SortedDictionary<Player, int> Scores;

    public static NetworkedGameLogic Instance { get; private set; }

    private GameLogic gameLogic;

    #region Public Methods
    public void StartGame()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        activePlayers = PhotonNetwork.PlayerList;
        questions = new SortedDictionary<Player, string>();
        currentQuestionIndex = 0;
        prompts = new SortedDictionary<Player, string>();
        Debug.Log("Starting game!");

        LoadGameScene("GameScene");
    }

    public KeyValuePair<Player, string> GetCurrentQuestion()
    {
        KeyValuePair<Player, string> currentQuestion = questions.ToArray()[currentQuestionIndex];
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

    public void SendQuestionPromptsToMaster(string prompt)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("ReceivePromptFromClient", RpcTarget.MasterClient, prompt);
    }

    public void SendScoreToMaster(int Score)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("ReceiveScoreFromClient", RpcTarget.MasterClient, Score);
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

    private void Download(string url, Player player)
    {
        StartCoroutine(LoadFromWeb(url, player));
    }

    IEnumerator LoadFromWeb(string url, Player player)
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

            GeneratedSprites.Add(player, sprite);
        }
    }

    [PunRPC]
    private void ReceiveQuestionFromClient(string question, Player client)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        questions.Add(client, question);

        if (activePlayers.Length <= questions.Count)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SyncQuestions", RpcTarget.All, questions);
        }
    }

    [PunRPC]
    private void ReceivePromptFromClient(string prompt, Player client)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        prompts.Add(client, prompt);

        //TODO: save prompts, generate images, sync everything to all clients

        if (activePlayers.Length <= prompts.Count)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SyncQuestions", RpcTarget.All, questions);
        }
    }

    [PunRPC]
    private void ReceiveScoreFromClient(int score, Player client)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Scores.Add(client, score);

        //TODO: Sync score with all clients

        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("SyncScores", RpcTarget.All, Scores);
    }

    [PunRPC]
    private void SyncQuestions(SortedDictionary<Player, string> questions)
    {
        this.questions = questions;


        foreach (var item in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (item.name == "UI")
            {
                gameLogic = item.GetComponent<GameLogic>();
            }
        }
        gameLogic.OnAllQuestionsReady();
    }

    [PunRPC]
    private void SyncPrompts(SortedDictionary<Player, string> prompts)
    {
        this.prompts = prompts;
    }

    [PunRPC]
    private void SyncSprites(SortedDictionary<Player, Sprite> GeneratedImages)
    {
        this.GeneratedSprites = GeneratedImages;
    }

    [PunRPC]
    private void SyncScores(SortedDictionary<Player, int> Scores)
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
