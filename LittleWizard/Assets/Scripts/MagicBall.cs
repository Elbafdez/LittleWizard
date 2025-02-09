using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2f); // Destruye la bala después de 2 segundos
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MagicBall>() == null) // Verifica que no sea otra MagicBall
        {
            Destroy(gameObject); // Se destruye al chocar con un objeto sólido
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<MagicBall>() == null) // Verifica que no sea otra MagicBall
        {
            Destroy(gameObject); // Se destruye si entra en contacto con un trigger
        }
    }
}
