using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float minDistance;
    private int lives = 2;

    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Follow();

        if (lives <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "MagicBall")
            {
                lives--;
            }
        }
    

    private void Follow()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        if (Vector2.Distance(transform.position, player.position) > minDistance)
        {
            moveDirection = direction;
            rb.velocity = moveDirection * speed;  // Usamos Rigidbody para mover al enemigo

            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetBool("IsMoving", true);
        }
        else
        {
            rb.velocity = Vector2.zero;  // Detener movimiento
            animator.SetBool("IsMoving", false);
            Attack();
        }
    }

    private void Attack(){
        //atacar
    }
}
