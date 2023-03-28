using System.Collections;
using UnityEngine;

public class MarioCodeAnimations : MonoBehaviour
{
    [SerializeField] private Sprite marioDeathSprite;

    [SerializeField] private float invincibleAnimationSpeed;

    private SpriteRenderer spriteRenderer;
    private MarioLevelSystem marioLevelSystem;
    private MarioCollisions marioCollisions;

    private bool countDownOver = true;

    private bool unscaledTimer = false;

    private float countDown = 0f;

    private readonly float invincibleAnimationTime = 10f;
    private readonly float levelUpAnimationTime = 1f;

    private readonly Color normalColor = new Color(1f, 1f, 1f, 1f);
    private readonly Color damagedColor = new Color(1f, 1f, 1f, 0.5f);

    //Layer masks
    private readonly int normalLayer = 8;
    private readonly int damagedLayer = 9;

    private enum AnimationType
    {
        LevelUp,
        Invincible
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        marioLevelSystem = GetComponent<MarioLevelSystem>();
        marioCollisions = GetComponent<MarioCollisions>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (unscaledTimer)
        {
            countDown -= Time.unscaledDeltaTime;
            if (countDown <= 0)
            {
                unscaledTimer = false;
            }
        }
    }
    private IEnumerator AnimateDamage()
    {
        gameObject.layer = damagedLayer;

        StartCoroutine(CountDown(4f));
        StartCoroutine(ResetTimeScaleAfterOneSeconds());

        while (!countDownOver)
        {
            if (spriteRenderer.color == normalColor)
            {
                spriteRenderer.color = damagedColor;
            }
            else
            {
                spriteRenderer.color = normalColor;
            }
            yield return new WaitForSecondsRealtime(.1f);
        }
        spriteRenderer.color = normalColor;
        gameObject.layer = normalLayer;

        marioCollisions.IsDamaged = false;
    }
    private IEnumerator AnimateColors(AnimationType animationType, float time)
    {
        float r = 0f;
        float g = 1f;
        float b = 1f;

        StartCoroutine(CountDown(time));
        


        while (time >= 0 && !countDownOver)
        {
            while (r <= 1 && g >= 0 && !countDownOver)
            {
                r += Time.unscaledDeltaTime * invincibleAnimationSpeed;
                g -= Time.unscaledDeltaTime * invincibleAnimationSpeed;

                spriteRenderer.color = new Color(r, g, b, 1f);

                yield return null;
            }
            r = 1;
            g = 0;
            while (g <= 1 && b >= 0 && !countDownOver)
            {
                g += Time.unscaledDeltaTime * invincibleAnimationSpeed;
                b -= Time.unscaledDeltaTime * invincibleAnimationSpeed;

                spriteRenderer.color = new Color(r, g, b, 1f);

                yield return null;
            }
            g = 1;
            b = 0;

            while (b <= 1 && r >= 0 && !countDownOver)
            {
                b += Time.unscaledDeltaTime * invincibleAnimationSpeed;
                r -= Time.unscaledDeltaTime * invincibleAnimationSpeed;

                spriteRenderer.color = new Color(r, g, b, 1f);

                yield return null;
            }

            b = 1;
            r = 0;
        }
        spriteRenderer.color = normalColor;

        switch (animationType)
        {
            case AnimationType.LevelUp:
                marioLevelSystem.LevelUpAnimationOver();
                break;
            case AnimationType.Invincible:
                GetComponent<MarioCollisions>().IsInvincible = false;
                AudioManager.instance.ContinueLevelMusic();
                break;
        }
    }
    private IEnumerator CountDown(float time)
    {
        countDownOver = false;
        while (time > 0)
        {
            time -= 1;
            yield return new WaitForSecondsRealtime(1);
        }
        countDownOver = true;
    }
    private IEnumerator ResetTimeScaleAfterOneSeconds()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
    }
    private IEnumerator MarioDeath()
    {
        GetComponent<Animator>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<CapsuleCollider2D>().enabled = false;

        Time.timeScale = 0f;

        spriteRenderer.sprite = marioDeathSprite;

        countDown = .5f;
        unscaledTimer = true;

        yield return new WaitForSecondsRealtime(countDown);

        float ellapsed = 0f;
        float duration = .35f;

        Vector2 currentPosition = transform.position;
        Vector2 maximumHigh = new Vector2(transform.position.x, transform.position.y + 3);

        while (ellapsed < duration)
        {
            float t = Mathf.Clamp01(ellapsed / duration);

            transform.position = Vector2.Lerp(currentPosition, maximumHigh, t);

            ellapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        transform.position = maximumHigh;

        currentPosition = transform.position;
        Vector2 underCameraPosition = new Vector2(transform.position.x, Camera.main.transform.position.y - 10);

        ellapsed = 0f;

        while (ellapsed < duration)
        {
            float t = Mathf.Clamp01(ellapsed / duration);

            transform.position = Vector2.Lerp(currentPosition, underCameraPosition, t);

            ellapsed += Time.unscaledDeltaTime;

            yield return null;
        }
        transform.position = underCameraPosition;
    }
    public void MarioDeathAnimation()
    {
        StartCoroutine(MarioDeath());
    }
    public void DamageAnimation()
    {
        StartCoroutine(AnimateDamage());
    }
    public void InvincibleAnimation()
    {
        if(gameObject.layer == damagedLayer) gameObject.layer = normalLayer;

        StartCoroutine(AnimateColors(AnimationType.Invincible, invincibleAnimationTime));
    }
    public void LevelUpToFieryMarioAnimation()
    {
        StartCoroutine(AnimateColors(AnimationType.LevelUp, levelUpAnimationTime));
    }
}