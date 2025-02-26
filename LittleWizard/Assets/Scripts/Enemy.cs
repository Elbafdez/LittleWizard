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

    void Start()
    {
        animator = GetComponent<Animator>();
        roomGenerator = FindObjectOfType<RoomGenerator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = FindObjectOfType<GameManager>();

        speed = Random.Range(1.3f, 1.8f);

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
        if (player == null) return;

        Follow();

        if (lives <= 0)
        {
            roomGenerator.EnemigoDerrotado();
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

    //---------------------------------- MOVIMIENTO ----------------------------------------------
    private void Follow()
    {
        if (currentTarget == null || Vector2.Distance(transform.position, player.position) > 2f) 
        {
            // Si no hay punto asignado o el jugador se aleja, buscar otro punto
            Transform nearestPoint = NearbyPoint(nearbyPoints);

            if (nearestPoint != null)
            {
                currentTarget = nearestPoint; // Guardar el punto actual
                occupiedPoints.Add(nearestPoint); // Marcarlo como ocupado
            }
        }

        if (currentTarget == null) return;

        if (Vector2.Distance(transform.position, currentTarget.position) > 0f)
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

    //---------------------------------- ATAQUE ----------------------------------------------
    private void Attack()
    {
        if (!hasAttacked)
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

        // Cuando el enemigo deja de atacar, liberar el punto ocupado
        if (currentTarget != null)
        {
            occupiedPoints.Remove(currentTarget);
            currentTarget = null; // Restablecer el punto actual
        }
    }

    private IEnumerator ApplyDamageOverTime()
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
