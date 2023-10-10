using UnityEngine;
using UniRx;
using System;
using UnityEngine.SceneManagement;

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
        scenaryData = gameData.actualScenaryData;
        gameData.actualMusicToPlay = scenaryData.musicToPlay;
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

        if (gameData.infinite)
        {
            compositeDisposable.Add(
            scenaryData.enemiesInScene
             .Where(_ => scenaryData.AreLessThanMinEnemiesInScene())
             .Subscribe(_ => StartScenearyWave()));
        }

        compositeDisposable.Add(
            scenaryData.enemiesInScene
              .Where(_ => scenaryData.IsLastWave() && scenaryData.enemiesInScene.Value == 0)
              .Subscribe(_ => Invoke("OnGameEnd", 1)));

        // when player die
        compositeDisposable.Add(
            playerGO.GetComponent<PlayerHealthLogic>().actualHealth
            .Where(IsDead)
            .Subscribe(_ => Invoke("OnGameEnd", 1))
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
            newEnemy.GetComponent<EnemyHealthLogic>().actualHealth.Where(IsDead).Subscribe(_ => UpdateEnemiesWave());
            OnEnemyCreated?.Invoke(newEnemy);
        }
        scenaryData.enemiesInScene.Value += scenaryData.waves[scenaryData.actualWave.Value];
    }

    void UpdateEnemiesWave()
    {
        scenaryData.enemiesInScene.Value--;
        gameData.enemiesKilled.Value++;
    }

    void ChangeWave()
    {
        if (scenaryData.IsLastWave()) return;
        scenaryData.actualWave.Value++;
        StartScenearyWave();
    }

    bool IsDead(int actualHealth)
        => actualHealth <= 0;

    void OnGameEnd()
    {
        StopGame();
        optionsGO.SetActive(true); 
    }

    void StopGame()
    {
        Time.timeScale = 0;
        playerGO.GetComponent<PlayerController>().UnSubscribe();
    }
    public void ReturnHomeMenu()
    {
        SceneManager.LoadScene(gameData.homeMenuScene);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        SceneManager.LoadScene(scenaryData.scenaryName);
        Time.timeScale = 1;
    }

}
