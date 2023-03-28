using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public static int fireBallCount = 0;
    private Rigidbody2D fireBallBody;
    private Animator fireBallAnimator;

    private Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {
        fireBallBody = GetComponent<Rigidbody2D>();
        fireBallAnimator = GetComponent<Animator>();

        fireBallCount++;

        fireBallBody.velocity = velocity;
    }
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        fireBallCount--;
    }
    private void FixedUpdate()
    {
        if (fireBallBody.velocity.y < velocity.y)
        {
            fireBallBody.velocity = velocity;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().Die();
            fireBallAnimator.SetTrigger("Exploded");
        }
        else
        {
            Vector2 normal = other.contacts[0].normal;
            if (normal == Vector2.left || normal == Vector2.right)
            {
                fireBallAnimator.SetTrigger("Exploded");
                fireBallBody.constraints = RigidbodyConstraints2D.FreezeAll;
                
                AudioManager.instance.BumpBlock();
            }
            else
            {
                fireBallBody.velocity = new Vector2(velocity.x, -velocity.y);
            }
        }
    }
    public void ExplodedAnimationOver()
    {
        Destroy(gameObject);
    }
    public void SetVelocity(bool isRight)
    {
        velocity = (isRight) ? new Vector2(17.5f, -7.5f) : new Vector2(-17.5f, -7.5f); 
    }
}