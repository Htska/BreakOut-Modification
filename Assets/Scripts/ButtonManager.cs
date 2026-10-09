using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("Juego");
        AudioManager.instance.PlayMusic();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
        
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
