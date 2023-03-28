using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioFoot : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    private SpriteRenderer parentSpriteRenderer;
 
    private MarioHead marioHead;
    private MarioCollisions marioCollisions;

    private PlayerController playerController;

    private bool isground;

    private readonly float distanceOfCenterY = -0.1f;

    private readonly float footSizeY = 0.3f;


    public bool IsGround
    {
        get => isground;
        set
        {
            isground = value;
            if (value)
            {
                marioHead.HeadHit = false;
            }
        }
    }
    private void Start()
    {
        marioHead = GameObject.Find("Mario Head").gameObject.GetComponent<MarioHead>();

        marioCollisions = transform.GetComponentInParent<MarioCollisions>();

        playerController = transform.GetComponentInParent<PlayerController>();

        parentSpriteRenderer = transform.GetComponentInParent<SpriteRenderer>();
    }
    private void Update()
    {
        BoxCast();
    }
    private void SmashEnemy(IEnemyDamage enemy)
    {
        playerController.JumpWithOutController();
        enemy.Samashed();
    }
    private void BoxCast()
    {
        RaycastHit2D hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y + distanceOfCenterY), GetSize(), 0f, Vector2.down, 0f, groundLayerMask);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Transform enemy = hit.collider.gameObject.transform;
                if (Mathf.InverseLerp(0, 1, (Mathf.Abs(transform.position.x - enemy.position.x))) < .6f && !marioCollisions.IsDamaged)
                {
                    SmashEnemy(hit.collider.GetComponent<IEnemyDamage>());
                }
            }
            else
            {
                IsGround = true;
            }
        }
        else
        {
            IsGround = false;
        }
    }
    private Vector2 GetSize()
    {
        return new Vector2(parentSpriteRenderer.bounds.size.x, footSizeY);
    }
}
