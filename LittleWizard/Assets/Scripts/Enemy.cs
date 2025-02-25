using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Enemy : MonoBehaviour
{
    private RoomGenerator roomGenerator;
    private Transform[] nearbyPoints;
    private GameManager gameManager;
    private float speed = 1.5f;
    private int lives = 3;
    private Animator animator;
    private Vector2 moveDirection = Vector2.down;
    private Transform player;
    private bool hasAttacked = false; // Variable de control para el ataque
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        roomGenerator = FindObjectOfType<RoomGenerator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = FindObjectOfType<GameManager>();

        // Para encontrar los NearbyPoints
        GameObject[] points = GameObject.FindGameObjectsWithTag("NearbyPoint"); // Busca todos los puntos esta etiqueta
        nearbyPoints = new Transform[points.Length];    // Crea un array de Transforms

        for (int i = 0; i < points.Length; i++) // Recorre todos los puntos
        {
            nearbyPoints[i] = points[i].transform; // Guarda solo el Transform
        }
    }

    void Update()
    {
        if (player == null) // Si el jugador no existe, no hacer nada
        {
            return;
        }

        Follow();

        if (lives <= 0)
        {
            roomGenerator.EnemigoDerrotado();   // Llamar al método EnemigoDerrotado
            Destroy(gameObject);
        }
    }
    
    //---------------------------------- PERDER VIDA ----------------------------------------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MagicBall")
        {
            lives--;
        }
    }

    //---------------------------------- MOVIMIENTO ----------------------------------------------
    private void Follow()
    {
        Transform nearestPoint = NearbyPoint(nearbyPoints); // Encuentra el punto más cercano

        if (Vector2.Distance(transform.position, nearestPoint.position) > 0f)   //Si la distancia es mayor a 0, moverse
        {
            transform.position = Vector2.MoveTowards(transform.position, nearestPoint.position, speed * Time.deltaTime);

            moveDirection = nearestPoint.position - transform.position;
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetBool("IsMoving", true);

            StopAttack();
        }
        else    // Si la distancia es 0, atacar
        {
            animator.SetBool("IsMoving", false);
            Attack();
        }
    }

    Transform NearbyPoint(Transform[] nearbyPoints)     // Método para encontrar el punto más cercano
    {
        Transform nearestPoint = null;  // Punto más cercano
        float minDistanceSqr = Mathf.Infinity;  // Distancia mínima
        
        foreach (Transform point in nearbyPoints)   // Recorre todos los puntos
        {
            float distanceSqr = (point.position - transform.position).sqrMagnitude; // Usamos sqrMagnitude para optimización
            if (distanceSqr < minDistanceSqr)   // Si la distancia es menor a la mínima
            {
                minDistanceSqr = distanceSqr;   // Actualiza la distancia mínima
                nearestPoint = point;   // Guarda el punto más cercano
            }
        }
        return nearestPoint;
    }
    
    //---------------------------------- ATAQUE ----------------------------------------------
    private void Attack()   // Método para atacar
    {
        if (!hasAttacked) // Verifica si ya ha atacado
        {
            AttackDirection();
            animator.SetBool("Attack", true);
            StartCoroutine(ApplyDamageOverTime());
            hasAttacked = true; // Marca que ya ha atacado
        }
    }

    private void StopAttack()   // Método para detener el ataque
    {
        isAttacking = false;
        animator.SetBool("Attack", false);
        hasAttacked = false;
        StopCoroutine(ApplyDamageOverTime());
    }

    private IEnumerator ApplyDamageOverTime()   // Método para aplicar daño por tiempo
    {
        isAttacking = true;
        while (isAttacking)
        {
            gameManager.ReducirVida();
            yield return new WaitForSeconds(1f);
        }
    }

    private void AttackDirection()
    {
        Vector2 attackDirection = player.position - transform.position;
        animator.SetFloat("Horizontal", attackDirection.x);
        animator.SetFloat("Vertical", attackDirection.y);
    }
}