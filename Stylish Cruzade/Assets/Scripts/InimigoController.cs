using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoController : MonoBehaviour
{
    public float velocidade = 3;
    int posicao;
    public float distanciaMinima = 0.5f;

    protected Rigidbody2D fisicaInimigo;
    protected SpriteRenderer spriteInimigo;
    public Transform[] pontos;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        fisicaInimigo = GetComponent<Rigidbody2D>();
        spriteInimigo = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected virtual void Update()
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
}
