using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameLogic : MonoBehaviour
{

    //cashed
    NetworkedGameLogic networkedGameLogic;

    GameObject QuestionScreen;
    GameObject QuestionInput;
    GameObject QuestionButton;

    GameObject WaitingScreen;

    // Start is called before the first frame update
    void Start()
    {
        networkedGameLogic = NetworkedGameLogic.Instance;

        Transform QuestionScreenTransform = transform.Find("QuestionScreen");
        QuestionScreen = QuestionScreenTransform.gameObject;
        QuestionInput = QuestionScreenTransform.Find("QuestionInput").gameObject;
        QuestionButton = QuestionScreenTransform.Find("QuestionButton").gameObject;

        QuestionButton.GetComponent<Button>().onClick.AddListener(OnQuestionButtonPressed);
    }

    void OnQuestionButtonPressed()
    {
        QuestionScreen.SetActive(false);

        string question = QuestionInput.GetComponent<TextMeshProUGUI>().text;
        networkedGameLogic.SendQuestionToMaster(question);
    }
}
