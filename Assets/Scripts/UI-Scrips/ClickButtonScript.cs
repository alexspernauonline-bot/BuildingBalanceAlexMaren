using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ClickButtonScript : MonoBehaviour
{
    //Verbindung zum Audio
    private AudioSource audioSource;
    public AudioClip clickSound;

    //Menü-Buttons Referenz
    public Button[] btns = new Button[7];


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //Fügt an jeden Button einen Listener fürs Klicken hinzu
        foreach (var button in btns)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }

    void PlayClickSound()
    {
        //Abspielen des Sounds
        audioSource.PlayOneShot(clickSound, 1f);
    }
}
