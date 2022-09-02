using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalScore : MonoBehaviour
{

    public static SingleScore[] scores = new SingleScore[]{
        new SingleScore(10, "Sean"),
        new SingleScore(20, "Joe"),
        new SingleScore(15, "Hello"),
        new SingleScore(25, "World")
    };
    
    public TMP_Text[] texts;

    // Start is called before the first frame update
    void Start()
    {
        SingleScore[] sorted = SortScoreArray(scores);
        
        for(int i=0; i< sorted.Length ; i++){
            string outputText = sorted[i].name+": "+sorted[i].score;
            texts[i].text = outputText;

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public SingleScore[] SortScoreArray(SingleScore[] array){
        return array.OrderByDescending( x => x.score ).ToArray();
    }         
}


