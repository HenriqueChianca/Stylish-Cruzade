using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiroInimigoController : MonoBehaviour
{
    SpriteRenderer direcaoTiro;
    BoxCollider2D colisorTiro;
    Rigidbody2D fisicaTiro;
    public int velocidadeTiro = 5;

    // Start is called before the first frame update
    void OnEnable()
    {
        direcaoTiro = GetComponent<SpriteRenderer>();
        colisorTiro = GetComponent<BoxCollider2D>();
        fisicaTiro = GetComponent<Rigidbody2D>();
        Invoke(nameof(Desativar), 3.5f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    // Update is called once per frame
    void Update()
    {
        fisicaTiro.velocity = new Vector2(velocidadeTiro, 0);
    }

    void Desativar()
    {
        gameObject.SetActive(false);
        velocidadeTiro = 5;
        direcaoTiro.flipX = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.collider is CapsuleCollider2D)
        {
            Desativar();
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            colisorTiro.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision is CircleCollider2D)
        {
            colisorTiro.isTrigger = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && colisorTiro.isTrigger && collision is BoxCollider2D)
        {
            colisorTiro.isTrigger = false;
        }
    }
}
