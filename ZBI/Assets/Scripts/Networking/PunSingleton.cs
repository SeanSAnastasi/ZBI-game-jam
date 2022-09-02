using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunSingleton<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
{
    protected static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = (T)FindObjectOfType(typeof(T));

                if (!m_instance && Application.isEditor) Debug.LogError("Instance of" + typeof(T) + " not found");

            }

            return m_instance;
        }
    }
}
