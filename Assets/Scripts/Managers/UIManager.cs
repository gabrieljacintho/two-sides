using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private Text scoreText = null;
    private Animator scoreTextAnimator;
    [SerializeField]
    private Text highScoreText = null;
    private Animator highScoreTextAnimator;

    public Animator mouseAnimator = null;
    private RectTransform mouseRectTransform;

    public GameObject tutorialIconObj = null;

    [SerializeField]
    private Animator pauseButtonAnimator = null;
    private Image pauseButtonImage;
    [SerializeField]
    private Animator pausePanelAnimator = null;

    [SerializeField]
    private Animator playButtonAnimator = null;
    private Image playButtonImage = null;

    [SerializeField]
    private Image muteButtonImage = null;
    [SerializeField]
    private Sprite withAudioSprite = null;
    [SerializeField]
    private Sprite withoutAudioSprite = null;

    private GameObject[] mainMenuObjs;

    [SerializeField]
    private AudioClip highScoreToScoreSound = null;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("Tutorial", 0) >= 2)
        {
            if (mouseAnimator != null)
            {
                Destroy(mouseAnimator.gameObject);
            }

            if (tutorialIconObj != null)
            {
                Destroy(tutorialIconObj);
            }
        }

        mouseRectTransform = mouseAnimator.GetComponent<RectTransform>();
        pauseButtonImage = pauseButtonAnimator.GetComponent<Image>();
        playButtonImage = playButtonAnimator.GetComponent<Image>();
        scoreTextAnimator = scoreText.GetComponent<Animator>();
        highScoreTextAnimator = highScoreText.GetComponent<Animator>();
        mainMenuObjs = GameObject.FindGameObjectsWithTag("MainMenu");

        highScoreTextAnimator.enabled = false;
        highScoreText.transform.localScale = new Vector3(1.25f, 1.25f, 1);
        if (PlayerPrefs.GetInt("HighScore", 0) > 0)
        {
            UpdateHighScoreText();
            highScoreText.rectTransform.anchoredPosition = new Vector2(0, 410);
        }
        else
        {
            highScoreText.color = new Color(highScoreText.color.r, highScoreText.color.g, highScoreText.color.b, 0);
        }

        scoreTextAnimator.enabled = false;
        scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 0);
    }

    public void AddMainMenuObjs()
    {
        foreach (GameObject mainMenuObj in mainMenuObjs)
        {
            mainMenuObj.GetComponent<Animator>().SetTrigger("Add");
        }

        if (mouseAnimator != null)
        {
            if (mouseAnimator.gameObject.activeSelf)
            {
                mouseAnimator.gameObject.SetActive(false);
            }
        }

        if (tutorialIconObj != null)
        {
            if (tutorialIconObj.activeSelf)
            {
                tutorialIconObj.SetActive(false);
            }
        }
        
        pauseButtonAnimator.SetTrigger("Remove");
        pauseButtonImage.raycastTarget = false;
        playButtonImage.raycastTarget = true;
    }

    public void RemoveMainMenuObjs()
    {
        foreach (GameObject mainMenuObj in mainMenuObjs)
        {
            mainMenuObj.GetComponent<Animator>().SetTrigger("Remove");
        }

        if (PlayerPrefs.GetInt("Tutorial", 0) < 2)
        {
            StartCoroutine(ActiveTutorial());
            PlayerPrefs.SetInt("Tutorial", PlayerPrefs.GetInt("Tutorial", 0) + 1);
            PlayerPrefs.Save();
        }
        else
        {
            if (mouseAnimator != null)
            {
                Destroy(mouseAnimator.gameObject);
            }

            if (tutorialIconObj != null)
            {
                Destroy(tutorialIconObj);
            }
        }

        pauseButtonAnimator.SetTrigger("Add");
        pauseButtonImage.raycastTarget = true;
    }

    public void UpdateScoreText()
    {
        scoreText.text = ScoreManager.instance.score.ToString();
    }

    public void UpdateHighScoreText()
    {
        if (GameManager.instance.gameInProgress)
        {
            highScoreText.text = scoreText.text;
        }
        else
        {
            highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        }
    }

    public void ActivateScoreTextAnimation()
    {
        if (!scoreTextAnimator.enabled) scoreTextAnimator.enabled = true;

        if (scoreText.color.a == 0)
        {
            scoreTextAnimator.SetTrigger("Add");
        }
        else
        {
            scoreTextAnimator.SetTrigger("Remove");
        }
    }

    public void ActivateHighScoreTextAnimation()
    {
        if (!highScoreTextAnimator.enabled) highScoreTextAnimator.enabled = true;

        if (highScoreText.color.a == 0)
        {
            highScoreTextAnimator.SetTrigger("Add");
        }
        else if (GameManager.instance.gameInProgress)
        {
            if (ScoreManager.instance.scoreExceeded)
            {
                if (highScoreText.rectTransform.localScale == Vector3.one)
                {
                    highScoreTextAnimator.SetTrigger("HighScoreToScore");
                    SongsManager.instance.audioSource.PlayOneShot(highScoreToScoreSound, 0.5f);
                }
            }
            else
            {
                highScoreTextAnimator.SetTrigger("MainMenuToHighScore");
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("HighScore", 0) > 0)
            {
                if (highScoreText.rectTransform.localScale == Vector3.one)
                {
                    highScoreTextAnimator.SetTrigger("HighScoreToMainMenu");
                }
                else
                {
                    highScoreTextAnimator.SetTrigger("ScoreToMainMenu");
                }
            }
            else
            {
                highScoreTextAnimator.SetTrigger("Remove");
            }
        }
    }

    public void MuteButton()
    {
        SongsManager.instance.audioSource.mute = SongsManager.instance.audioSource.mute ? false : true;
        if (SongsManager.instance.audioSource.mute)
        {
            muteButtonImage.sprite = withoutAudioSprite;
        }
        else
        {
            muteButtonImage.sprite = withAudioSprite;
        }
    }

    public void PlayButton()
    {
        if (GameManager.instance.canStart && !GameManager.instance.gameInProgress)
        {
            GameManager.instance.StartGame();
            playButtonAnimator.SetTrigger("Remove");
            playButtonImage.raycastTarget = false;
        }
        else if (GameManager.instance.gameInProgress)
        {
            Time.timeScale = 1;
            pausePanelAnimator.SetTrigger("Remove");
            pauseButtonAnimator.SetTrigger("Add");
            pauseButtonImage.raycastTarget = true;
            playButtonAnimator.SetTrigger("Remove");
            playButtonImage.raycastTarget = false;
        }
    }

    public void PauseButton()
    {
        Time.timeScale = 0;
        pausePanelAnimator.SetTrigger("Add");
        pauseButtonAnimator.SetTrigger("Remove");
        pauseButtonImage.raycastTarget = false;
        playButtonAnimator.SetTrigger("Add");
        playButtonImage.raycastTarget = true;
    }

    private IEnumerator ActiveTutorial()
    {
        yield return new WaitForSeconds(1);
        mouseRectTransform.anchoredPosition = new Vector2(-220, -390);
        mouseAnimator.gameObject.SetActive(true);
        tutorialIconObj.SetActive(true);
    }

}
