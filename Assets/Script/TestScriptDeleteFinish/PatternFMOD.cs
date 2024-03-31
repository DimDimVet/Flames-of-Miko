using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class PatternFMOD : MonoBehaviour
{
    // Заготовка для вставки в код
    [Header("Установка eventFMOD")]
    [SerializeField] private EventReference eventAudioFon;
    [Header("Установка громкости")]
    [SerializeField][Range(0, 20)] private float eventAudioFonVolume;
    [Header("Установка доп.параметров")]
    [SerializeField] private string nameParametr;
    [SerializeField] private float floatValue;
    private EventInstance audioFon;
    void Start()
    {
        if (!eventAudioFon.IsNull)
        {
            audioFon = RuntimeManager.CreateInstance(eventAudioFon);
            audioFon.start();
        }

    }
    void Update()
    {
        audioFon.setVolume(eventAudioFonVolume);
    }
}
