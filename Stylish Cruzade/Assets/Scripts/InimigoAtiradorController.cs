using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoAtiradorController : InimigoController
{
    [Header("As variáveis abaixo são exclusivas de inimigos que atiram")]
    public GameObject tiro;
    public GameObject localTiro;
    Transform alvo;
    float distanciay = 2f;
    float timer;
    float tempoEntreTiros = 2.5f;


    // Update is called once per frame
    protected override void Update()
    {
        if (alvo == null)
        {
            base.Update();
        }
        else
        {
            Vector2 direcao = (alvo.position - transform.position).normalized;
            if (direcao.x > 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            if (direcao.x < 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Atirar();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        timer += Time.deltaTime;
        if (collision.CompareTag("Player"))
        {
            alvo = collision.transform;
            //movimentacao é o novo movimento do inimigo quando o jogador vira o alvo
            Vector2 movimentacao = new Vector2(0, alvo.position.y - transform.position.y).normalized;
            fisicaInimigo.velocity = movimentacao * velocidade;

            if (Mathf.Abs(alvo.transform.position.y - transform.position.y) < distanciay && timer >= tempoEntreTiros)
            {
                Atirar();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            alvo = null;
        }
    }

    void Atirar()
    {
        if (transform.eulerAngles.y == 0)
        {
            GameObject tiroInimigo = Instantiate(tiro, localTiro.transform.position, Quaternion.identity);
            tiroInimigo.GetComponent<TiroInimigoController>().velocidadeTiro *= -1;
            timer = 0f;
        }
        else
        {
            GameObject tiroInimigo = Instantiate(tiro, localTiro.transform.position, Quaternion.identity);
            tiroInimigo.GetComponent<SpriteRenderer>().flipX = true;
            timer = 0f;
        }
    }
}
