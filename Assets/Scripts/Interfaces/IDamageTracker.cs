using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageTracker
{
    void SendDamage(DamageInfo damageInfo);
}
