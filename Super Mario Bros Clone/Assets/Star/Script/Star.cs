using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private Rigidbody2D starBody;

    private void Awake()
    {
        starBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        starBody.isKinematic = false;
        GetComponent<BoxCollider2D>().enabled = true;

        starBody.AddForce(Vector2.right * 5f, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        if (starBody.velocity.y > 7.5f)
        {
            starBody.velocity = new Vector2(starBody.velocity.x, 7.5f);
        }
        if (starBody.velocity.y < -7.5f)
        {
            starBody.velocity = new Vector2(starBody.velocity.x, -7.5f);
        }
    }
}
