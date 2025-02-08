using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public Sprite spritePuertasCerradas; // Sprite inicial de la habitación (puertas cerradas)
    public Sprite spritePuertasAbiertas; // Sprite de la habitación cuando se eliminan todos los enemigos

    private SpriteRenderer spriteRenderer;
    private int enemigosRestantes; // Número de enemigos actuales en la habitación

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ResetearHabitacion(); // Se inicia con el sprite de puertas cerradas y enemigos nuevos
    }

    // Método que se llama cuando se genera una nueva habitación
    public void ResetearHabitacion()
    {
        enemigosRestantes = 1; // Número aleatorio de enemigos en la habitación ¡¡¡¡¡¡¡¡CAMBIAR!!!!!!
        spriteRenderer.sprite = spritePuertasCerradas; // Restaurar sprite inicial
    }

    // Método llamado cuando un enemigo es derrotado
    public void EnemigoDerrotado()
    {
        enemigosRestantes--;

        if (enemigosRestantes <= 0)
        {
            AbrirPuertas();
        }
    }

    // Cambia el sprite para mostrar puertas abiertas
    private void AbrirPuertas()
    {
        spriteRenderer.sprite = spritePuertasAbiertas;
    }
}
