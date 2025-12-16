using UnityEngine;

public class RandomParticleRotation : MonoBehaviour
{
    void OnEnable()
    {
        float randomY = Random.Range(0f, 360f);
        transform.localRotation = Quaternion.Euler(0f, randomY, 0f);
        Debug.Log("Rotación aplicada: " + randomY);
    }
}
