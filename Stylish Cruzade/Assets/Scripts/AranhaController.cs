using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AranhaController : MonoBehaviour
{
    public float velocidade = 3;
    int posicao;
    public float distanciaMinima = 0.5f;

    protected Rigidbody2D fisicaInimigo;
    protected SpriteRenderer spriteInimigo;
    public Transform[] pontos;
    protected Animator anim;


    public GameObject localTiro;
    float timer;
    float tempoEntreTiros = 1.2f;

    public PoolingTiro poolingDotiro;

    AudioSource audioAranha;
    public AudioClip lancaTeia;

    void Start()
    {
        fisicaInimigo = GetComponent<Rigidbody2D>();
        spriteInimigo = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioAranha = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 direcao = (pontos[posicao].position - transform.position).normalized;
        fisicaInimigo.velocity = direcao * velocidade;
        if ((pontos[posicao].position - transform.position).magnitude < distanciaMinima)
        {
            posicao++;
            if (posicao >= pontos.Length)
            {
                posicao = 0;
            }
        }
        if (direcao.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        if (direcao.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timer += Time.deltaTime;           

            if (timer >= tempoEntreTiros)
            {
                Atirar();
                timer = 0;
            }
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
        if (transform.eulerAngles.y == 180)
        {
            GameObject tiroInimigo = poolingDotiro.PegaObjeto();
            tiroInimigo.transform.position = localTiro.transform.position;
            tiroInimigo.SetActive(true);
            tiroInimigo.GetComponent<SpriteRenderer>().flipX = true;
        }
        audioAranha.PlayOneShot(lancaTeia);
    }
}
