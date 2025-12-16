using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public GameObject menuPrincipal;   // Arrastra tu GameObject "Menu" aquí
    public GameObject menuOpciones;    // Arrastra tu GameObject "MenuOpciones" aquí

    // Botón Jugar
    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Botón Salir
    public void Salir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }

    // Botón Opciones
    public void MostrarOpciones()
    {
        menuPrincipal.SetActive(false); // Oculta el menú principal
        menuOpciones.SetActive(true);   // Muestra el menú de opciones
    }

    // Botón Volver desde opciones
    public void VolverMenuPrincipal()
    {
        menuOpciones.SetActive(false);  // Oculta el menú de opciones
        menuPrincipal.SetActive(true);  // Muestra el menú principal
    }
}
