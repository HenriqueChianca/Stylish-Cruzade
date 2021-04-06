using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    float tempoDeJogo;
    public Text cronometro;

    // Start is called before the first frame update
    void Start()
    {
        tempoDeJogo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        tempoDeJogo = tempoDeJogo + Time.deltaTime;
        cronometro.text = tempoDeJogo.ToString("F2");
    }
}
