using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiroInimigoController : MonoBehaviour
{
    BoxCollider2D colisorTiro;
    Rigidbody2D fisicaTiro;
    public int velocidadeTiro = 5;
    // Start is called before the first frame update
    void Start()
    {
        colisorTiro = GetComponent<BoxCollider2D>();
        fisicaTiro = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        fisicaTiro.velocity = new Vector2(velocidadeTiro, 0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.collider is CapsuleCollider2D)
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            colisorTiro.isTrigger = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
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
