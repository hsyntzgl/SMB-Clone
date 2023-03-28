using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaParatroopa : Enemy, IEnemyDamage
{
    [SerializeField] private Sprite deathSprite;

    private bool isHiding = false;
    private bool smashedWhenIsHiding = false;

    private Coroutine hidingCoroutine;

    private readonly Vector2 normalOffset = new Vector2(0, -0.04f);
    private readonly Vector2 hidingOffset = new Vector2(0, 0);

    public bool SmashedWhenIsHiding { get => smashedWhenIsHiding; }

    public bool HitByBody() => IsCanDamage();

    public void Samashed()
    {
        if (!isHiding)
        {
            isHiding = true;
            AudioManager.instance.Stomp();
            hidingCoroutine = StartCoroutine(KoopaBeganHiding());
        }
        else if (!smashedWhenIsHiding)
        {
            SmashedWhenHiding();
        }
    }
    // Update is called once per frame
    private void SmashedWhenHiding()
    {
        StopCoroutine(hidingCoroutine);
        AudioManager.instance.Kick();
        enemyAnimator.SetBool("Hiding", true);
        speed *= 7.5f;
        if (GameObject.Find("Mario").transform.position.x < transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
        smashedWhenIsHiding = true;
    }

    protected override void FixedUpdate()
    {
        if (!isHiding || smashedWhenIsHiding)
        {
            base.FixedUpdate();
        }
    }
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && smashedWhenIsHiding)
        {
            other.gameObject.GetComponent<Enemy>().Die();
        }
        else
        {
            base.OnCollisionEnter2D(other);
        }
    }
    private IEnumerator KoopaBeganHiding()
    {
        enemyCollider.offset = hidingOffset;
        enemyAnimator.SetBool("Hiding", true);

        yield return new WaitForSeconds(3f);

        enemyAnimator.SetBool("Hiding Time Over", true);
    }
    public void AnimationOver()
    {
        enemyCollider.offset = normalOffset;
        enemyAnimator.SetBool("Hiding", false);
        isHiding = false;
    }
    public override void Die()
    {
        spriteRenderer.sprite = deathSprite;
        base.Die();
    }
    public bool IsCanDamage() => ((isHiding && smashedWhenIsHiding) || (!isHiding));
}