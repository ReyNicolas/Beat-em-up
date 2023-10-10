using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemiesKilledUI : MonoBehaviour
{
    [SerializeField] GameData gameData;
    [SerializeField] TextMeshProUGUI countText;

    private void OnEnable()
    {
        countText.text = gameData.enemiesKilled.Value.ToString();
    }
}
