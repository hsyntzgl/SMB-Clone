using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpBlock : Blocks, IBlock
{
    private GameObject magicMushroomRed;
    private GameObject fireFlower;

    private bool isBlockEmpty = false;

    private readonly float outOfBlockDistance = 1.4f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        magicMushroomRed = Resources.Load<GameObject>("MagicMushroomRed");
        fireFlower = Resources.Load<GameObject>("FireFlower");
    }
    public void MarioHitted()
    {
        if (!isBlockEmpty)
        {
            if (marioLevelSystem.CurrentLevel == 0)
            {
                GameObject temp = Instantiate(magicMushroomRed);
                temp.transform.position = new Vector3(transform.position.x, transform.position.y + .5f, 0);

                StartCoroutine(Animate(temp, NewPosition(), true));
            }
            else
            {
                GameObject temp = Instantiate(fireFlower);
                temp.transform.position = transform.position;

                StartCoroutine(Animate(temp, NewPosition()));
            }
            AudioManager.instance.PowerUpAppears();
            GetComponent<Animator>().enabled = false;
            base.Hitted(transform.position);
            isBlockEmpty = true;
            GetComponent<SpriteRenderer>().sprite = emptyItemBlock;
        }
        else
        {
            AudioManager.instance.BumpBlock();
        }
    }
    private IEnumerator Animate(GameObject levelUpObject, Vector2 to, bool canMove = false)
    {
        float ellapsed = 0f;
        float duration = .5f;

        Vector2 from = levelUpObject.transform.position;

        while (ellapsed < duration)
        {
            float t = (ellapsed / duration);

            levelUpObject.transform.position = Vector2.Lerp(from, to, t);
            ellapsed += Time.deltaTime;

            yield return null;
        }
        levelUpObject.transform.position = to;

        if (canMove)
        {
            levelUpObject.GetComponent<MagicMushrooms>().Move();
        }
    }
    private Vector2 NewPosition()
    {
        return new Vector2(transform.position.x, transform.position.y + outOfBlockDistance);
    }
}
