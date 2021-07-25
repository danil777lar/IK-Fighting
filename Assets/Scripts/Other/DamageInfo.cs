using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public int damage;
    public Vector2 speed;
    public Vector2 direction;
    public IDamageTracker tracker;
    public ContactPoint2D[] contacts;

    public DamageInfo(int damage, Vector2 speed, Vector2 direction, IDamageTracker tracker, ContactPoint2D[] contacts)
    {
        this.damage = damage;
        this.speed = speed;
        this.direction = direction;
        this.tracker = tracker;
        this.contacts = contacts;
    }

    public void Send() 
    {
        tracker.SendDamage(this);
    }
}
