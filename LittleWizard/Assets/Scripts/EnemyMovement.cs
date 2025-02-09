using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private RoomGenerator roomGenerator;
    private float speed = 1.5f;
    private float minDistance = 0.6f;
    private int lives = 3;

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
            roomGenerator.EnemigoDerrotado();   // Llamar al método EnemigoDerrotado
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
        
        // minDistance = player.position.x - 0.5f;

        if (Vector2.Distance(transform.position, player.position) > minDistance)
        {            
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            moveDirection = player.position - transform.position;
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
            Attack();
        }
    }

    private void Attack(){
        //atacar
    }
}
