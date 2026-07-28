using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ControlVolume : MonoBehaviour
{
    //Verbindung zum Volume-Slider
    public Slider volumeSlider;
    public Toggle muteToggle;

    private float volume;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Mögliche gespeicherte Preferenzen
        volume = PlayerPrefs.GetFloat("Volume", 1f);

        //Slider wird an ursprüngliche Lautstärke angepasst
        volumeSlider.value = volume;

        volumeSlider.onValueChanged.AddListener(SetVolume);
        muteToggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(muteToggle);
        });
    }

    private void ToggleValueChanged(Toggle change)
    {
        if (muteToggle.isOn)
        {
            volume = 0f;
        }
        else
        {
            volume = volumeSlider.value;
        }

        SetVolume(volume);
    }

    private void SetVolume(float volume)
    {
        //Stellt Lautstärke basierend auf Slider ein
        AudioListener.volume = volume;

        print(AudioListener.volume);

        //Speichert eingestellte Lautstärke für die anderen Szenen
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }
}
