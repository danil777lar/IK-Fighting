using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageTracker
{
    void SendDamage(int damage, Vector2 direction);
}
