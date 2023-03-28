using UnityEngine;

public class MarioCollisions : MonoBehaviour
{
    private bool isInvincible = false;
    private bool isDamaged = false;

    private MarioCodeAnimations marioCodeAnimations;
    private MarioLevelSystem marioLevelSystem;

    private PlayerController playerController;

    public bool IsInvincible
    {
        get => isInvincible;
        set => isInvincible = value;
    }
    public bool IsDamaged
    {
        get => isDamaged;
        set => isDamaged = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        marioCodeAnimations = GetComponent<MarioCodeAnimations>();
        marioLevelSystem = GetComponent<MarioLevelSystem>();

        playerController = GetComponent<PlayerController>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Star"))
        {
            isInvincible = true;
            marioCodeAnimations.InvincibleAnimation();
            AudioManager.instance.PlayStarMusic();
            other.gameObject.SetActive(false);
            LevelManager.instance.Score += 1000;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            Vector2 normal = other.contacts[0].normal;

            if (Mathf.Abs(normal.x) > .6f)
            {
                if (isInvincible)
                {
                    other.gameObject.GetComponent<Enemy>().Die();
                }
                else
                {
                    if (other.gameObject.GetComponent<Enemy>() as KoopaParatroopa)
                    {
                        if (other.gameObject.GetComponent<IEnemyDamage>().HitByBody())
                        {
                            isDamaged = true;
                            marioLevelSystem.LevelDown();
                        }
                    }
                    else
                    {
                        isDamaged = true;
                        marioLevelSystem.LevelDown();
                    }
                }
            }
        }
        if (other.gameObject.CompareTag("Level Up"))
        {
            LevelManager.instance.Score += 1000;
            other.gameObject.SetActive(false);
            marioLevelSystem.LevelUp();
            AudioManager.instance.PowerUp();
        }
        if (other.gameObject.CompareTag("One Up"))
        {
            LevelManager.marioLifeCount++;
            other.gameObject.SetActive(false);
            AudioManager.instance.OneUp();
        }
        else
        {
            CheckContactsLeftOrRight(other);
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        string tag = other.gameObject.tag;

        if (tag != "Enemy" && tag != "Level Up" && tag != "One Up" && tag != "Star")
        {
            CheckContactsLeftOrRight(other);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        playerController.MarioCanGoRight = true;
        playerController.MarioCanGoLeft = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            LevelManager.instance.Coin++;
            LevelManager.instance.Score += 200;
            AudioManager.instance.Coin();
        }
        if (other.gameObject.CompareTag("Pole"))
        {
            playerController.DeactiveAllComponents();
            playerController.LookRight();
            LevelManager.instance.StartPoleProcess();
        }
        if (other.gameObject.CompareTag("Castle"))
        {
            gameObject.SetActive(false);
        }
    }
    private void CheckContactsLeftOrRight(Collision2D other)
    {
        Vector2 normal = other.contacts[0].normal;

        if (normal == Vector2.left)
        {
            playerController.MarioCanGoRight = false;
        }
        else
        {
            playerController.MarioCanGoRight = true;
        }
        if (normal == Vector2.right)
        {
            playerController.MarioCanGoLeft = false;
        }
        else
        {
            playerController.MarioCanGoLeft = true;
        }
    }
}
