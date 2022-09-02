using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LimitInputCharacters : MonoBehaviour
{
    public int charactersLimit = 50;

    void Update()
    {
        if(this.GetComponent<TMP_InputField>().text.Length > charactersLimit) 
        {
            this.GetComponent<TMP_InputField>().text = this.GetComponent<TMP_InputField>().text.Remove(charactersLimit);
        }

    }
}
