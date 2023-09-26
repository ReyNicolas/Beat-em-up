using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptables/Game Data", order = 2)]
public class GameData : ScriptableObject
{
    [Header("Game info")]
    public string homeMenuScene;
    public PlayerData playerdata;
    public ScenaryData actualScenaryData;
    public List<ScenaryData> allScenariesDatas;
    [Header("Sound Settings")]
    public AudioMixer mixer;
    public ReactiveProperty<float> masterVolume = new ReactiveProperty<float>(1);
    public ReactiveProperty<float> musicVolume = new ReactiveProperty<float>(1);
    public ReactiveProperty<float> soundEffectsVolume = new ReactiveProperty<float>(1);
    public ReactiveProperty<float> interfaceVolume = new ReactiveProperty<float>(1);
    public float volumeMultiplier;
    [Header("Music")]
    public List<AudioClip> allMusic;
    public List<AudioClip> actualMusicToPlay;
    CompositeDisposable compositeDisposable;

    public void SetStartingValues()
    {
        playerdata.playerHealth.Value = actualScenaryData.initialHealth;
        actualScenaryData.actualWave.Value = 0;
        actualScenaryData.enemiesInScene.Value = 0;
    }


    public void DisposeMixer()
    {
        compositeDisposable.Clear();
    }
    public void SetAudioMixer()
    {
        compositeDisposable = new CompositeDisposable
        {
            masterVolume.Subscribe(value => SetAudio("master", value / 100)),
            musicVolume.Subscribe(value => SetAudio("bgm", value / 100)),
            soundEffectsVolume.Subscribe(value => SetAudio("sfx", value / 100)),
            interfaceVolume.Subscribe(value => SetAudio("sui", value / 100))
        };
    }
    void SetAudio(string param, float value)
        => mixer.SetFloat(param, Mathf.Log10(value) * volumeMultiplier);

}
