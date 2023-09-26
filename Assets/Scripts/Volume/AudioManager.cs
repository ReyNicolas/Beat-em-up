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
    }

    void PlayNewTrack() =>
        musicSource.PlayOneShot(GetRandomMusic());

    bool MusicEnd() => 
        !musicSource.isPlaying;

    AudioClip GetRandomMusic() =>
        gameData.actualMusicToPLay[Random.Range(0, gameData.actualMusicToPLay.Count)];


}
