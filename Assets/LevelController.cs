using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private static LevelController _default;
    public static LevelController Default => _default;

    [SerializeField] private Transform _hostSpawn;
    [SerializeField] private Transform _clientSpawn;

    public Vector2 HostSpawnPosition => _hostSpawn.transform.position;
    public Vector2 ClientSpawnPosition => _clientSpawn.transform.position;

    private void Awake()
    {
        _default = this;
    }
}
