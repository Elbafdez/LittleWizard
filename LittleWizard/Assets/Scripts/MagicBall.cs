using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1.5f); // Destruye la bala después de 2 segundos
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject); // Se destruye al chocar con un objeto sólido
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject); // Se destruye si entra en contacto con un trigger
    }
}
