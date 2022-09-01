using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    VisualElement rootVisualElement;

    void OnEnable() 
    {
        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        rootVisualElement.Q<Button>("continueButton").RegisterCallback<ClickEvent>(ev=> OnContinueButton());
        StartAnimation();
    }

    void OnContinueButton()
    {
        LoadScene("gameScene");
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    void StartAnimation()
    {
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        while(true)
        {
            rootVisualElement.Q<VisualElement>("textContainer").Q<TextElement>().ToggleInClassList("popUpAnimation");

            yield return new WaitForSeconds(0.5f);

             
        }
    }
}
