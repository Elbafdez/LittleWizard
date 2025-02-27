using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Obtener el componente AudioSource
        audioSource = GetComponent<AudioSource>();

        // Reproducir la música
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    // Método para detener la música
    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Método para reiniciar la música desde el inicio
    public void RestartMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop(); // Asegúrate de detener cualquier reproducción actual
            audioSource.Play(); // Reproducir desde el principio
        }
    }
}
