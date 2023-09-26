using UnityEngine;
using UniRx;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ScenaryData", menuName = "Scriptables/Scenary Data", order = 3)]
public class ScenaryData : ScriptableObject
{
    [Header("Scenary info")]
    public float xHalf;
    public float yHalf;
    public float minDistance;
    public string scenaryName;
    public List<AudioClip> musicToPlay;

    [Header("Player info")]
    public int initialHealth;

    [Header("Enemies info")]
    public List<GameObject> enemiesPrefabs;
    public int minEnemiesInScene;
    public List<int> waves = new List<int>();
    public ReactiveProperty<int> actualWave = new ReactiveProperty<int>();
    public ReactiveProperty<int> enemiesInScene = new ReactiveProperty<int>();




    public bool IsLastWave()
    {
        return waves.Count == actualWave.Value +1;
    }

    public bool AreLessThanMinEnemiesInScene()
    {
        return minEnemiesInScene > enemiesInScene.Value; 
    }

    public GameObject GetRandomEnemyPrefab()
    {
        return enemiesPrefabs[Random.Range(0, enemiesPrefabs.Count)];
    }

}