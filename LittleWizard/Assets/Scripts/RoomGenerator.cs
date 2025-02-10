using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    //-------------------------------- HABITACION ------------------------------------------------
    public Sprite spritePuertasCerradas; // Sprite inicial de la habitación (puertas cerradas)
    public Sprite spritePuertasAbiertas; // Sprite de la habitación cuando se eliminan todos los enemigos
    private SpriteRenderer spriteRenderer;
    private int enemigosRestantes; // Número de enemigos actuales en la habitación

    //-------------------------------- ENEMIGOS ------------------------------------------------
    [SerializeField] private GameObject enemyPrefab; // Prefab del enemigo
    private int startMinEnemies = 1; // Min enemigos en la 1ra habitación
    private int startMaxEnemies = 2; // Max enemigos en la 1ra habitación
    private int finalMinEnemies = 6; // Min enemigos en habitaciones avanzadas
    private int finalMaxEnemies = 8; // Max enemigos en habitaciones avanzadas
    private int roomsUntilMax = 10; // Número de habitaciones hasta alcanzar el límite
    private int currentRoom = 1; // Número de habitación actual
    private float spawnRadius = 1.5f; // Distancia mínima entre enemigos
    [SerializeField] private float xMin, xMax, yMin, yMax; // Límites de la habitación

    private List<Vector2> spawnPositions = new List<Vector2>(); // Puntos de spawn de enemigos

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ResetearHabitacion(); // Se inicia con el sprite de puertas cerradas y enemigos nuevos

        int enemyCount = GetEnemyCount();   // Obtener el número de enemigos a spawnear
        SpawnEnemies(enemyCount);          // Spawnear enemigos
    }

    //-------------------------------- ENEMIGOS ------------------------------------------------

    private int GetEnemyCount()     // Método que devuelve un número aleatorio de enemigos a spawnear
    {
        // Calcular progresivamente el mínimo y máximo de enemigos hasta llegar al límite
        int dynamicMin = (int)Mathf.Lerp(startMinEnemies, finalMinEnemies, (float)currentRoom / roomsUntilMax);
        int dynamicMax = (int)Mathf.Lerp(startMaxEnemies, finalMaxEnemies, (float)currentRoom / roomsUntilMax);

        // Convertir a valores enteros y asegurarse de que el mínimo no supere el máximo
        int minEnemies = dynamicMin;
        int maxEnemies = dynamicMax;

        return Random.Range(minEnemies, maxEnemies + 1); 
    }

    private void SpawnEnemies(int enemyCount)       // Método que spawneará enemigos en la habitación
    {
        int spawnedEnemies = 0;
        int maxAttempts = 100; // Para evitar bucles infinitos
        int attempts = 0;

        while (spawnedEnemies < enemyCount && attempts < maxAttempts)
        {
            attempts++;

            // Generar una posición aleatoria dentro de los límites
            Vector2 randomPosition = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));

            // Verificar si la posición es válida (sin solapamiento)
            if (IsValidSpawn(randomPosition))
            {
                spawnPositions.Add(randomPosition); // Guardar la posición
                Instantiate(enemyPrefab, randomPosition, Quaternion.identity); // Spawnear enemigo
                spawnedEnemies++;
            }
        }
    }

    private bool IsValidSpawn(Vector2 position)     // Método que verifica si una posición es válida para spawnear un enemigo
    {
        foreach (Vector2 existingPos in spawnPositions)
        {
            if (Vector2.Distance(position, existingPos) < spawnRadius)
            {
                return false; // Demasiado cerca de otro enemigo
            }
        }
        return true;
    }
    
    //-------------------------------- HABITACION ------------------------------------------------

    public void ResetearHabitacion()    // Método que resetea la habitación
    {   
        int enemyCount = GetEnemyCount(); // Obtener el número de enemigos a spawnear
        enemigosRestantes = enemyCount; // Asignar el número de enemigos restantes
        spriteRenderer.sprite = spritePuertasCerradas; // Restaurar sprite inicial
    }

    public void EnemigoDerrotado()      // Método que se llama cuando un enemigo es derrotado
    {
        enemigosRestantes--;

        if (enemigosRestantes <= 0)
        {
            AbrirPuertas();
        }
    }

    private void AbrirPuertas()     // Método que cambia el sprite de la habitación a puertas abiertas
    {
        spriteRenderer.sprite = spritePuertasAbiertas;
    }
}
