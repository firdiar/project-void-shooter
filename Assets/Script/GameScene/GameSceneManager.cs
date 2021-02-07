using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnemyType {
    public Plane prefabs;
    public int cost = 1;
    public int probability = 1;
}

public class GameSceneManager : MonoBehaviour
{

    public static GameSceneManager main = null;

    public GameObject player = null;

    int totalProbability = 0;
    [SerializeField] EnemyType[] enemyType = new EnemyType[0];

    List<GameObject> enemy = new List<GameObject>();

    [SerializeField] int wave = 0;
    [SerializeField] int costLeft = 0;
    [SerializeField] int score = 0;
    [SerializeField] AudioClip bgm = null;
    [Header("UI")]
    [SerializeField] Text waveText = null;
    [SerializeField] Text scoreText = null;
    [SerializeField] GameObject pause = null;
    [SerializeField] GameObject over = null;
    [SerializeField] GameObject finish = null;

    void Start()
    {
        GtionProduction.GtionBGM.Play(bgm);
        Physics2D.IgnoreLayerCollision(8, 9);
        main = this;
        InvokeRepeating("Check", 2, 2);
        foreach (EnemyType e in enemyType)
        {
            totalProbability += e.probability;
        }

        
    }


    public void PauseGame() {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    public void ShowPause()
    {
        PauseGame();
        pause.SetActive(true);
    }
    public void HidePause()
    {
        ResumeGame();
        pause.SetActive(false);
    }
    public void ShowOver()
    {
        //PauseGame();
        if (score > PlayerPrefs.GetInt("highscore", 25)) {
            PlayerPrefs.SetInt("highscore", score);
        }
        over.SetActive(true);
    }

    public void AddScore(int scoreBonus) {
        score += scoreBonus;
        scoreText.text = "Score : " + score;
    }

    public void GotoMain()
    {
        ResumeGame();
        GtionProduction.GtionLoading.openLayerLoading(() =>
        {
            GtionProduction.GtionLoading.startMoveScene("HomeScene");
        });

    }

    public void Retry()
    {
        ResumeGame();
        GtionProduction.GtionLoading.openLayerLoading(() =>
        {
            GtionProduction.GtionLoading.startMoveScene("GameScene");
        });

    }


    void Check() {

        if (costLeft > 0)
            return;
        else
            CancelInvoke("SummonEnemy");

        for (int i = enemy.Count - 1; i >= 0; i--) {
            if (enemy[i] == null) {
                enemy.RemoveAt(i);
            }
        }

        //Debug.Log(enemy.Count);
        if (enemy.Count == 0 && costLeft <= 0)
            NewWave();
    }

    void NewWave() {
        wave++;
        waveText.text = "Wave : " + wave;
        costLeft = wave;
        InvokeRepeating("SummonEnemy", 4, 1);
    }

    void SummonEnemy() {

        if (costLeft <= 0)
            return;

        int prob = Random.Range(0, totalProbability);
        foreach (EnemyType e in enemyType)
        {
            prob -= e.probability;
            if (prob <= 0)
            {
                Summon(e);
                break;
            }
        }

    }

    void Summon(EnemyType e) {

        costLeft -= e.cost;
        Vector2 pos = transform.position + new Vector3(
                       (Random.value - 0.5f) * 137.6007f,
                       (Random.value - 0.5f) * 45,
                       (Random.value - 0.5f) * 0
                    );

        Plane p = Instantiate(e.prefabs, pos, Quaternion.identity);
        p.health = wave * e.cost;
        p.attack = wave * e.cost;

        enemy.Add(p.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
