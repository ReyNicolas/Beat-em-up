
using System.Collections.Generic;
using UnityEngine;

public class AudioEntityControl : MonoBehaviour
{
    [SerializeField] List<AudioClip> attackAudioClip;
    [SerializeField] List<AudioClip> damageAudioClip;
    [SerializeField] List<AudioClip> deadAudioClip;
    [SerializeField] AudioSource audioSource;
    IEventEntity audioEntity;

    private void Awake()
    {
       audioEntity =  GetComponent<IEventEntity>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        audioEntity.onAttack += PlayAttackClip;
        audioEntity.onTakeDamage += PlayDamageClip;
        audioEntity.onDead += PlayDeadClip;
    }

    private void OnDisable()
    {
        audioEntity.onAttack -= PlayAttackClip;
        audioEntity.onTakeDamage -= PlayDamageClip;
        audioEntity.onDead -= PlayDeadClip;
    }

    void PlayAttackClip() =>
        audioSource.PlayOneShot(attackAudioClip[Random.Range(0, attackAudioClip.Count)]);
    void PlayDamageClip() =>
        audioSource.PlayOneShot(damageAudioClip[Random.Range(0, damageAudioClip.Count)]);
    void PlayDeadClip() =>
        audioSource.PlayOneShot(deadAudioClip[Random.Range(0, deadAudioClip.Count)]);
}
