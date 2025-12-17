using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioSource musicSource;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Seguridad por si no lo arrastraste
        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();

        // Cargar volumen guardado
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        // Asegurarse de que suene
        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    // 🔊 Llamado desde slider o código
    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
}
