using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    //public Player player;
    
    void Awake()
    {
        //player = new Player();
        LoadScene("MenuScene");
    }

    void Start()
    {
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
