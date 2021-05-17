using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorcegoController : MonoBehaviour
{
    Transform alvo;

    public float velocidade;

    Rigidbody2D fisicaInimigo;

    // Start is called before the first frame update
    void Start()
    {
        fisicaInimigo = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alvo != null)
        {
            Vector2 direcao = (alvo.position - transform.position).normalized;
            fisicaInimigo.velocity = direcao * velocidade;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            alvo = collision.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            alvo = null;
        }
    }
}
