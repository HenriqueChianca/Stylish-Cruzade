using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidaJogador : MonoBehaviour
{
    public int vida;

    public List<Image> coracoes;
    public Sprite coracaoCheio;
    public Sprite coracaoMetade;
    public Sprite coracaoVazio;
    public Image telaDeDerrota;

    // Start is called before the first frame update
    void Start()
    {
        vida = GetComponent<JogadorController>().vidaJogador;
        for (int i = 0; i < coracoes.Count; i++)
        {
            if (i < vida)
            {
                coracoes[i].sprite = coracaoCheio;
            }
            else
            {
                coracoes[i].sprite = coracaoVazio;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MostrarVida();
    }

    void MostrarVida()
    {
        vida = GetComponent<JogadorController>().vidaJogador;

        if (vida == 6)
        {
            coracoes[0].sprite = coracaoCheio;
            coracoes[1].sprite = coracaoCheio;
            coracoes[2].sprite = coracaoCheio;
        }
        if (vida == 5)
        {
            coracoes[0].sprite = coracaoCheio;
            coracoes[1].sprite = coracaoCheio;
            coracoes[2].sprite = coracaoMetade;
        }
        if (vida == 4)
        {
            coracoes[0].sprite = coracaoCheio;
            coracoes[1].sprite = coracaoCheio;
            coracoes[2].sprite = coracaoVazio;
        }
        if (vida == 3)
        {
            coracoes[0].sprite = coracaoCheio;
            coracoes[1].sprite = coracaoMetade;
            coracoes[2].sprite = coracaoVazio;
        }
        if (vida == 2)
        {
            coracoes[0].sprite = coracaoCheio;
            coracoes[1].sprite = coracaoVazio;
            coracoes[2].sprite = coracaoVazio;
        }
        if (vida == 1)
        {
            coracoes[0].sprite = coracaoMetade;
            coracoes[1].sprite = coracaoVazio;
            coracoes[2].sprite = coracaoVazio;
        }
        if (vida == 0)
        {
            coracoes[0].sprite = coracaoVazio;
            coracoes[1].sprite = coracaoVazio;
            coracoes[2].sprite = coracaoVazio;
            telaDeDerrota.gameObject.SetActive(true);
        }
    }
}
