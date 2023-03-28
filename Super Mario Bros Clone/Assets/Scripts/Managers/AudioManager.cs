using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioClip overWorld, hurriedOverWorld, underGround, hurriedUnderGround, star;

    [SerializeField] private AudioClip coin, smallMarioJump, superMarioJump, oneUp, powerUp, powerUpAppears, breakBlock, bumpBlock, fireBall, gameOver, marioDeath, pause, pipeAndLevelDown, warning, stomp, kick, flagPole, levelClear, worldClear;

    private float pauseTime = 0f;

    private AudioSource source;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    // Musics

    private IEnumerator StarMusicPlaying()
    {
        source.Stop();
        source.clip = star;
        source.Play();

        yield return null;
    }

    private AudioClip GetMusic()
    {
        switch (LevelManager.instance.levelTheme)
        {
            case LevelManager.LevelTheme.OverWorld:
                return (LevelManager.instance.beHurry) ? hurriedOverWorld : overWorld;
            case LevelManager.LevelTheme.UnderGround:
                return (LevelManager.instance.beHurry) ? hurriedUnderGround : underGround;
            default:
                return null;
        }
    }
    public void PlayLevelMusic()
    {
        AudioClip levelMusic = GetMusic();

        if (levelMusic == null) return;

        source.Stop();
        source.clip = levelMusic;
        source.time = 0f;
        source.Play();

        Invoke("MusicOver", levelMusic.length);
    }
    public void ContinueLevelMusic()
    {
        StopAllCoroutines();
        source.Stop();
        source.clip = GetMusic();

        if (source.clip == null) return;

        source.time = pauseTime;
        source.Play();
    }

    public void PlayStarMusic()
    {
        pauseTime = source.time;
        StartCoroutine(StarMusicPlaying());
    }
    public void PlayUnderGroundMusic()
    {
        source.Stop();
        source.clip = (LevelManager.instance.beHurry) ? hurriedUnderGround : underGround;
        source.Play();
    }
    private void MusicOver()
    {
        source.time = 0f;
    }

    // Effects
    public void Coin()
    {
        source.PlayOneShot(coin);
    }
    public void MarioJump()
    {
        if (GameObject.Find("Mario").GetComponent<MarioLevelSystem>().CurrentLevel == 0)
        {
            source.PlayOneShot(smallMarioJump);
        }
        else
        {
            source.PlayOneShot(superMarioJump);
        }
    }
    public void OneUp()
    {
        source.PlayOneShot(oneUp);
    }
    public void PowerUp()
    {
        source.PlayOneShot(powerUp);
    }
    public void PowerUpAppears()
    {
        source.PlayOneShot(powerUpAppears);
    }
    public void BreakBlock()
    {
        source.PlayOneShot(breakBlock);
    }
    public void BumpBlock()
    {
        source.PlayOneShot(bumpBlock);
    }
    public void FireBall()
    {
        source.PlayOneShot(fireBall);
    }
    public void Pause(bool isPaused)
    {

        if (isPaused)
        {
            pauseTime = source.time;
            source.Stop();
        }
        else
        {
            source.Play();
            source.time = pauseTime;
        }
        source.PlayOneShot(pause);
    }
    public void Pipe()
    {
        source.Stop();
        source.PlayOneShot(pipeAndLevelDown);
    }
    public void LevelDown()
    {
        source.PlayOneShot(pipeAndLevelDown);
    }
    public void Warning()
    {
        source.PlayOneShot(warning);
    }
    public void Stomp()
    {
        source.PlayOneShot(stomp);
    }
    public void Kick()
    {
        source.PlayOneShot(kick);
    }
    public void FlagPole()
    {
        source.Stop();
        source.PlayOneShot(flagPole);
    }
    public void LevelClear()
    {
        source.PlayOneShot(levelClear);
    }
    public float GameOver()
    {
        source.Stop();
        source.PlayOneShot(gameOver);

        return gameOver.length;
    }
    public float MarioDead()
    {
        source.Stop();
        source.PlayOneShot(marioDeath);

        return marioDeath.length;
    }
    public float WorldClear()
    {
        source.Stop();
        source.PlayOneShot(worldClear);
        return worldClear.length;
    }
}
