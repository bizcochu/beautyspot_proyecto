using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Dialogue : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panel;
    public TextMeshProUGUI texto;

    [Header("Service Buttons")]
    public Button btnPelo;  // Assign in Inspector (e.g., "Corte/Pelo")
    public Button btnManos; // Assign in Inspector (e.g., "Manicura")
    public Button btnPies;  // Assign in Inspector (e.g., "Pedicura")

    public void ShowServiceSelection(string mensaje, Action onPelo, Action onManos, Action onPies)
    {
        panel.SetActive(true);
        texto.text = mensaje;

        // Reset previous listeners
        btnPelo.onClick.RemoveAllListeners();
        btnManos.onClick.RemoveAllListeners();
        btnPies.onClick.RemoveAllListeners();

        // Assign new logic to buttons
        btnPelo.onClick.AddListener(() =>
        {
            ClosePanel();
            onPelo?.Invoke();
        });

        btnManos.onClick.AddListener(() =>
        {
            ClosePanel();
            onManos?.Invoke();
        });

        btnPies.onClick.AddListener(() =>
        {
            ClosePanel();
            onPies?.Invoke();
        });
    }

    private void ClosePanel()
    {
        panel.SetActive(false);
    }
}