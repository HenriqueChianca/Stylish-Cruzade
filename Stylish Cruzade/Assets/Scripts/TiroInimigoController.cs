using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiroInimigoController : MonoBehaviour
{
    Rigidbody2D fisicaTiro;
    public int velocidadeTiro = 5;
    // Start is called before the first frame update
    void Start()
    {
        fisicaTiro = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        fisicaTiro.velocity = new Vector2(velocidadeTiro, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision is CapsuleCollider2D)
        {
            Destroy(gameObject);
        }
    }
}
