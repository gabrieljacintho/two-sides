using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField]
    private GameObject playerObjPrefab = null;

    [SerializeField]
    public Animator spinesAnimator;

    [SerializeField]
    private GameObject whiteBallObjPrefab = null;
    [SerializeField]
    private GameObject blackBallObjPrefab = null;

    public int ballsPerSecond;
    public int initialBallsPerSecond = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ballsPerSecond = initialBallsPerSecond;
        StartCoroutine(InitialSpawnBallTimer());
    }

    public void SpawnPlayer()
    {
        Instantiate(playerObjPrefab, new Vector2(0, -7), Quaternion.identity);
        spinesAnimator.SetTrigger("Add");
    }

    public void SpawnBall()
    {
        if (Random.Range(0, 3) == 0)
        {
            Instantiate(whiteBallObjPrefab, new Vector2(Random.Range(-2.6f, 2.6f), 8), Quaternion.identity);
        }
        else
        {
            Instantiate(blackBallObjPrefab, new Vector2(Random.Range(-2.6f, 2.6f), 8), Quaternion.identity);
        }
    }

    public void StartSpawnBallTimer()
    {
        StartCoroutine(SpawnBallTimer());
    }

    public void StartInitialSpawnBallTimer()
    {
        StartCoroutine(InitialSpawnBallTimer());
    }

    private IEnumerator SpawnBallTimer()
    {
        for (int i = 0; i < ballsPerSecond; i++)
        {
            SpawnBall();
            yield return new WaitForSeconds(1.0f / ballsPerSecond);
        }

        if (GameManager.instance.gameInProgress) StartCoroutine(SpawnBallTimer());
    }

    private IEnumerator InitialSpawnBallTimer()
    {
        for (int i = 0; i < ballsPerSecond; i++)
        {
            SpawnBall();
            yield return new WaitForSeconds(1.0f / 6);
        }

        if (!GameManager.instance.gameInProgress) StartCoroutine(InitialSpawnBallTimer());
    }
}
