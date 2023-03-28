using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioHead : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    private SpriteRenderer parentSpriteRenderer;

    private MarioLevelSystem marioLevelSystem;

    private bool headHit;

    private readonly float smallMarioCenterY = 0.7f;
    private readonly float smallMarioSizeY = -1f;

    private readonly float bigMarioCenterY = 1.3f;
    private readonly float bigMarioSizeY = -2.15f;


    public bool HeadHit
    {
        get => headHit;
        set => headHit = value;
    }
    private void Start()
    {
        parentSpriteRenderer = transform.GetComponentInParent<SpriteRenderer>();

        marioLevelSystem = transform.GetComponentInParent<MarioLevelSystem>();
    }

    private void Update()
    {
        CheckMarioHead();
    }
    private void CheckMarioHead()
    {
        RaycastHit2D hit = Physics2D.BoxCast(GetPosition(), GetSize(), 0f, Vector2.up, 0f, layerMask);

        if (hit.collider != null && !headHit)
        {
            Transform block = hit.collider.gameObject.transform;

            if (Mathf.InverseLerp(0, 1, (Mathf.Abs(transform.position.x - block.position.x))) < 1)
            {
                hit.collider.GetComponentInParent<IBlock>().MarioHitted();
                headHit = true;
            }
        }
    }
    private Vector2 GetSize()
    {
        if (marioLevelSystem.CurrentLevel == 0)
        {
            return new Vector2(parentSpriteRenderer.bounds.size.x, parentSpriteRenderer.bounds.size.y + smallMarioSizeY);
        }
        return new Vector2(parentSpriteRenderer.bounds.size.x, parentSpriteRenderer.bounds.size.y + bigMarioSizeY);
    }
    private Vector2 GetPosition()
    {
        if (marioLevelSystem.CurrentLevel == 0)
        {
            return new Vector2(parentSpriteRenderer.bounds.center.x, parentSpriteRenderer.bounds.center.y + smallMarioCenterY);
        }
        return new Vector2(parentSpriteRenderer.bounds.center.x, parentSpriteRenderer.bounds.center.y + bigMarioCenterY);
    }
}
