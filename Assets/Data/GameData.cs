using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptables/Game Data", order = 2)]
public class GameData : ScriptableObject
{
    public string homeMenuScene;
    public PlayerData playerdata;
    public ScenaryData scenaryData;
    [Header("Sound Settings")]
    public AudioMixer mixer;
    public ReactiveProperty<float> masterVolume = new ReactiveProperty<float>(1);
    public ReactiveProperty<float> musicVolume = new ReactiveProperty<float>(1);
    public ReactiveProperty<float> soundEffectsVolume = new ReactiveProperty<float>(1);
    public ReactiveProperty<float> interfaceVolume = new ReactiveProperty<float>(1);
    public float volumeMultiplier;
    public List<AudioClip> allMusic;
    public List<AudioClip> actualMusicToPLay;

    public void SetStartingValues()
    {
        playerdata.playerHealth.Value = scenaryData.initialHealth;
        scenaryData.actualWave.Value = 0;
        scenaryData.enemiesInScene.Value = 0;
    }


    public void DisposeMixer()
    {
        masterVolume.Dispose();
        musicVolume.Dispose();
        soundEffectsVolume.Dispose();
        interfaceVolume.Dispose();
    }
    public void SetAudioMixer()
    {
        masterVolume.Subscribe(value => SetAudio("master", value / 100));
        musicVolume.Subscribe(value => SetAudio("bgm", value / 100));
        soundEffectsVolume.Subscribe(value => SetAudio("sfx", value / 100));
        interfaceVolume.Subscribe(value => SetAudio("sui", value / 100));
    }
    void SetAudio(string param, float value)
        => mixer.SetFloat(param, Mathf.Log10(value) * volumeMultiplier);

}
