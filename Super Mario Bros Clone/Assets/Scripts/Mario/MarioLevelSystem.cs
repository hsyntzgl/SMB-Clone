using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioLevelSystem : MonoBehaviour
{
    private int currentLevel;
    private float animationTime;
    private bool timerStart = false;
    private bool isUp;

    private Animator animatorController;
    private CapsuleCollider2D parentCapsuleCollider2D;
    private SpriteRenderer spriteRenderer;

    private MarioCodeAnimations marioCodeAnimations;

    private MarioHead marioHead;
    private MarioFoot marioFoot;

    private readonly float ignoreValue = 0.0f;
    private readonly float defaultAnimationTime = 1f;

    public int CurrentLevel { get => currentLevel;}

    // Start is called before the first frame update
    void Start()
    {
        animatorController = GetComponent<Animator>();
        parentCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        marioCodeAnimations = GetComponent<MarioCodeAnimations>();

        marioHead = GetComponentInChildren<MarioHead>();
        marioFoot = GetComponentInChildren<MarioFoot>();

        animationTime = defaultAnimationTime;
    }

    // Update is called once per frame
    void Update()
    {
        CheckColliderSize();

        if (timerStart)
        {
            animationTime -= Time.unscaledDeltaTime;
            if (animationTime <= 0)
            {
                SetLayer();
                timerStart = false;
                animationTime = defaultAnimationTime;
                Time.timeScale = 1f;
            }
        }
    }
    private void CheckColliderSize()
    {
        if (GetSpriteSize().x != parentCapsuleCollider2D.size.x || GetSpriteSize().y != parentCapsuleCollider2D.size.y)
        {
            Vector2 newColliderSize = GetSpriteSize();
            parentCapsuleCollider2D.size = newColliderSize;
            parentCapsuleCollider2D.offset = new Vector2(0, newColliderSize.y / 2);
        }
    }
    private Vector2 GetSpriteSize()
    {
        return new Vector2((spriteRenderer.bounds.size.x / 10) - ignoreValue, spriteRenderer.bounds.size.y / 10);
    }
    private void SetLayer()
    {
        animatorController.Rebind();
        if (isUp)
        {
            animatorController.SetLayerWeight(++currentLevel, 1f);
        }
        else
        {
            animatorController.SetLayerWeight(currentLevel--, 0f);
        }
    }
    public void LevelUp()
    {
        isUp = true;
        switch (currentLevel)
        {
            case 0:
                animatorController.SetTrigger("Level Up");
                timerStart = true;
                Time.timeScale = 0f;
                break;
            case 1:
                currentLevel++;
                Time.timeScale = 0f;
                animatorController.updateMode = AnimatorUpdateMode.Normal;
                marioCodeAnimations.LevelUpToFieryMarioAnimation();
                break;
            case 2:
                LevelManager.instance.Score += 1000;
                break;
        }
    }
    public void LevelDown()
    {
        isUp = false;
        switch (currentLevel)
        {
            case 0:
                GetComponent<PlayerController>().enabled = false;

                marioHead.enabled = false;
                marioFoot.enabled = false;

                marioCodeAnimations.MarioDeathAnimation();
                LevelManager.instance.MarioDead();
                break;
            case 1:
                animatorController.updateMode = AnimatorUpdateMode.UnscaledTime;
                marioCodeAnimations.DamageAnimation();
                animatorController.SetTrigger("Level Down");
                timerStart = true;
                Time.timeScale = 0f;

                AudioManager.instance.LevelDown();
                break;
            case 2:
                marioCodeAnimations.DamageAnimation();
                Time.timeScale = 0f;
                animatorController.SetLayerWeight(currentLevel--, 0f);

                AudioManager.instance.LevelDown();
                break;
        }
    }
    public void LevelUpAnimationOver()
    {
        animatorController.SetLayerWeight(currentLevel, 1f);
        Time.timeScale = 1f;
    }
}
