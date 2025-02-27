using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private RoomGenerator roomGenerator;
    private static List<Transform> occupiedPoints = new List<Transform>();
    private Transform[] nearbyPoints;
    private GameManager gameManager;
    private float speed;
    private int lives = 3;
    private Animator animator;
    private Vector2 moveDirection = Vector2.down;
    private Transform player;
    private bool hasAttacked = false; 
    private bool isAttacking = false;
    private Transform currentTarget = null; // Guarda el punto actual donde el enemigo está atacando
    private bool colliderBug;
    private bool isRepositioning = false; // Para evitar cambios bruscos de target

    void Start()    // Se genera una vez por enemigo
    {
        animator = GetComponent<Animator>();
        roomGenerator = FindObjectOfType<RoomGenerator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = FindObjectOfType<GameManager>();

        speed = Random.Range(1.2f, 1.8f);

        // Obtener los NearbyPoints
        GameObject[] points = GameObject.FindGameObjectsWithTag("NearbyPoint");
        nearbyPoints = new Transform[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            nearbyPoints[i] = points[i].transform;
        }
    }

    void Update()
    {
        if (player == null) return;     // Cuando el player muera

        Follow();

        if (lives <= 0)     // Cuando el enemigo muera
        {
            ReleasePoint();     // Libera el punto al morir
            roomGenerator.EnemigoDerrotado();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)     // Daño de la magic ball
    {
        if (other.gameObject.tag == "MagicBall")
        {
            lives--;
        }
    }

    //---------------------------------- MOVIMIENTO ----------------------------------------------
    private void Follow()
    {
        // Verificar si el enemigo ya tiene un objetivo y si el jugador está cerca
        if (currentTarget == null || Vector2.Distance(transform.position, player.position) > 1.5f) 
        {
            // Buscar un nuevo punto cercano
            Transform nearestPoint = NearbyPoint(nearbyPoints);

            if (nearestPoint != null)
            {
                ReleasePoint(); // Liberar el punto anterior antes de asignar uno nuevo
                currentTarget = nearestPoint;
                occupiedPoints.Add(nearestPoint);
            }
        }

        // Si no hay un punto válido, salir
        if (currentTarget == null) return;

        if (Vector2.Distance(transform.position, currentTarget.position) > 0.1f)
        {
            // Moverse hacia el punto
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

            moveDirection = currentTarget.position - transform.position;
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetBool("IsMoving", true);

            StopAttack();
        }
        else
        {
            // Atacar solo si ha llegado al punto
            animator.SetBool("IsMoving", false);
            Attack();
        }
    }

    Transform NearbyPoint(Transform[] points)
    {
        Transform nearestPoint = null;
        float minDistanceSqr = Mathf.Infinity;

        foreach (Transform point in points)
        {
            if (occupiedPoints.Contains(point)) continue; // Saltar los puntos ocupados

            float distanceSqr = (point.position - transform.position).sqrMagnitude;
            if (distanceSqr < minDistanceSqr)
            {
                minDistanceSqr = distanceSqr;
                nearestPoint = point;
            }
        }
        return nearestPoint;
    }

    //---------------------------------- REPOSICIÓN ----------------------------------------------

    void OnCollisionEnter2D(Collision2D collision)  // Solucion para daño repetitivo
    {
        if (collision.gameObject.CompareTag("Enemy") && !isRepositioning)
        {
            colliderBug = true;
            StartCoroutine(RepositionAfterCollision());
        }
    }
    void OnCollisionExit2D(Collision2D collision)   // Solucion para daño repetitivo
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            colliderBug = false;
        }
    }

    private IEnumerator RepositionAfterCollision()
    {
        isRepositioning = true;
        ReleasePoint();
        yield return new WaitForSeconds(0.5f);
        currentTarget = NearbyPoint(nearbyPoints);

        if (currentTarget != null)
        {
            occupiedPoints.Add(currentTarget);
        }
        isRepositioning = false;
    }

    private void ReleasePoint()
    {
        if (currentTarget != null)
        {
            occupiedPoints.Remove(currentTarget);
            currentTarget = null;
        }
    }

    //---------------------------------- ATAQUE ----------------------------------------------

    private void Attack()
    {
        if (!hasAttacked && !colliderBug)
        {
            AttackDirection();
            animator.SetBool("Attack", true);
            StartCoroutine(ApplyDamageOverTime());
            hasAttacked = true;
        }
    }

    private void StopAttack()
    {
        isAttacking = false;
        animator.SetBool("Attack", false);
        hasAttacked = false;
        StopCoroutine(ApplyDamageOverTime());
        ReleasePoint();
    }

    private IEnumerator ApplyDamageOverTime()
    {
        isAttacking = true;
        while (isAttacking)
        {
            gameManager.ReducirVida();
            yield return new WaitForSeconds(2f);
        }
    }

    private void AttackDirection()
    {
        Vector2 attackDirection = player.position - transform.position;
        animator.SetFloat("Horizontal", attackDirection.x);
        animator.SetFloat("Vertical", attackDirection.y);
    }
}
