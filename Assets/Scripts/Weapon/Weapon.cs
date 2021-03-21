using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapon : MonoBehaviour
{
    protected string _targetTag;

    // VIRTUAL
    public virtual void Init(Transform hand)
    {
        transform.parent = hand;
        transform.localPosition = new Vector2(0f, 0f);

        _targetTag = hand.tag == "Player" ? "Enemy" : "Player";
    }

    // ABSTRACT
    public abstract void MotionFinished();
}
