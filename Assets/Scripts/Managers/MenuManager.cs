using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameData gameData;
    [SerializeField] List<AudioClip> musicToPlay;
    [SerializeField] Toggle infiniteToggle;

    private void Awake()
    {
        gameData.actualMusicToPlay = musicToPlay;
    }

    public void PlaySceneary()
    {
        gameData.infinite = infiniteToggle.isOn;
       gameData.actualScenaryData =  gameData.allScenariesDatas[Random.Range(0, gameData.allScenariesDatas.Count)];
        SceneManager.LoadScene(gameData.actualScenaryData.scenaryName);
    }
}
