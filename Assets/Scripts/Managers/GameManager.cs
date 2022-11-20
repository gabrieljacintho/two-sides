using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool canStart = true;
    public bool gameInProgress = false;

    public float gravityScale;
    public readonly float initialGravityScale = 0.2f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gravityScale = initialGravityScale;
    }

    private void Update()
    {
        if (gameInProgress)
        {
            gravityScale += 0.01f * Time.deltaTime;

            if (gravityScale >= 0.4)
            {
                if (SpawnManager.instance.ballsPerSecond == SpawnManager.instance.initialBallsPerSecond)
                {
                    SpawnManager.instance.ballsPerSecond++;
                }
                else if (gravityScale >= 0.8)
                {
                    if (SpawnManager.instance.ballsPerSecond == SpawnManager.instance.initialBallsPerSecond + 1)
                    {
                        SpawnManager.instance.ballsPerSecond++;
                    }
                    else if (gravityScale >= 1.2 && SpawnManager.instance.ballsPerSecond == SpawnManager.instance.initialBallsPerSecond + 2)
                    {
                        SpawnManager.instance.ballsPerSecond++;
                    }
                }
            }
        }
    }

    public void StartGame()
    {
        GameObject[] whiteBalls = GameObject.FindGameObjectsWithTag("WhiteBall");
        if (whiteBalls.Length > 0)
        {
            foreach (GameObject whitebBall in whiteBalls)
            {
                Destroy(whitebBall);
            }
        }

        GameObject[] blackBalls = GameObject.FindGameObjectsWithTag("BlackBall");
        if (blackBalls.Length > 0)
        {
            foreach (GameObject blackBall in blackBalls)
            {
                Destroy(blackBall);
            }
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) Destroy(player);

        gravityScale = initialGravityScale;
        canStart = false;
        gameInProgress = true;
        SpawnManager.instance.ballsPerSecond = SpawnManager.instance.initialBallsPerSecond;
        ScoreManager.instance.score = 0;
        ScoreManager.instance.scoreExceeded = false;
        UIManager.instance.UpdateScoreText();
        UIManager.instance.RemoveMainMenuObjs();
        UIManager.instance.ActivateScoreTextAnimation();
        UIManager.instance.ActivateHighScoreTextAnimation();
        SpawnManager.instance.SpawnPlayer();
        SpawnManager.instance.StartSpawnBallTimer();
    }

    public void GameOver()
    {
        gameInProgress = false;
        ScoreManager.instance.UpdateHighScore();
        UIManager.instance.AddMainMenuObjs();
        UIManager.instance.ActivateScoreTextAnimation();
        UIManager.instance.ActivateHighScoreTextAnimation();
        AdsManager.instance.ShowAd();
        StartCoroutine(CanStartTimer());
        SpawnManager.instance.StartInitialSpawnBallTimer();
    }

    private IEnumerator CanStartTimer()
    {
        yield return new WaitForSeconds(1);
        canStart = true;
    }
}
