using UnityEngine;
using UniRx;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptables/Player Data", order = 1)]
public class PlayerData : ScriptableObject
{
    public int playerLevel;
    public ReactiveProperty<int> playerHealth = new ReactiveProperty<int>(0);

   public void Dispose()
    {
        playerHealth.Dispose();
    }
}
