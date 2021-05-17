using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{

    public float tempoDeJogo;
    int tempoint;
    public TextMeshProUGUI cronometro;

    // Start is called before the first frame update
    void Start()
    {
        tempoDeJogo = 120;
    }

    // Update is called once per frame
    void Update()
    {
        tempoDeJogo = tempoDeJogo - Time.deltaTime;
        tempoint = Mathf.RoundToInt(tempoDeJogo);
        cronometro.text = tempoint.ToString("000");

        if (tempoint <= 0)
        {
            tempoint = 0;
        }
    }
}
