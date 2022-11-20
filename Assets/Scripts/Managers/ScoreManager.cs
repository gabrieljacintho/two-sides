using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score;
    public bool scoreExceeded = false;

    private void Awake()
    {
        instance = this;
    }

    public void AddScore()
    {
        score++;
        UIManager.instance.UpdateScoreText();
        if (score >= PlayerPrefs.GetInt("HighScore", 0))
        {
            if (!scoreExceeded)
            {
                scoreExceeded = true;
                UIManager.instance.ActivateHighScoreTextAnimation();
            }
            UIManager.instance.UpdateHighScoreText();
        }

        if (UIManager.instance.tutorialIconObj != null)
        {
            if (UIManager.instance.tutorialIconObj.activeSelf && score > 3)
            {
                UIManager.instance.tutorialIconObj.SetActive(false);
            }
        }
    }

    public void UpdateHighScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            UIManager.instance.UpdateHighScoreText();
        }
    }
}
