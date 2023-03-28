using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUpBlock : Blocks, IBlock
{
    private GameObject oneUpMushroom;
    private bool isBlockEmpty = false;

    private readonly float outOfBlockDistance = 1.4f;

    protected override void Start()
    {
        base.Start();
        oneUpMushroom = Resources.Load<GameObject>("OneUpMushroom");
    }
    public void MarioHitted()
    {
        if (!isBlockEmpty)
        {
            GameObject temp = Instantiate(oneUpMushroom);
            temp.transform.position = new Vector3(transform.position.x, transform.position.y + .5f, 0);

            StartCoroutine(Animate(temp, NewPosition()));

            base.Hitted(transform.position);
            isBlockEmpty = true;
            GetComponent<SpriteRenderer>().sprite = emptyItemBlock;
            GetComponent<BoxCollider2D>().usedByEffector = false;
            AudioManager.instance.PowerUpAppears();
            StartCoroutine(Animate(oneUpMushroom, NewPosition()));
        }
        else
        {
            AudioManager.instance.BumpBlock();
        }
    }
    private IEnumerator Animate(GameObject oneUpMushroom, Vector2 to)
    {
        float ellapsed = 0f;
        float duration = .5f;

        Vector2 from = oneUpMushroom.transform.position;

        while (ellapsed < duration)
        {
            float t = (ellapsed / duration);

            oneUpMushroom.transform.position = Vector2.Lerp(from, to, t);
            ellapsed += Time.deltaTime;

            yield return null;
        }
        oneUpMushroom.transform.position = to;

        oneUpMushroom.GetComponent<MagicMushrooms>().Move();
    }
    private Vector2 NewPosition()
    {
        return new Vector2(transform.position.x, transform.position.y + outOfBlockDistance);
    }
}