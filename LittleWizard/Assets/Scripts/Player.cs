using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private RoomGenerator roomGenerator;
    private GameManager gameManager;
    private Animator playeranimator;
    private float speed = 2f;
    private Rigidbody2D rbplayer;
    private Vector2 moveImput;
    private Vector2 lastMoveDirection = Vector2.down;

    void Start()
    {
        rbplayer = GetComponent<Rigidbody2D>();
        playeranimator = GetComponent<Animator>();
        roomGenerator = FindObjectOfType<RoomGenerator>();
    }

    void Update()
    {
        PlayerMovement();
    }

    private void FixedUpdate()
    {
        rbplayer.MovePosition(rbplayer.position + moveImput * speed * Time.fixedDeltaTime); // Movimiento del personaje
    }

    private void PlayerMovement()
    {
        // Movimiento 
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveImput = new Vector2(moveX, moveY).normalized;

        // Si el personaje se está moviendo, actualizamos la última dirección
        if (moveImput != Vector2.zero)
        {
            lastMoveDirection = moveImput;
        }

        // Pasar valores al Animator
        playeranimator.SetFloat("Horizontal", moveX);
        playeranimator.SetFloat("Vertical", moveY);
        playeranimator.SetFloat("Speed", moveImput.sqrMagnitude);

        // Guardar la última dirección en la que se movió para el idle
        playeranimator.SetFloat("LastMoveX", lastMoveDirection.x);
        playeranimator.SetFloat("LastMoveY", lastMoveDirection.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            Debug.Log("Cambio de habitación");
            transform.position *= -1;
            roomGenerator.NewRoom();
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameManager.ReducirVida();
        }
    }
}
