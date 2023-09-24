using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float duration;
    float actualDuration;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        actualDuration = duration;
    }

    // Update is called once per frame
    void Update()
    {
        actualDuration -= Time.deltaTime;
        spriteRenderer.color = new Color(1, 1, 1, actualDuration / duration);
        if (actualDuration < 0) Destroy(gameObject);
    }
}
