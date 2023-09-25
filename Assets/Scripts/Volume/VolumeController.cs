using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController:MonoBehaviour
{
    [SerializeField] List<Slider> volumeSliders;
    [SerializeField] List<TextMeshProUGUI> volumeTexts;
    [SerializeField] GameData gameData;
    bool volumeSlidersSetted;

    private void OnEnable()
    {
        gameData.SetAudioMixer();        
    }

    private void Start()
    {
        SetInitialVolumesSliders();
    }

    private void OnDisable()
    {
        gameData.DisposeMixer();
    }
    
    public void SetAllVolumesUIs()
    {
        if (!volumeSlidersSetted) return;
        gameData.masterVolume.Value = volumeSliders[0].value;
        volumeTexts[0].text = volumeSliders[0].value.ToString("f0");
        gameData.soundEffectsVolume.Value = volumeSliders[1].value;
        volumeTexts[1].text = volumeSliders[1].value.ToString("f0");
        gameData.interfaceVolume.Value = volumeSliders[2].value;
        volumeTexts[2].text = volumeSliders[2].value.ToString("f0");
        gameData.musicVolume.Value = volumeSliders[3].value;
        volumeTexts[3].text = volumeSliders[3].value.ToString("f0");
    }
    void SetInitialVolumesSliders()
    {
        volumeSliders[0].value = gameData.masterVolume.Value;
        volumeTexts[0].text = volumeSliders[0].value.ToString("f0");
        volumeSliders[1].value = gameData.soundEffectsVolume.Value;
        volumeTexts[1].text = volumeSliders[1].value.ToString("f0");
        volumeSliders[2].value = gameData.interfaceVolume.Value;
        volumeTexts[2].text = volumeSliders[2].value.ToString("f0");
        volumeSliders[3].value = gameData.musicVolume.Value;
        volumeTexts[3].text = volumeSliders[3].value.ToString("f0");
        volumeSlidersSetted = true;
    }

}







