using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class PatternFMOD : MonoBehaviour
{
    // ��������� ��� ������� � ���
    [Header("��������� eventFMOD")]
    [SerializeField] private EventReference eventAudioFon;
    [Header("��������� ���������")]
    [SerializeField][Range(0, 20)] private float eventAudioFonVolume;
    [Header("��������� ���.����������")]
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
