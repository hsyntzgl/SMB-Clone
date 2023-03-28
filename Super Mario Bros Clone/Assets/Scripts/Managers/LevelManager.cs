using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public static bool newGame = true;

    public static int marioLifeCount = 3;
    public static int score;
    public static int coin;

    public bool beHurry = false;

    public LevelTheme levelTheme;

    [SerializeField] private TextMeshProUGUI scoreTextGUI, coinTextGUI, timeTextGUI;

    [SerializeField] private GameObject blackScreen;

    [SerializeField] private GameObject playerCanContinuePanel, gameOverPanel, worldClearPanel;

    private float gameTime = 400;

    private bool isGamePlaying = false;
    private bool isLevelCompleted = false;

    private bool isPaused = false;

    private Animator playerAnimator;

    private readonly float startScreenTime = 2f;
    private readonly float restartGameTime = 3f;
    private readonly float groundPositionY = -4f;
    private readonly float flagGroundPositionY = -2f;

    private float GameTime
    {
        get => gameTime;
        set
        {
            if (value <= 0 && !isLevelCompleted)
            {
                TimeOver();
            }
            gameTime = value;
            timeTextGUI.SetText("TIME<br>{0}", (int)gameTime);
        }
    }
    public int Score
    {
        get => score;
        set
        {
            score = value;
            scoreTextGUI.SetText("MARIO<br>" + score.ToString("000000"));
        }
    }

    public int MarioLifeCount
    {
        get => marioLifeCount;
        set
        {
            marioLifeCount++;
        }
    }
    public int Coin
    {
        get => coin;
        set
        {
            if (value < 100)
            {
                coin = value;
            }
            else
            {
                coin = 0;
                MarioLifeCount++;
            }
            coinTextGUI.SetText(" x " + coin.ToString("00"));
        }
    }
    public enum LevelTheme
    {
        OverWorld,
        UnderGround
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (newGame)
        {
            Time.timeScale = 0f;
            blackScreen.SetActive(true);

            StartCoroutine(StartGame(newGame));

            newGame = false;
        }
        else
        {
            StartCoroutine(StartGame(newGame));
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GameObject.Find("Mario").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGamePlaying && !isLevelCompleted)
        {
            Debug.Log("Erişim devam ediyor");
            GameTime -= Time.deltaTime * 2;
            if (gameTime <= 100 && !beHurry)
            {
                beHurry = true;
                AudioManager.instance.PlayLevelMusic();
            }
            if (Input.GetKeyDown(KeyCode.Return)) IsPaused();
        }
    }
    private void TimeOver()
    {
        GameObject.Find("Mario").GetComponent<MarioCodeAnimations>().MarioDeathAnimation();
        StartCoroutine(MarioDeadProcess());
    }
    private void IsPaused()
    {
        if (!isPaused)
        {
            isPaused = true;
            playerAnimator.updateMode = AnimatorUpdateMode.Normal;
            Time.timeScale = 0f;
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1f;
            playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
        AudioManager.instance.Pause(isPaused);
    }
    private void ResetStaticVariables()
    {
        newGame = true;

        marioLifeCount = 3;
        score = 0;
        coin = 0;
    }
    private IEnumerator AddTimeToScore()
    {
        while (gameTime > 0)
        {
            Score += 50;
            GameTime--;
            yield return new WaitForSeconds(.01f);
        }
        yield return CastleFlagAnimation();
        StartCoroutine(WorldClear());
    }
    private IEnumerator StartGame(bool currentNewGameValue)
    {
        if (currentNewGameValue)
            yield return new WaitForSecondsRealtime(startScreenTime);

        Time.timeScale = 1f;
        blackScreen.SetActive(false);
        isGamePlaying = true;
        AudioManager.instance.PlayLevelMusic();
    }
    private IEnumerator MarioDeadProcess()
    {
        Time.timeScale = 0f;
        isGamePlaying = false;

        float waitTime = AudioManager.instance.MarioDead();

        yield return new WaitForSecondsRealtime(waitTime);

        timeTextGUI.SetText("TIME<br> ");

        if (--marioLifeCount <= 0)
        {
            gameOverPanel.SetActive(true);
            playerCanContinuePanel.SetActive(false);

            StartCoroutine(GameOver());
        }
        else
        {
            playerCanContinuePanel.SetActive(true);
            gameOverPanel.SetActive(false);

            playerCanContinuePanel.GetComponentInChildren<TextMeshProUGUI>().SetText("x  {0}", marioLifeCount);

            StartCoroutine(RestartGame());
        }
        blackScreen.SetActive(true);

        yield return null;
    }
    private IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(restartGameTime);

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private IEnumerator GameOver()
    {
        float waitTime = AudioManager.instance.GameOver();

        yield return new WaitForSecondsRealtime(waitTime);

        ResetStaticVariables();

        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }
    private IEnumerator WorldClear()
    {
        playerCanContinuePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        worldClearPanel.SetActive(true);
        blackScreen.SetActive(true);

        float waitTime = AudioManager.instance.WorldClear();

        yield return new WaitForSecondsRealtime(waitTime);

        ResetStaticVariables();

        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }
    private IEnumerator PoleProcess()
    {
        isLevelCompleted = true;

        GameObject flag = GameObject.Find("Flag");

        Vector2 flagStartPosition = flag.transform.position;
        Vector2 flagEndPosition = new Vector2(flag.transform.position.x, flagGroundPositionY);

        GameObject mario = GameObject.Find("Mario");
        Animator marioAnimator = mario.GetComponent<Animator>();
        PlayerController playerController = mario.GetComponent<PlayerController>();

        Vector2 marioOnPolePosition = mario.transform.position;
        Vector2 marioOnTheGroundPosition = new Vector2(marioOnPolePosition.x, groundPositionY);

        float ellapsed = 0f;
        float duration = 1f;

        playerController.ResetPlayerVelocity();
        playerController.IsControllerActive = false;
        
        marioAnimator.SetBool("Pole", true);
        AudioManager.instance.FlagPole();


        while (ellapsed < duration)
        {
            float t = ellapsed / duration;

            flag.transform.position = Vector2.Lerp(flagStartPosition, flagEndPosition, t);
            mario.transform.position = Vector2.Lerp(marioOnPolePosition, marioOnTheGroundPosition, t);

            ellapsed += Time.deltaTime;

            yield return null;
        }

        Vector2 sidePole = new Vector2(marioOnTheGroundPosition.x + 1.5f, groundPositionY);

        mario.transform.position = sidePole;
        marioAnimator.SetBool("Pole", false);
        playerController.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;

        AudioManager.instance.LevelClear();

        while (mario.activeSelf)
        {
            playerController.WalkSlowSpeed();
            yield return null;
        }
        yield return AddTimeToScore();
    }
    private IEnumerator CastleFlagAnimation()
    {
        GameObject castleFlag = GameObject.Find("Castle Flag");

        Vector2 castleFlagStartPosition = castleFlag.transform.position;
        Vector2 castleFlagEndingPosition = new Vector2(castleFlagStartPosition.x, castleFlagStartPosition.y + 2f);

        float ellapsed = 0f;
        float duration = .5f;

        while (ellapsed < duration)
        {
            float t = ellapsed / duration;

            castleFlag.transform.position = Vector2.Lerp(castleFlagStartPosition, castleFlagEndingPosition, t);

            ellapsed += Time.deltaTime;

            yield return null;
        }

        float flagVisualTime = .5f;

        yield return new WaitForSecondsRealtime(flagVisualTime);
    }
    public void MarioDead()
    {
        StartCoroutine(MarioDeadProcess());
    }
    public void StartPoleProcess()
    {
        StartCoroutine(PoleProcess());
    }
}
