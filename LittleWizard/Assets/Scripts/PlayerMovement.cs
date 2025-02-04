using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator playeranimator;
    private float speed = 2.5f;
    private Rigidbody2D rbplayer;
    private Vector2 moveImput;
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

        playeranimator.SetFloat("Horizontal", moveX);
        playeranimator.SetFloat("Vertical", moveY);
        playeranimator.SetFloat("Speed", moveImput.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        rbplayer.MovePosition(rbplayer.position + moveImput * speed * Time.fixedDeltaTime);
    }
}
