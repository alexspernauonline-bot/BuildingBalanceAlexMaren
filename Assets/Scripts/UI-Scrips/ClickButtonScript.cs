using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickButtonScript : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clickSound;
    public Button returnBtn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        returnBtn.onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound, 1f);
    }
}
