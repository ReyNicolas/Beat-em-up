
using System.Collections.Generic;
using UnityEngine;

public class AudioHitController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AttackLogic attackLogic;
    [SerializeField] List<AudioClip> hitAudioClip;
    [SerializeField] List<AudioClip> launchAudioClip;

    private void OnEnable()
    {
        attackLogic.OnHitEnemy += PlayHit;
        attackLogic.OnLaunch += PlayLaunch;
    }

    void PlayHit() =>
        audioSource.PlayOneShot(hitAudioClip[Random.Range(0, hitAudioClip.Count)]);

    void PlayLaunch()=>
        audioSource.PlayOneShot(launchAudioClip[Random.Range(0, launchAudioClip.Count)]);

}