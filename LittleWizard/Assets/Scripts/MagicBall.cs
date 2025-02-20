using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    public float growthRate = 0.5f; // Tasa de crecimiento por segundo
    public float maxSize = 1f; // Tamaño máximo de la bola
    public float rotationSpeed = 100f; // Velocidad de rotación en grados por segundo

    void Start()
    {
        transform.localScale = new Vector3 (0.6f, 0.6f, 0.6f); // Inicializa la bola con tamaño cero
        Destroy(gameObject, 2f); // Destruye la bala después de 2 segundos
    }

    void Update()
    {
        if (transform.localScale.x < maxSize)
        {
            // Aumenta el tamaño de la bola gradualmente
            transform.localScale += Vector3.one * growthRate * Time.deltaTime;
        }
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime); // Rota la bola
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