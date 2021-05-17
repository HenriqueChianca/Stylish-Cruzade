using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingPlataforma : MonoBehaviour
{
    public PoolingTiro pooling;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AparecerPlataforma();
    }

    void AparecerPlataforma()
    {
        GameObject plataforma = pooling.PegaObjeto();
        if (plataforma != null)
        {
            plataforma.transform.position = transform.position;
            plataforma.SetActive(true);
            plataforma.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }
}
