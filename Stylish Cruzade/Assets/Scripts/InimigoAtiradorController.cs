using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoAtiradorController : InimigoController
{
    [Header("As variáveis abaixo são exclusivas de inimigos que atiram")]
    public GameObject localTiro;
    Transform alvo;
    float distanciay = 2f;
    float timer;
    float tempoEntreTiros = 1.2f;

    public PoolingTiro poolingDotiro;


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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timer += Time.deltaTime;
            alvo = collision.transform;
            //movimentacao é o novo movimento do inimigo quando o jogador vira o alvo
            Vector2 movimentacao = new Vector2(0, alvo.position.y - transform.position.y).normalized;
            fisicaInimigo.velocity = movimentacao * velocidade;

            if (Mathf.Abs(alvo.transform.position.y - transform.position.y) < distanciay && timer >= tempoEntreTiros)
            {
                Atirar();
                timer = 0;
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
            GameObject tiroInimigo = poolingDotiro.PegaObjeto();
            tiroInimigo.transform.position = localTiro.transform.position;
            tiroInimigo.SetActive(true);
            tiroInimigo.GetComponent<TiroInimigoController>().velocidadeTiro *= -1;
        }
        if(transform.eulerAngles.y == 180)
        {
            GameObject tiroInimigo = poolingDotiro.PegaObjeto();
            tiroInimigo.transform.position = localTiro.transform.position;
            tiroInimigo.SetActive(true);
            tiroInimigo.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}
