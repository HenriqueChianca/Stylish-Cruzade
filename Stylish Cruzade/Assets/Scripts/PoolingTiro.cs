using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingTiro : MonoBehaviour
{
    public GameObject objetoParaInstanciar;
    public int comecaInstanciado = 4;

    public List<GameObject> listaDeObjetos;

    public bool podeAumentar = true;
    // Start is called before the first frame update
    void Start()
    {
        listaDeObjetos = new List<GameObject>();

        for (int i = 0; i < comecaInstanciado; i++)
        {
            listaDeObjetos.Add(Instantiate(objetoParaInstanciar));
            listaDeObjetos[i].SetActive(false);
        }
    }

    public GameObject PegaObjeto()
    {
        for (int i = 0; i < listaDeObjetos.Count; i++)
        {
            if (!listaDeObjetos[i].activeInHierarchy)
            {
                return listaDeObjetos[i];
            }
        }

        if (!podeAumentar)
        {
            return null;
        }

        listaDeObjetos.Add(Instantiate(objetoParaInstanciar));
        return listaDeObjetos[listaDeObjetos.Count - 1];
    }
}
