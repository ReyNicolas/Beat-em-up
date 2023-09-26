using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIDash : MonoBehaviour
{
    Image image;
    Color colorWhiteAlphaZero = new Color(1, 1, 1, 0);
    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = colorWhiteAlphaZero;
    }

    private void Start()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().dashTimer.Where(value => value < 0).Subscribe(_ => image.color = Color.white);
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().dashTimer.Where(value => value > 0 && image.color.a == 1 ).Subscribe(_ => image.color = colorWhiteAlphaZero);

    }
}
