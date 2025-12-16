using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject dialoguePanel;
    public GameObject optionsMenu;

    private bool isPaused = false;

    // =========================
    // OPCIONES
    // =========================
    public void OpenOptionsMenu()
    {
        Time.timeScale = 0f;           // Pausa el juego
        isPaused = true;

        optionsMenu.SetActive(true);
        dialoguePanel.SetActive(false);
    }

    public void CloseOptionsMenu()
    {
        Time.timeScale = 1f;           // Reanuda el juego
        isPaused = false;

        optionsMenu.SetActive(false);
    }

    // =========================
    // DIÁLOGO
    // =========================
    public void ShowDialogue()
    {
        if (isPaused) return;          // No mostrar si está en pausa
        dialoguePanel.SetActive(true);
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
