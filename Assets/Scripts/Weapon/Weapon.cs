using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapon : MonoBehaviour
{
    protected string _targetTag;
    protected float _attackProgress;

    // VIRTUAL
    public virtual void Init(Transform hand, float attackProgress)
    {
        transform.parent = hand;
        transform.localPosition = new Vector2(0f, 0f);

        _targetTag = hand.tag == "Player" ? "Enemy" : "Player";
        _attackProgress = attackProgress > 1f ? 1f : attackProgress;
    }

    // ABSTRACT
    public abstract void MotionFinished();
}
