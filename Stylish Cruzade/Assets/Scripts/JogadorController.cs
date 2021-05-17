using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.ParticleSystemJobs;

public class JogadorController : MonoBehaviour
{
    public int velocidade = 8;
    bool controleAtivo;
    bool mudancaDirecao;
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

    public bool invencivel;
    float tempoInvencibilidade = 0f;


    public int vidaJogador;
    int vidaMax;
    public bool morteQueda = false;
    public Timer cronometro;

    public LayerMask layerChao;
    public Transform posicaoPe;
    public float raioChecarChao = 0.2f;
    public bool contatoChao = false;


    Rigidbody2D fisicaJogador;
    Animator anim;
    BoxCollider2D colisorEspada;

    public AudioClip somEspadaAcertou;
    public AudioClip somEspadaErrou;
    public AudioClip somDano;
    public AudioClip somDash;
    public AudioClip somPulo;
    public AudioClip audioVitoria;
    public AudioClip audioDerrota;
    AudioSource audioJogador;

    public Image telaDeDerrota;
    public Image telaDeVitoria;
    bool ganhou;
    bool morreu;

    public ParticleSystem poeira;
    public ParticleSystem efeitoDash;
    public ParticleSystemRenderer flipEfeitoDash;

    AudioListener listener;

    public AudioListener cameraListener;

    public AudioSource musicaFase;

    bool tocouAudio = false;
    // Start is called before the first frame update
    void Start()
    {
        fisicaJogador = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        colisorEspada = GetComponent<BoxCollider2D>();
        audioJogador = GetComponent<AudioSource>();
        listener = GetComponent<AudioListener>();

        invencivel = false;

        vidaMax = 6;
        vidaJogador = vidaMax;

        ganhou = false;

        musicaFase.Play();
    }

    // Update is called once per frame
    void Update()
    {
        contatoChao = Physics2D.OverlapCircle(posicaoPe.position, raioChecarChao, layerChao);
        //Movimentação do jogador. ControleAtivo serve para travar o jogador no momento que atacar.
        float controleJogador = Input.GetAxis("Horizontal");

        //Ativação da partícula de poeira ao andar
        if (contatoChao)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                AtivarPoeira();
            }
        }
        if (controleAtivo)
        {
            fisicaJogador.velocity = new Vector2(controleJogador * velocidade, fisicaJogador.velocity.y);
        }
        //Mudança de direção do sprite e dos colisores. o mudancaDirecao serve para travar o jogador na direção que ele atacar.
        if (controleJogador > 0 && mudancaDirecao)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            //Mudança de direção da partícula de dash.
            flipEfeitoDash.flip = Vector3.zero;
        }
        if (controleJogador < 0 && mudancaDirecao)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            //Mudança de direção da partícula de dash.
            flipEfeitoDash.flip = Vector3.right;
        }


        if (contatoChao)
        {
            anim.SetBool("Chao", true);
            anim.SetBool("Ataque Aereo", false);

            qtdPulos = 0;
            if (!invencivel)
            {
                espadaAereo = false;
                controleAtivo = true;
                mudancaDirecao = true;
            }

            if (controleJogador == 0)
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
            anim.SetBool("Chao", false);
        }
        

        //Pulo do jogador
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!invencivel && qtdPulos < 2)
            {
                //Ativação da partícula de poeira ao pular
                AtivarPoeira();
                audioJogador.PlayOneShot(somPulo);
            }
            if (!invencivel && qtdPulos < 2)
            {
                anim.SetBool("Chao", false);
                anim.SetTrigger("Pulo");
                if (qtdPulos == 0)
                {
                    fisicaJogador.velocity = new Vector2(fisicaJogador.velocity.x, forcaPulo);
                }
                if (qtdPulos == 1)
                {
                    fisicaJogador.velocity = new Vector2(fisicaJogador.velocity.x, 0.8f * forcaPulo);
                    anim.SetTrigger("Pulo Duplo");
                }
                qtdPulos++;
            }
        }

        RaycastHit2D podeDash = Physics2D.Raycast(transform.position, new Vector2(controleJogador, 0), 3);

        //Dash do jogador
        if (Input.GetKeyDown(KeyCode.Z) && contadorDash < 1)
        {
            if (!podeDash.collider.CompareTag("Chao"))
            {
                if (!invencivel)
                {
                    dashAtivo = true;
                    timerAtualDash = dashTimer;
                    fisicaJogador.velocity = Vector2.zero;
                    audioJogador.PlayOneShot(somDash);
                    efeitoDash.Play();
                    if (!contatoChao)
                    {
                        contadorDash++;
                    }
                }
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
            if (!invencivel)
            {
                fisicaJogador.velocity = new Vector2(direcaoDash * forcaDash, 0);
            }
            timerAtualDash -= Time.deltaTime;
            if (timerAtualDash <= 0)
            {
                dashAtivo = false;
            }
            if (espadaAereo)
            {
                espadaAereo = false;
            }
        }
        else
        {
            anim.SetBool("Dash", false);
        }

        //Ataque aereo com a espada
        if (Input.GetKeyDown(KeyCode.X) && !contatoChao)
        {
            if (!invencivel)
            {
                if (!espadaAereo)
                {
                    audioJogador.PlayOneShot(somEspadaErrou);
                }
                espadaAereo = true;
                mudancaDirecao = false;
                
            }
        }

        if (espadaAereo)
        {
            anim.ResetTrigger("Pulo");
            anim.SetTrigger("Ataque Aereo");
            mudancaDirecao = true;
        }

        //Ataque com a espada
        if (Input.GetKeyDown(KeyCode.X) && contatoChao)
        {
            if (!invencivel)
            {
                audioJogador.PlayOneShot(somEspadaErrou);
                anim.SetBool("Ataque", true);
                espada = true;
                controleAtivo = false;
                mudancaDirecao = false;
            }
        }

        if (espada)
        {
            if (anim.GetBool("Ataque"))
            {
                fisicaJogador.velocity = new Vector2(0, fisicaJogador.velocity.y);
            }
            espada = false;
        }

        // Invencibilidade após tomar dano
        if (invencivel)
        {
            tempoInvencibilidade += Time.deltaTime;
            if (tempoInvencibilidade > 2f)
            {
                invencivel = false;
                tempoInvencibilidade = 0;
                anim.SetBool("Dano", false);
                mudancaDirecao = true;
                controleAtivo = true;

                fisicaJogador.sharedMaterial.friction = 0f;
            }
        }

        //Morte por cair do mapa
        if (morteQueda)
        {
            if (!audioJogador.isPlaying && !tocouAudio)
            {
                audioJogador.PlayOneShot(audioDerrota);
                tocouAudio = true;
            }
            musicaFase.Stop();
            fisicaJogador.gravityScale = 0;
            fisicaJogador.velocity = Vector2.zero;
            invencivel = true;
            telaDeDerrota.gameObject.SetActive(true);
            listener.enabled = false;
            cameraListener.enabled = true;
            if (Input.anyKeyDown)
            {
                Morrer();
            }
        }
        //Morte quando o tempo acabar
        if (cronometro.tempoDeJogo <= 0)
        {
            if (!audioJogador.isPlaying && !tocouAudio)
            {
                audioJogador.PlayOneShot(audioDerrota);
                tocouAudio = true;
            }
            musicaFase.Stop();
            telaDeDerrota.gameObject.SetActive(true);
            invencivel = true;
            fisicaJogador.velocity = Vector2.zero;
            fisicaJogador.isKinematic = true;
            listener.enabled = false;
            cameraListener.enabled = true;
            if (Input.anyKeyDown)
            {
                Morrer();
            }
        }
        //Em caso de vitória
        if (ganhou)
        {
            if (!audioJogador.isPlaying && !tocouAudio)
            {
                audioJogador.PlayOneShot(audioVitoria);
                tocouAudio = true;
            }
            musicaFase.Stop();
            listener.enabled = false;
            cameraListener.enabled = true;
            fisicaJogador.isKinematic = true;
            invencivel = true;
            if (Input.anyKeyDown)
            {
                Ganhar();
            }
        }
        //Em caso do jogador morrer por tomar dano
        if (morreu)
        {
            if (!audioJogador.isPlaying && !tocouAudio)
            {
                audioJogador.PlayOneShot(audioDerrota);
                tocouAudio = true;
            }
            musicaFase.Stop();
            listener.enabled = false;
            cameraListener.enabled = true;
            invencivel = true;
            fisicaJogador.velocity = Vector2.zero;
            fisicaJogador.isKinematic = true;
            if (Input.anyKeyDown)
            {
                Morrer();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D objetoColidido)
    {
        if (colisorEspada.isActiveAndEnabled)
        {
            if (objetoColidido.gameObject.CompareTag("Enemy") && objetoColidido is BoxCollider2D)
            {
                audioJogador.Stop();
                audioJogador.PlayOneShot(somEspadaAcertou);
                Destroy(objetoColidido.gameObject);
            }
        }
        else
        {
            if (!invencivel)
            {
                if ((objetoColidido.gameObject.CompareTag("Enemy") || objetoColidido.gameObject.CompareTag("Tiro Inimigo")) 
                    && objetoColidido is BoxCollider2D)
                {
                    TomarDano(objetoColidido);
                }
            }
        }
        //Caso o jogador caia do mapa
        if (objetoColidido.gameObject.CompareTag("Buraco"))
        {
            morteQueda = true;
        }
        //Caso o jagador vença
        if (objetoColidido.gameObject.CompareTag("Oculos"))
        {
            objetoColidido.gameObject.SetActive(false);
            telaDeVitoria.gameObject.SetActive(true);
            ganhou = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D objetoColidido)
    {
        if (!invencivel)
        {
            if (objetoColidido.gameObject.CompareTag("Enemy") || objetoColidido.gameObject.CompareTag("Tiro Inimigo"))
            {
                TomarDano(objetoColidido.collider);
            }
        }
    }

    void TomarDano(Collider2D Inimigo)
    {
        mudancaDirecao = false;
        controleAtivo = false;
        Vector2 direcaoRecuo = (Inimigo.transform.position - transform.position).normalized;
        Vector2 forcaRecuo = new Vector2(80 * -direcaoRecuo.x, 300);
        fisicaJogador.velocity = Vector2.zero;
        fisicaJogador.AddForce(forcaRecuo);
        anim.SetBool("Dano", true);
        audioJogador.PlayOneShot(somDano);

        invencivel = true;

        fisicaJogador.sharedMaterial.friction = 1f;
        //Morte ao perder a vida toda
        vidaJogador -= 1;
        if (vidaJogador <= 0)
        {
            telaDeDerrota.gameObject.SetActive(true);
            morreu = true;
        }
    }

    void FinalizarAtaque()
    {
        anim.SetBool("Ataque", false);
        controleAtivo = true;
        mudancaDirecao = true;
    }


    void Morrer()
    {
        SceneManager.LoadScene(0);
    }
    void Ganhar()
    {
        SceneManager.LoadScene(0);
    }

    void AtivarPoeira()
    {
        poeira.Play();
    }
}
