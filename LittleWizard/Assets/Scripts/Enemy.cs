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

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"));

        animator = GetComponent<Animator>();
        roomGenerator = FindObjectOfType<RoomGenerator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = FindObjectOfType<GameManager>();

        // Para encontrar los NearbyPoints
        GameObject[] points = GameObject.FindGameObjectsWithTag("NearbyPoint"); // Busca todos los puntos esta etiqueta
        nearbyPoints = new Transform[points.Length];    // Crea un array de Transforms

        for (int i = 0; i < points.Length; i++)
        {
            nearbyPoints[i] = points[i].transform; // Guarda solo el Transform
        }
    }

    void Update()
    {
        if (player == null)
        {
            // Si el jugador no existe, no hacer nada
            return;
        }

        Follow();

        if (lives <= 0)
        {
            roomGenerator.EnemigoDerrotado();   // Llamar al método EnemigoDerrotado
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MagicBall")
        {
            lives--;
        }
    }

    private void Follow()
    {
        Transform nearestPoint = NearbyPoint(nearbyPoints); // Encuentra el punto más cercano

        if (Vector2.Distance(transform.position, nearestPoint.position) > 0f)
        {
            transform.position = Vector2.MoveTowards(transform.position, nearestPoint.position, speed * Time.deltaTime);

            moveDirection = nearestPoint.position - transform.position;
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetBool("IsMoving", true);

            StopAttack();
        }
        else
        {
            animator.SetBool("IsMoving", false);

            Attack();
        }
    }

    Transform NearbyPoint(Transform[] nearbyPoints)     // Método para encontrar el punto más cercano
    {
        Transform nearestPoint = null;
        float minDistanceSqr = Mathf.Infinity;
        
        foreach (Transform point in nearbyPoints)
        {
            float distanceSqr = (point.position - transform.position).sqrMagnitude; // Usamos sqrMagnitude para optimización
            if (distanceSqr < minDistanceSqr)
            {
                minDistanceSqr = distanceSqr;
                nearestPoint = point;
            }
        }
        return nearestPoint;
    }
    
    private void Attack()
    {
        if (!hasAttacked) // Verifica si ya ha atacado
        {
            AttackDirection();
            animator.SetBool("Atack", true);
            gameManager.ReducirVida();
            hasAttacked = true; // Marca que ya ha atacado
        }
    }

    private void StopAttack()
    {
        animator.SetBool("Atack", false);
        hasAttacked = false; // Resetea la variable de control cuando deja de atacar
    }

    private void AttackDirection()
    {
        Vector2 attackDirection = player.position - transform.position;
        animator.SetFloat("Horizontal", attackDirection.x);
        animator.SetFloat("Vertical", attackDirection.y);
    }
}