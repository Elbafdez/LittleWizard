using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    //-------------------------------- HABITACION ------------------------------------------------
    public Sprite spritePuertasCerradas;
    public Sprite spritePuertasAbiertas;
    private SpriteRenderer spriteRenderer;
    private int enemigosRestantes; // Número de enemigos actuales en la habitación
    [SerializeField] private GameObject[] Doors;   // Referencia a las puertas
    //-------------------------------- ENEMIGOS ------------------------------------------------
    private Transform player;  // Referencia al jugador
    [SerializeField] private GameObject enemyPrefab; // Prefab del enemigo
    private int startMinEnemies = 1; // Min enemigos en la 1ra habitación
    private int startMaxEnemies = 1; // Max enemigos en la 1ra habitación
    private int finalMinEnemies = 5; // Min enemigos en habitaciones avanzadas
    private int finalMaxEnemies = 6; // Max enemigos en habitaciones avanzadas
    private int roomsUntilMax = 10; // Número de habitaciones hasta alcanzar el límite
    private int currentRoom = 0; // Número de habitación actual
    private int nEnemies = 0;
    [SerializeField] private float xMin, xMax, yMin, yMax; // Límites de la habitación

    private List<Vector2> spawnPositions = new List<Vector2>(); // Puntos de spawn de enemigos

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        NewRoom(); // Se inicia con el sprite de puertas cerradas y enemigos nuevos
    }

    //-------------------------------- HABITACION ------------------------------------------------

    public void NewRoom()    // Método que resetea la habitación
    {   
        spawnPositions.Clear(); // Limpiar lista de posiciones de spawn
        Debug.Log("N.Habitación: " + currentRoom);
        currentRoom ++; // Incrementar el número de habitación

        nEnemies = GetEnemyCount(); // Obtener el número de enemigos a spawnear
        enemigosRestantes = nEnemies; // Asignar el número de enemigos restantes
        Debug.Log("N.Enemigos: " + nEnemies);

        spriteRenderer.sprite = spritePuertasCerradas; // Restaurar sprite inicial (Puertas cerradas)
        foreach (GameObject door in Doors)  // Ocultar puertas
        {
            door.SetActive(false);
        }

        SpawnEnemies(nEnemies);   // Spawnear enemigos
    }

    public void EnemigoDerrotado()      // Método que se llama cuando un enemigo es derrotado
    {
        enemigosRestantes--;

        if (enemigosRestantes <= 0)
        {
            Debug.Log("Todos los enemigos derrotados, abrir puertas");
            spriteRenderer.sprite = spritePuertasAbiertas;
            foreach (GameObject door in Doors)  // Mostrar puertas
            {
                door.SetActive(true);
            }
        }
    }

    //-------------------------------- ENEMIGOS ------------------------------------------------

    public int GetEnemyCount()     // Método que devuelve un número aleatorio de enemigos a spawnear------------------------------ NO VA --------------------------------
    {
        // Calcular progresivamente el mínimo y máximo de enemigos hasta llegar al límite
        int dynamicMin = (int)Mathf.Lerp(startMinEnemies, finalMinEnemies, (float)currentRoom / roomsUntilMax);
        int dynamicMax = (int)Mathf.Lerp(startMaxEnemies, finalMaxEnemies, (float)currentRoom / roomsUntilMax);

        return Random.Range(dynamicMin, dynamicMax + 1);
    }

    private void SpawnEnemies(int nEnemies)    // Método que spawneará enemigos en la habitación
    {
        int spawnedEnemies = 0;
        int maxAttempts = 100; // Numero max. de busqueda de pt. de spawn (para evitar bucles infinitos)
        int attempts = 0;

        while (spawnedEnemies < nEnemies && attempts < maxAttempts)
        {
            attempts++;

            Vector2 spawnPoint = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));   // Punto de spawn aleatorio

            // Verificar si la posición es válida (sin solapamiento)
            if (IsValidSpawn(spawnPoint))
            {
                spawnPositions.Add(spawnPoint); // Guardar la posición
                Instantiate(enemyPrefab, spawnPoint, Quaternion.identity); // Spawnear enemigo
                spawnedEnemies++;
            }
        }
        Debug.Log("Enemigos spawneados: " + spawnedEnemies);
    }

    private bool IsValidSpawn(Vector2 spawnPoint)     // Método que verifica si una posición no coincide con el mago ni con el resto de posiciones de spawn
    {
        float playerRadius = 1.5f; // Radio de distancia minima al jugador
        float ememySpawnRadius = 1f; // Distancia mínima entre enemigos
        bool isValid = true;
        
        // Verifica la distancia con el jugador
        if (Vector2.Distance(spawnPoint, player.transform.position) < playerRadius)     // NO SE ESTA COMPROBANDO
        {
            isValid = false; // Demasiado cerca del jugador
        }
        
        foreach (Vector2 existingPoint in spawnPositions)
        {
            // Verifica la distancia con otro spawnPoint
            if (Vector2.Distance(spawnPoint, existingPoint) < ememySpawnRadius)
            {
                isValid = false; // Demasiado cerca de otro spawnPoint
            }
        }
        return isValid;
    }
}
