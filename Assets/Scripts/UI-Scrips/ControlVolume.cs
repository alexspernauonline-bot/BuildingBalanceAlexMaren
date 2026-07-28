using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ControlVolume : MonoBehaviour
{
    //Verbindung zum Volume-Slider
    public Slider volumeSlider;

    //Variable für die Lautstärke
    public float setVolume;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Mögliche gespeicherte Preferenzen
        float volume = PlayerPrefs.GetFloat("Volume", 1f);

        //Slider wird an ursprüngliche Lautstärke angepasst
        volumeSlider.value = volume;

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float volume)
    {
        //Stellt Lautstärke basierend auf Slider ein
        AudioListener.volume = volume;

        //Speichert eingestellte Lautstärke für die anderen Szenen
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }
}
