using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BlinkText : MonoBehaviour
{
    public float speed = 1.0f;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        TextMeshProUGUI tmp = GetComponent<TextMeshProUGUI>();

        while (this.isActiveAndEnabled)
        {
            if (tmp.alpha > 0.0f) tmp.alpha = 0.0f;
            else tmp.alpha = 1.0f;

            yield return new WaitForSeconds(speed);
        }

    }
}
