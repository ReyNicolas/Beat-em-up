using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameData gameData;
    

    public void PlaySceneary()
    {
       gameData.actualScenaryData =  gameData.allScenariesDatas[Random.Range(0, gameData.allScenariesDatas.Count)];
        SceneManager.LoadScene(gameData.actualScenaryData.scenaryName);
    }
}
