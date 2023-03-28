using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleCoinBlock : Blocks, IBlock
{
    private int coinCount = 10;
    private bool isBlockEmpty = false;

    private ParticleSystem coinAnimation, tempAnimation;


    private void PlayCoinAnimation()
    {
        if(tempAnimation == null)
            tempAnimation = Instantiate(coinAnimation);
        
        tempAnimation.gameObject.transform.position = transform.position;
        tempAnimation.Play();
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
            if (coinCount > 0)
            {
                Hitted(transform.position);
                LevelManager.instance.Coin++;
                LevelManager.instance.Score += 200;
                coinCount--;
                AudioManager.instance.BumpBlock();
                AudioManager.instance.Coin();

                PlayCoinAnimation();

                coinAnimation.Play();
                if (coinCount == 0)
                {
                    isBlockEmpty = true;
                    GetComponent<SpriteRenderer>().sprite = emptyItemBlock;
                }
            }
        }
        else
        {
            AudioManager.instance.BumpBlock();
        }
    }

}
