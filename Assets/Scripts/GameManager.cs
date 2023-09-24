using UnityEngine;
using UniRx;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameData gameData;
    public event Action<GameObject> OnEnemyCreated;
    ScenaryData scenaryData;
    GameObject playerGO;
    PositionAssigner positionAssigner;

    CompositeDisposable compositeDisposable = new CompositeDisposable();
    private void Awake()
    {        
        gameData.SetStartingValues();
        scenaryData = gameData.scenaryData;
        playerGO = GameObject.FindWithTag("Player");
        positionAssigner = new PositionAssigner(playerGO.transform,scenaryData.xHalf,scenaryData.yHalf,scenaryData.minDistance);
        StartScenearyWave();
    }
   
    private void OnEnable()
    {
        compositeDisposable.Add(
            scenaryData.enemiesInScene
             .Where(_ => scenaryData.AreLessThanMinEnemiesInScene())
             .Subscribe(_ => ChangeWave()));

        compositeDisposable.Add(
            scenaryData.enemiesInScene
              .Where(_ => scenaryData.IsLastWave() && scenaryData.enemiesInScene.Value == 0)
              .Subscribe(_ => Invoke("StopGame",1)));

        compositeDisposable.Add(
            playerGO.GetComponent<PlayerHealthLogic>().actualHealth
            .Where(IsDead)
            .Subscribe(_ => Invoke("StopGame", 1))
            );
    }

    private void OnDisable()
    {
        compositeDisposable.Dispose();
    }

    void StartScenearyWave()
    {
        for (int i = 0; i < scenaryData.waves[scenaryData.actualWave.Value]; i++)
        {
            GameObject newEnemy = Instantiate(scenaryData.GetRandomEnemyPrefab(), positionAssigner.GetPosition(), Quaternion.identity);
            newEnemy.GetComponent<EnemyHealthLogic>().actualHealth.Where(IsDead).Subscribe(_ => UpdateWave());
            OnEnemyCreated?.Invoke(newEnemy);
        }
        scenaryData.enemiesInScene.Value += scenaryData.waves[scenaryData.actualWave.Value];
    }

    void UpdateWave()
    {
        scenaryData.enemiesInScene.Value--;
    }

    private void ChangeWave()
    {
        if (scenaryData.IsLastWave()) return;
        scenaryData.actualWave.Value++;
        StartScenearyWave();
    }

    bool IsDead(int actualHealth)
        => actualHealth <= 0;

    void StopGame() 
        => Time.timeScale = 0;


}
