using UniRx;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [SerializeField] GameData gameData;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource waveSource;
    [SerializeField] AudioSource interfaceSounds;
    CompositeDisposable compositeDisposable = new CompositeDisposable();

    private void Awake()
    {
        gameData.SetAudioMixer();         
    }

    private void OnEnable()
    {
        var update = Observable.EveryUpdate();
        // update
        compositeDisposable.Add
            (update
            .Where(_ => MusicEnd())
            .Subscribe(_ => PlayNewTrack()));
    }

    private void OnDisable()
    {
        compositeDisposable.Dispose();
        gameData.DisposeMixer();
    }

    void PlayNewTrack()
    {
        musicSource.clip = GetRandomMusic();
        musicSource.Play();
    }
    bool MusicEnd() => 
        !musicSource.isPlaying;

    AudioClip GetRandomMusic() =>
        gameData.actualMusicToPlay[Random.Range(0, gameData.actualMusicToPlay.Count)];


}
