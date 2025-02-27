using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int playerLives = 6;
    public string Game;
    public RoomGenerator roomGenerator;
    public TextMeshProUGUI nRoomText;   // Texto N Room
    public TextMeshProUGUI nEnemyText;   // Texto N Enemies
    public GameObject player;
    public GameObject gameOver;
    public MusicManager musicManager;
    [SerializeField] public GameObject[] hearts;

    void Start()
    {
        roomGenerator = FindObjectOfType<RoomGenerator>();  
    }

    void Update()
    {
        if (playerLives <= 0)
        {
            Destroy(player);
            Time.timeScale = 0;
            GameOver();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }
        EnemyNumerator();
    }

    public void EnemyNumerator()
    {
        nRoomText.text = string.Format("Room: " + "{0:0}", roomGenerator.currentRoom);
        nEnemyText.text = string.Format("Enemies: " + "{0:000}", roomGenerator.enemigosDerrotados);
    }

    public void ReducirVida()
    {
        playerLives--;
        Debug.Log("Vidas: " + playerLives);

        RestarVidaUI(); // Restar vida en la UI
    }

    private void RestarVidaUI()
    {
        hearts[playerLives].SetActive(false);
    }

    private void GameOver()
    {
        gameOver.SetActive(true);

        if (musicManager != null)
        {
            musicManager.StopMusic();
        }
    }

    private void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(Game);

        if (musicManager != null)
        {
            musicManager.RestartMusic();
        }
    }
}
