using UnityEngine;
using UnityEngine.SceneManagement;

public class MenySystem : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir()

    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }
   
}
