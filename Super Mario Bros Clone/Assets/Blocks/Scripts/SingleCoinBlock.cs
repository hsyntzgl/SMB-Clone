using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCoinBlock : Blocks, IBlock
{
    private bool isBlockEmpty = false;

    private ParticleSystem coinAnimation;

    private void PlayCoinAnimation()
    {
        ParticleSystem ps = Instantiate(coinAnimation);
        ps.gameObject.transform.position = transform.position;
        ps.Play();
    }

    protected override void Start()
    {
        coinAnimation = Resources.Load<ParticleSystem>("Block Coin Animation");
        base.Start();
    }
    public void MarioHitted()
    {
        if (!isBlockEmpty)
        {
            isBlockEmpty = true;

            GetComponent<Animator>().enabled = false;

            base.Hitted(transform.position);

            LevelManager.instance.Coin++;
            AudioManager.instance.Coin();

            PlayCoinAnimation();
            GetComponent<SpriteRenderer>().sprite = emptyItemBlock;
            LevelManager.instance.Score += 200;
        }
        else
        {
            AudioManager.instance.BumpBlock();
        }
    }
}
