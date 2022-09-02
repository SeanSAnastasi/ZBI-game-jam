using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class GameLogic : MonoBehaviour
{

    //cashed
    NetworkedGameLogic networkedGameLogic;

    [Space(5)]
    [Header("Input fields")]
    public TMP_InputField questionInput;
    public TMP_InputField promptInput;


    [Space(5)]
    [Header("Screens")]
    public GameObject questionScreen;
    public GameObject promptScreen;
    public GameObject waitingScreen;
    public GameObject votingScreen;
    public GameObject scoreScreen;
    public GameObject resultScreen;
    public GameObject messageCanvas;

    [Space(5)]
    [Header("Players")]
    public TextMeshProUGUI player1Name;
    public TextMeshProUGUI player2Name;
    public TextMeshProUGUI player3Name;
    public TextMeshProUGUI player4Name;
    public List<Image> sprites;

    [Space(5)]
    [Header("UI")]
    public TextMeshProUGUI promptText;


    // Start is called before the first frame update
    void Start()
    {
        networkedGameLogic = NetworkedGameLogic.Instance;
    }

    public void OnQuestionButtonPressed()
    {
        if (string.IsNullOrEmpty(questionInput.text))
        {
            StartCoroutine(OnMessage("Your question is not valid."));
        }
        else
        {
            questionScreen.SetActive(false);
            waitingScreen.SetActive(true);

            string question = questionInput.GetComponent<TMP_InputField>().text;
            networkedGameLogic.SendQuestionToMaster(question);
        }
    }

    public void OnPromptButtonPressed()
    {
        if (string.IsNullOrEmpty(promptInput.text))
        {
            StartCoroutine(OnMessage("Your prompt is not valid."));
        }
        else
        {
            promptScreen.SetActive(false);
            waitingScreen.SetActive(true);

            string prompt = promptInput.GetComponent<TMP_InputField>().text;
            networkedGameLogic.SendPromptsToMaster(prompt);
        }
    }

    public void OnAllQuestionsReady()
    {
        waitingScreen.SetActive(false);
        promptScreen.SetActive(true);

        KeyValuePair<string, string> question = networkedGameLogic.GetCurrentQuestion();
        promptText.text = question.Value;
    }

    public void OnAllPromptsReady()
    {
        waitingScreen.SetActive(false);
        votingScreen.SetActive(true);

        if (!PhotonNetwork.IsMasterClient) return;

        for (int i = 0; i < 2; i++)
        {
            Debug.Log(networkedGameLogic.prompts[PhotonNetwork.PlayerList[i].NickName]);
            networkedGameLogic.Download(networkedGameLogic.prompts[PhotonNetwork.PlayerList[i].NickName], sprites[i]);
        }

    }

    IEnumerator OnMessage(string message)
    {
        messageCanvas.SetActive(true);
        messageCanvas.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        yield return new WaitForSeconds(1.5f);
        messageCanvas.SetActive(false);
    }
}
