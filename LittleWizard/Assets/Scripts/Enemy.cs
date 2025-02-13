using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private RoomGenerator roomGenerator;
    private Transform[] nearbyPoints;
    private float speed = 1.5f;
    private int lives = 3;

    private Animator animator;
    private Vector2 moveDirection;

    void Start()
    {
        animator = GetComponent<Animator>();
        roomGenerator = FindObjectOfType<RoomGenerator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

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
        Follow();

        if (lives <= 0)
        {
            roomGenerator.EnemigoDerrotado();   // Llamar al método EnemigoDerrotado
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "MagicBall")
            {
                lives--;
            }
        }

    private void Follow()
    {
        Transform nearestPoint = NearbyPoint(nearbyPoints); // Encuentra el punto más cercano
        

        if (Vector2.Distance(transform.position, nearestPoint.position) > 0f){         

            transform.position = Vector2.MoveTowards(transform.position, nearestPoint.position, speed * Time.deltaTime);

            moveDirection = nearestPoint.position - transform.position;
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

    private void Attack(){
        //atacar
    }
}
