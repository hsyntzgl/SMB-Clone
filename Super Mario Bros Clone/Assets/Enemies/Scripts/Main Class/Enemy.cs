using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float speed;
    protected float direction = -1; // Left Side

    protected Rigidbody2D enemyBody;
    protected CircleCollider2D enemyCollider;

    protected bool isAlive = true;

    protected Animator enemyAnimator;
    protected SpriteRenderer spriteRenderer;

    private bool inCamera = false;


    private GameObject mario;

    private readonly float maxDistance = 30f;


    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CircleCollider2D>();
        enemyAnimator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        mario = GameObject.Find("Mario");
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (Distance() && isAlive)
        {
            transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
        }
    }
    private void OnBecameVisible()
    {
        inCamera = true;
    }
    private void OnBecameInvisible()
    {
        if (inCamera && !Distance()) Destroy(gameObject);
    }
    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Pipe":
            case "Enemy":
            case "Rock":
                direction *= -1;
                spriteRenderer.flipX = !spriteRenderer.flipX;
                break;
            default:
                //Nothing
                break;
        }
    }
    public virtual void Die()
    {
        if (isAlive)
        {
            enemyAnimator.enabled = false;
            enemyCollider.enabled = false;
            spriteRenderer.flipY = true;
            isAlive = false;

            AudioManager.instance.Kick();

            Vector2 direction = Vector2.right + Vector2.up;

            enemyBody.velocity = new Vector2(direction.x, direction.y * 7.5f);
        }

    }
    private bool Distance()
    {
        if (mario != null)
            return (transform.position.x - mario.transform.position.x) <= maxDistance;
        return false;
    }

}
