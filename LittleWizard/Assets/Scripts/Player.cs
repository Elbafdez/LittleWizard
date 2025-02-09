using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator playeranimator;
    private float speed = 2f;
    private int lives = 5;
    private Rigidbody2D rbplayer;
    private Vector2 moveImput;
    private Vector2 lastMoveDirection = Vector2.down;
    private Coroutine damageCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        rbplayer = GetComponent<Rigidbody2D>();
        playeranimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        if (lives <= 0)
        {
            Destroy(gameObject);
            //Game Over
        }
    }

    private void FixedUpdate()
    {
        rbplayer.MovePosition(rbplayer.position + moveImput * speed * Time.fixedDeltaTime);
    }

    private void PlayerMovement()
    {
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

    void OnCollisionEnter2D(Collision2D collision)  //Si lo llegara a tocar (no cuando ataca)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamageOverTime());
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DamageOverTime()
    {
        while (true)
        {
            lives--;
            Debug.Log("Vidas: " + lives);
            yield return new WaitForSeconds(1f); // Ajusta el tiempo según sea necesario
        }
    }
}
