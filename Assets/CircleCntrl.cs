using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class CircleCntrl : NetworkBehaviour
{
    bool left;
    bool right;
    bool down;
    bool up;

    private void Start()
    { 
        if (IsHost) GetComponent<SpriteRenderer>().color = IsLocalPlayer ? Color.red : Color.blue;
        else GetComponent<SpriteRenderer>().color = IsLocalPlayer ? Color.blue : Color.red;
    }

    void Update()
    {
        if (IsLocalPlayer) 
        {
            left = Input.GetKey("a");
            right = Input.GetKey("d");
            up = Input.GetKey("w");
            down = Input.GetKey("s");
        }
    }

    private void FixedUpdate()
    {
        if (left)
            transform.position += Vector3.left / 10f;
        if (right)
            transform.position += Vector3.right / 10f;
        if (down)
            transform.position += Vector3.down / 10f;
        if (up)
            transform.position += Vector3.up / 10f;
    }
}
