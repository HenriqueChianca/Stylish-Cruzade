using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogadorController : MonoBehaviour
{
    public int velocidade = 8;
    bool controleAtivo;
    bool mudancaDirecao;
    public bool contatoChao;
    public bool espada;
    public bool espadaAereo;
    public float forcaPulo = 6;

    public int qtdPulos;

    public int forcaDash = 10;
    public bool dashAtivo;
    public float dashTimer = 0.25f;
    float timerAtualDash;
    public float direcaoDash;
    int contadorDash;

    Rigidbody2D fisicaJogador;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        fisicaJogador = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //Movimentação do jogador. ControleAtivo serve para travar o jogador no momento que atacar.
        float controleJogador = Input.GetAxis("Horizontal");
        if (controleAtivo)
        {
            fisicaJogador.velocity = new Vector2(controleJogador * velocidade, fisicaJogador.velocity.y);
        }
        //Mudança de direção do sprite e dos colisores. o mudancaDirecao serve para travar o jogador na direção que ele atacar.
        if (controleJogador > 0 && mudancaDirecao)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (controleJogador < 0 && mudancaDirecao)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (contatoChao)
        {
            anim.ResetTrigger("Pulo Duplo");
            anim.SetTrigger("Chao");
            anim.SetBool("Ataque Aereo", false);
            if (fisicaJogador.velocity.x == 0)
            {
                anim.SetBool("Andando", false);
            }
            else
            {
                anim.SetBool("Andando", true);
            }
        }

        //Pulo do jogador
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (contatoChao)
            {
                qtdPulos++;
                contatoChao = false;
                fisicaJogador.velocity = new Vector2(fisicaJogador.velocity.x, forcaPulo);
            }
            else
            {
                //Pulo duplo
                if (qtdPulos < 2)
                {
                    fisicaJogador.velocity = new Vector2(fisicaJogador.velocity.x, 0.8f * forcaPulo);
                    qtdPulos++;
                    anim.SetTrigger("Pulo Duplo");
                }
            }
        }
        if (!contatoChao)
        {
            if (qtdPulos < 2)
            {
                anim.SetTrigger("Pulo");
            }
        }

        //Dash do jogador
        if (Input.GetKeyDown(KeyCode.Z) && contadorDash < 1)
        {
            dashAtivo = true;
            timerAtualDash = dashTimer;
            fisicaJogador.velocity = Vector2.zero;
            if (!contatoChao)
            {
                contadorDash++;
            }
        }
        if (contatoChao)
        {
            contadorDash = 0;
        }
        if (dashAtivo)
        {
            if (controleJogador > 0 || transform.eulerAngles.y == 0)
            {
                direcaoDash = 1;
            }
            if (controleJogador < 0 || transform.eulerAngles.y == 180)
            {
                direcaoDash = -1;
            }
            anim.SetBool("Dash", true);
            fisicaJogador.velocity = new Vector2(direcaoDash * forcaDash,0);
            timerAtualDash -= Time.deltaTime;
            if (timerAtualDash <= 0)
            {
                dashAtivo = false;
            }
        }
        else
        {
            anim.SetBool("Dash", false);
        }

        //Ataque aereo com a espada
        if (Input.GetKeyDown(KeyCode.X) && !contatoChao)
        {
            espadaAereo = true;
            mudancaDirecao = false;
        }

        if (espadaAereo)
        {
            anim.SetBool("Ataque Aereo", true);
        }

        //Ataque com a espada
        if (Input.GetKeyDown(KeyCode.X) && contatoChao)
        {
            espada = true;
            controleAtivo = false;
            mudancaDirecao = false;
        }

        if (espada)
        {
            anim.SetTrigger("Ataque");
            fisicaJogador.velocity = new Vector2(0, fisicaJogador.velocity.y);
            espada = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D objetoColidido)
    {
        if (objetoColidido.gameObject.CompareTag("Enemy") && objetoColidido is BoxCollider2D)
        {
            Destroy(objetoColidido.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D objetoColidido)
    {
        if (objetoColidido.gameObject.CompareTag("Chao"))
        {
            contatoChao = true;
            espadaAereo = false;
            controleAtivo = true;
            mudancaDirecao = true;
            qtdPulos = 0;
        }
    }
}
