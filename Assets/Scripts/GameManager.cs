using UnityEngine;
using UniRx;
using System;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameData gameData;
    [SerializeField] GameObject optionsGO;

    public event Action<GameObject> OnEnemyCreated;
    ScenaryData scenaryData;
    GameObject playerGO;
    PositionAssigner positionAssigner;
    float timeScaleBeforeMenu;

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
        var update = Observable.EveryUpdate();
        // update
        compositeDisposable.Add
            (update
            .Where(_ => PressedMenu())
            .Subscribe(_ => OpenCloseMenu()));

        // wave control
        compositeDisposable.Add(
            scenaryData.enemiesInScene
             .Where(_ => scenaryData.AreLessThanMinEnemiesInScene())
             .Subscribe(_ => ChangeWave()));

        compositeDisposable.Add(
            scenaryData.enemiesInScene
              .Where(_ => scenaryData.IsLastWave() && scenaryData.enemiesInScene.Value == 0)
              .Subscribe(_ => Invoke("StopGame",1)));

        // when player die
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


    bool PressedMenu() =>
        Input.GetKeyDown(KeyCode.Escape);
    void OpenCloseMenu()
    {        
        if (!optionsGO.activeSelf)
        {
            timeScaleBeforeMenu = Time.timeScale;
            StopGame();
        }
        else
        {
            Time.timeScale = timeScaleBeforeMenu;
            playerGO.GetComponent<PlayerController>().SubscribeLogic();   
        }
        optionsGO.SetActive(!optionsGO.activeSelf);        
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

    void UpdateWave() => 
        scenaryData.enemiesInScene.Value--;

    void ChangeWave()
    {
        if (scenaryData.IsLastWave()) return;
        scenaryData.actualWave.Value++;
        StartScenearyWave();
    }

    bool IsDead(int actualHealth)
        => actualHealth <= 0;

    void StopGame()
    {
        Time.timeScale = 0;
        playerGO.GetComponent<PlayerController>().UnSubscribe();
    }
    public void ReturnHomeMenu()
    {
        SceneManager.LoadScene(gameData.homeMenuScene);
    }

}
