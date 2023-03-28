using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public bool canMove = true;

    [SerializeField] private Transform mario;

    private readonly float cameraYPosition = 4f;

    private readonly float offset = 5f;
    
    // Update is called once per frame
    void LateUpdate()
    {
        SetPosition();
    }
    private void SetPosition()
    {
        if (!(transform.position.x - mario.position.x > offset) && canMove)
        {
            transform.position = new Vector3(mario.position.x + offset, cameraYPosition, transform.position.z);
        }
    }
    public void SetCameraPositionOneFrame()
    {
         transform.position = new Vector3(mario.position.x + offset, cameraYPosition, transform.position.z);
    }
}
