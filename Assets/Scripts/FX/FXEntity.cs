using Cinemachine;
using UnityEngine;

public class FXEntity : MonoBehaviour
{
    [Header("Screen shake FX")]
    CinemachineImpulseSource screenShake;
    [SerializeField] float shakeMultiplier;
    [SerializeField] Vector3 shakePower;
    [SerializeField] Transform entityTransform;

    private void Awake()
    {
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        IEventEntity eventEntity = entityTransform.GetComponent<IEventEntity>();
        eventEntity.onTakeDamage += ScreenShake;
        eventEntity.onDead += ScreenShake;
    }

    private void OnDisable()
    {
        IEventEntity eventEntity = entityTransform.GetComponent<IEventEntity>();
        eventEntity.onTakeDamage -= ScreenShake;
        eventEntity.onDead -= ScreenShake;
    }

    void ScreenShake()
    {
       // screenShake.m_DefaultVelocity = new Vector3(shakePower.x * entityTransform.localScale.x, shakePower.y ) * shakeMultiplier;
        screenShake.m_DefaultVelocity = new Vector3(shakePower.x * Random.Range(-1,2) , shakePower.y * Random.Range(-1, 2)) * shakeMultiplier;        
        screenShake.GenerateImpulse();
    }
}
