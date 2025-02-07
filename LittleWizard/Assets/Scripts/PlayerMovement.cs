using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator playeranimator;
    [SerializeField] private float speed = 2f;
    private Rigidbody2D rbplayer;
    private Vector2 moveImput;
    private Vector2 lastMoveDirection = Vector2.down;

    // Start is called before the first frame update
    void Start()
    {
        rbplayer = GetComponent<Rigidbody2D>();
        playeranimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveImput = new Vector2 (moveX, moveY).normalized;

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

    private void FixedUpdate()
    {
        rbplayer.MovePosition(rbplayer.position + moveImput * speed * Time.fixedDeltaTime);
    }
}
