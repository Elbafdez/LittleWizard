using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int lives = 60;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gameOver;
    [SerializeField] public GameObject[] hearts;

    void Update()
    {
        if (lives <= 0)
        {
            Destroy(player);
            Time.timeScale = 0;
            gameOver.SetActive(true);
        }
    }

    public void ReducirVida()
    {
        lives--;
        Debug.Log("Vidas: " + lives);

        if (lives <= 0)
        {
            gameOver.SetActive(true);
        }

        RestarVidaUI();
    }

    private void RestarVidaUI()
    {
        hearts[lives].SetActive(false);
    }
}
