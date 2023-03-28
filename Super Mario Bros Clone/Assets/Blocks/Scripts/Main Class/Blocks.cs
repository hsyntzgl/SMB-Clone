using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Blocks : MonoBehaviour
{
    protected Sprite emptyItemBlock;
    protected Vector2 blockPosition;

    protected MarioLevelSystem marioLevelSystem;

    private BoxCollider2D boxCollider2D;
    private LayerMask layerAsLayerMask;

    private readonly int enemyLayer = 10;
    private readonly int levelUpLayer = 16;

    private readonly float animateDistance = 0.5f;

    protected enum BlockType
    {
        SingleCoin,
        MultipleCoin,
        Star,
        LevelUp,
        Empty
    };
    protected virtual void Start()
    {
        emptyItemBlock = Resources.Load<Sprite>("EmptyBlock");

        marioLevelSystem = GameObject.Find("Mario").GetComponent<MarioLevelSystem>();

        boxCollider2D = GetComponent<BoxCollider2D>();

        layerAsLayerMask |= (1 << enemyLayer);
        layerAsLayerMask |= (1 << levelUpLayer);
    }
    protected void Hitted(Vector2 blockPosition)
    {
        BoxCast();
        this.blockPosition = blockPosition;
        StartCoroutine(AnimateBlock());
    }
    protected void BoxCast()
    {

        Vector2 position = new Vector2(transform.position.x, transform.position.y + .25f);

        RaycastHit2D hit = Physics2D.BoxCast(position, boxCollider2D.bounds.size, 0f, Vector2.up, 0f, layerAsLayerMask);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Level Up"))
            {
                Rigidbody2D rb = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, 7.5f);
            }
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<Enemy>().Die();
            }
        }
    }
    private IEnumerator AnimateBlock()
    {
        Vector2 animatedPosition = blockPosition + Vector2.up * animateDistance;

        yield return Move(blockPosition, animatedPosition);
        yield return Move(animatedPosition, blockPosition);
    }
    private IEnumerator Move(Vector2 from, Vector2 to)
    {
        float ellapsed = 0f;
        float duration = 0.125f;

        while (ellapsed < duration)
        {
            float t = ellapsed / duration;

            transform.position = Vector2.Lerp(from, to, t);
            ellapsed += Time.deltaTime;

            yield return null;
        }
        transform.position = to;
    }
}
