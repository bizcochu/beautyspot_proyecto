using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject dialoguePanel;
    public GameObject optionsMenu;
    public GameObject creditsPanel;   // 👈 NUEVO (opcional)

    private bool isPaused = false;
    private bool dialogueWasVisible = false;

    void Start()
    {
        optionsMenu.SetActive(false);
        if (creditsPanel != null)
            creditsPanel.SetActive(false);

        dialoguePanel.SetActive(false);
    }

    // =========================
    // OPCIONES
    // =========================
    public void OpenOptionsMenu()
    {
        Time.timeScale = 0f;
        isPaused = true;

        dialogueWasVisible = dialoguePanel.activeSelf;

        dialoguePanel.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        optionsMenu.SetActive(false);

        if (dialogueWasVisible)
            dialoguePanel.SetActive(true);
    }

    // =========================
    // BOTONES DEL MENÚ OPCIONES
    // =========================

    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // MUY IMPORTANTE
        SceneManager.LoadScene("Menu");
    }

    public void OpenCredits()
    {
        if (creditsPanel == null) return;

        optionsMenu.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        if (creditsPanel == null) return;

        creditsPanel.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }

    // =========================
    // DIÁLOGO
    // =========================
    public void ShowDialogue()
    {
        if (isPaused) return;
        dialoguePanel.SetActive(true);
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueWasVisible = false;
    }
}
