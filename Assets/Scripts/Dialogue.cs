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
    // NOMBRES CORREGIDOS SEGÚN TUS BOTONES
    public Button btnLavado;
    public Button btnPermanente;
    public Button btnSecado;

    // --- NUEVO MÉTODO PARA MOSTRAR SÓLO EL MENSAJE (Usado para el despacho automático) ---
    public void ShowMessage(string mensaje)
    {
        panel.SetActive(true);
        texto.text = mensaje;

        // Ocultar los botones cuando la acción es automática
        if (btnLavado != null) btnLavado.gameObject.SetActive(false);
        if (btnPermanente != null) btnPermanente.gameObject.SetActive(false);
        if (btnSecado != null) btnSecado.gameObject.SetActive(false);
    }

    // --- Hide (Ahora es público para llamarlo desde ClientManager) ---
    public void Hide()
    {
        ClosePanel();
    }

    // --- ShowServiceSelection (Mantenido por si quieres volver a la lógica interactiva) ---
    public void ShowServiceSelection(string mensaje, Action onLavado, Action onPermanente, Action onSecado)
    {
        panel.SetActive(true);
        texto.text = mensaje;

        // Mostrar los botones para que el jugador pueda elegir
        if (btnLavado != null) btnLavado.gameObject.SetActive(true);
        if (btnPermanente != null) btnPermanente.gameObject.SetActive(true);
        if (btnSecado != null) btnSecado.gameObject.SetActive(true);

        // Reset previous listeners
        btnLavado.onClick.RemoveAllListeners();
        btnPermanente.onClick.RemoveAllListeners();
        btnSecado.onClick.RemoveAllListeners();

        // Assign new logic to buttons
        btnLavado.onClick.AddListener(() =>
        {
            ClosePanel();
            onLavado?.Invoke();
        });

        btnPermanente.onClick.AddListener(() =>
        {
            ClosePanel();
            onPermanente?.Invoke();
        });

        btnSecado.onClick.AddListener(() =>
        {
            ClosePanel();
            onSecado?.Invoke();
        });
    }

    private void ClosePanel()
    {
        panel.SetActive(false);

        // Opcional: Podrías volver a activar los botones aquí al cerrar el panel
        // si ShowMessage los ocultó, pero es más limpio que ShowServiceSelection los active explícitamente.
    }
}