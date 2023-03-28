using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGoomba : Enemy, IEnemyDamage
{
    public bool HitByBody() => false;
    public void Samashed()
    {
        enemyBody.constraints = RigidbodyConstraints2D.FreezeAll;
        enemyAnimator.SetTrigger("Smashed");
        isAlive = false;

        enemyBody.constraints = RigidbodyConstraints2D.FreezePositionY;
        enemyCollider.enabled = false;

        AudioManager.instance.Stomp();

        Invoke("DestroyEnemy", 1f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    public override void Die()
    {
        base.Die();
    }
}
