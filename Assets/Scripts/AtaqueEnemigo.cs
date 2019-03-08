﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueEnemigo : MonoBehaviour
{

    public LayerMask layerMask;

    GameObject golpeado;
    public float danoEnemigo;
    Animator animZorro;
    bool enCombate;
    SpriteRenderer sprZorro;

    // Start is called before the first frame update
    void Start()
    {
        animZorro = GetComponent<Animator>();
        sprZorro = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction;
        if (sprZorro.flipX)
        {
            direction = Vector2.right;
        }
        else
            direction = Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1, layerMask);





        if (hit)
        {
            Debug.DrawRay(transform.position, direction * 1, Color.yellow);
            Debug.Log("Did Hit " + hit.transform.name);

            if (golpeado == null)
            {
                StartCoroutine(Atacando());
                golpeado = hit.collider.gameObject;
                
            }
            enCombate = true;
            GetComponent<movimientoZorro>().velocidad = 0;
            animZorro.SetBool("EnCombate", enCombate);

        }
        else
        {
            Debug.DrawRay(transform.position, direction * 1, Color.white);
            Debug.Log("Did not Hit");
            golpeado = null;
            enCombate = false;
            GetComponent<movimientoZorro>().velocidad = 1;
            animZorro.SetBool("EnCombate", enCombate);
            StopAllCoroutines();

        }
    }


    IEnumerator Atacando()
    {
        while (true)
        {
            if (golpeado != null)
            {
                animZorro.SetTrigger("Ataque");
                golpeado.GetComponent<EstadisticasPJ>().QuitaVida(danoEnemigo);
            }
            yield return new WaitForSeconds(2f);
        }

    }
}
