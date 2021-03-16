using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogadorController : MonoBehaviour
{
    public int velocidade = 8;
    public bool contatoChao;
    public bool espada;
    public bool espadaAereo;
    public int forcaPulo = 6;

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
        RodarAnimacoes();
        //Movimentação do jogador
        float controleJogador = Input.GetAxis("Horizontal");
        fisicaJogador.velocity = new Vector2(controleJogador * velocidade, fisicaJogador.velocity.y);
        
        //Mudança de direção do sprite e dos colisores
        if (controleJogador > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (controleJogador < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        //Pulo do jogador
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (contatoChao)
            {
                contatoChao = false;
                fisicaJogador.velocity = new Vector2(fisicaJogador.velocity.x, forcaPulo);
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
            fisicaJogador.velocity = new Vector2(direcaoDash * forcaDash,0);
            timerAtualDash -= Time.deltaTime;
            if (timerAtualDash <= 0)
            {
                dashAtivo = false;
            }
        }
        //Ataque aereo com a espada
        if (Input.GetKeyDown(KeyCode.X) && !contatoChao)
        {
            espadaAereo = true;
        }
        //Ataque com a espada
        if (Input.GetKeyDown(KeyCode.X) && contatoChao)
        {
            espada = true;
        }
    }
    void RodarAnimacoes()
    {
        if (dashAtivo)
        {
            anim.SetBool("Dash", true);
        }
        else
        {
            anim.SetBool("Dash", false);
        }
        if (espada)
        {
            anim.SetTrigger("Ataque");
            fisicaJogador.velocity = new Vector2(0, fisicaJogador.velocity.y);
            espada = false;
        }
        if (contatoChao)
        {
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
        else
        {
            anim.SetTrigger("Pulo");
            if (espadaAereo)
            {
                anim.SetBool("Ataque Aereo",true);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D objetoColidido)
    {
        if (objetoColidido.gameObject.CompareTag("Enemy"))
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
        }
    }
}
