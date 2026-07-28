using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ClickButtonScript : MonoBehaviour
{
    //Verbindung zum Audio
    private AudioSource audioSource;
    public AudioClip clickSound;

    //Menü-Button Refs
    /*public Button returnBtnHTP;
    public Button returnBtnSettings;
    public Button returnBtnEndTitle;
    public Button closeBtn;
    public Button startBtn;
    public Button settingsBtn;
    public Button guideBtn;*/

    public Button[] btns = new Button[7];


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //Wird beim Klicken ausgeführt
        /*returnBtnHTP.onClick.AddListener(PlayClickSound);
        returnBtnSettings.onClick.AddListener(PlayClickSound);
        returnBtnEndTitle.onClick.AddListener(PlayClickSound);
        returnBtnEndTitle.onClick.AddListener(PlayClickSound);*/

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
