using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBlock : Blocks, IBlock
{
    [SerializeField] private GameObject star;
    private bool isBlockEmpty = false;

    private readonly float outOfBlockY = 2.36f;
    public void MarioHitted()
    {
        if (!isBlockEmpty)
        {
            isBlockEmpty = true;
            base.Hitted(transform.position);
            GetComponent<SpriteRenderer>().sprite = emptyItemBlock;

            StartCoroutine(Animate());
        }
    }
    private IEnumerator Animate()
    {
        Vector2 from = star.transform.position;
        Vector2 to = new Vector2(star.transform.position.x, outOfBlockY);

        float ellapsed = 0f;
        float duration = .5f;

        while (ellapsed < duration)
        {
            float t = ellapsed / duration;

            star.transform.position = Vector2.Lerp(from, to,t);

            ellapsed += Time.deltaTime;

            yield return null;
        }

        star.GetComponent<Star>().enabled = true;
    }
}
