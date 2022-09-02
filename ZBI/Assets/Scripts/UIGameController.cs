using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGameController : MonoBehaviour
{
    public GameObject questionScreen;
    public TMP_InputField questionInput;
    public GameObject promptScreen;
    public TMP_InputField promptInput;
    public GameObject waitingScreen;
    public GameObject votingScreen;
    public GameObject scoreScreen;
    public GameObject resultScreen;
    public GameObject messageCanvas;

    public void OnQuestionSubmit()
    {
        if(!string.IsNullOrEmpty(questionInput.text))
        {
            promptScreen.SetActive(true);
            questionScreen.SetActive(false);
        }
        else
        {
            StartCoroutine(OnMessage("Your question is not valid."));
        }
    }

    public void OnPromptSubmit()
    {
        if(!string.IsNullOrEmpty(promptInput.text))
        {
            waitingScreen.SetActive(true);
            promptScreen.SetActive(false);
        }
        else
        {
            StartCoroutine(OnMessage("Your prompt is not valid."));
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
