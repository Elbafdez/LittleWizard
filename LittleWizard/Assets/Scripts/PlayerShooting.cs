using UnityEngine;
using System.Collections;  // Necesario para usar corrutinas

public class PlayerShooting : MonoBehaviour
{
    public GameObject magicBall;
    public Transform scepter;
    private float magicBallSpeed = 2f;
    private float fireRate = 0.8f;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Disparar();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Disparar()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);    // Convertimos la posición del mouse a coordenadas del mundo
        mousePosition.z = 0f;

        Vector3 direccion = (mousePosition - scepter.position).normalized;  // Obtenemos la dirección hacia la que apuntamos

        GameObject bala = Instantiate(magicBall, scepter.position, Quaternion.identity);    // Creamos la bala
        bala.GetComponent<Rigidbody2D>().velocity = direccion * magicBallSpeed;  // Aplicamos velocidad a la bala
    }
}