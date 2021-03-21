using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : Weapon
{
    public override void MotionFinished()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == _targetTag)
        {
            Debug.Log("bam");
            Destroy(gameObject);
        }
    }
}
