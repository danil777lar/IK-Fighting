using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetWeapon);
    }

    private void SetWeapon() 
    {
        Player.Default.FightController.SetupWeapon(_weapon);
    }
}
