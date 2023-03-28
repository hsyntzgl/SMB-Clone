using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBlock : Blocks, IBlock
{
    private ParticleSystem ps;

    protected override void Start()
    {
        ps = Resources.Load<ParticleSystem>("Breaked Block Particle System");
        base.Start();
    }
    public void MarioHitted()
    {
        if (marioLevelSystem.CurrentLevel == 0)
        {
            AudioManager.instance.BumpBlock();
            base.Hitted(transform.position);
        }
        else
        {
            var temp = Instantiate(ps);
            temp.gameObject.transform.position = transform.position;
            temp.Play();

            AudioManager.instance.BreakBlock();
            BoxCast();

            gameObject.SetActive(false);
        }
    }
}
