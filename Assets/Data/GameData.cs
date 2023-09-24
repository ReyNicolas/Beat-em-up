using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptables/Game Data", order = 2)]
public class GameData : ScriptableObject
{
    public PlayerData playerdata;
    public ScenaryData scenaryData;

    public void SetStartingValues()
    {
        playerdata.playerHealth.Value = scenaryData.initialHealth;
        scenaryData.actualWave.Value = 0;
        scenaryData.enemiesInScene.Value = 0;
    }

}
