using UnityEngine;

public class MachineParticles : MonoBehaviour
{
    public ParticleSystem particles;
    private int npcInside = 0;

    private void Start()
    {
        particles.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("NPC"))
        {
            npcInside++;

            if (!particles.isPlaying)
                particles.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("NPC"))
        {
            npcInside--;

            if (npcInside <= 0)
            {
                npcInside = 0;
                particles.Stop();
            }
        }
    }
}
