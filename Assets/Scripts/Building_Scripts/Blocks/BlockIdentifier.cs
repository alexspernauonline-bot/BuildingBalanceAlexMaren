using UnityEngine;

public enum BlockWeightCategory
{
    Light,
    Medium,
    Heavy
}

public class BlockIdentifier : MonoBehaviour
{
    
    // Hier kannst du im Inspector die Gewichtsklasse einstellen
    public BlockWeightCategory myCategory;

    // Hier merkt sich der Block sp�ter heimlich seine echte Farbe
    public Material trueMaterial;

    public AudioClip placeSound;
    public AudioClip crashSound;
    public AudioSource blockAudioSource;

    void Awake()
    {

        blockAudioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        blockAudioSource.PlayOneShot(placeSound, 1);
        print("soundblockk");
    }
}
