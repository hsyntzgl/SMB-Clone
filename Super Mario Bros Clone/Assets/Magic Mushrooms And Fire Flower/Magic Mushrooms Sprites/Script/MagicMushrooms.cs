using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushrooms : MonoBehaviour
{
    [SerializeField] private float speed;

    private int direction = 1;

    private bool canMove = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Pipe")) direction *= -1;
    }
    public void Move()
    {
        canMove = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
    }
}
