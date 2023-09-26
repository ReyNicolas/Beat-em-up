
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FXFireBall : MonoBehaviour
{
    [SerializeField] float changeSizeSecond;
    [SerializeField] float minSize;
    [SerializeField] float maxSize;
    [SerializeField] float changeLightIntensitySecond;
    [SerializeField] Light2D light;

    private void Update()
    {
       

        transform.localScale -= changeSizeSecond * Time.deltaTime * Vector3.one;
        light.intensity -= changeLightIntensitySecond * Time.deltaTime;
        if (transform.localScale.x < minSize)
        {            
            transform.localScale = Vector3.one * minSize;
            changeSizeSecond *= -1;
            changeLightIntensitySecond *= -1;
        }
        else if (transform.localScale.x > maxSize) 
        { 
            transform.localScale = Vector3.one * maxSize;
            changeSizeSecond *= -1;
            changeLightIntensitySecond *= -1;

        }
    }
}
