using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LarjeEnum;
using DG.Tweening;

public class BodyChangeTech : MonoBehaviour
{
    [SerializeField] private GameObject _objectToChangePrefab;
    [SerializeField] private ParticleSystem _particlesPrefab;
    [SerializeField] private ParticleSystem _particleTrailPrefab;

    private HealthManager _healthManager;

    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _healthManager.OnPlayerDeath += BodyChange;
    }

    private void BodyChange(int damage, Vector2 direction) 
    {
        _healthManager.SetHeal(damage);

        float distance = 6f;
        Rigidbody2D _body = GetComponent<PointerFiller>().GetPointer(KinematicsPointerType.Body);

        GameObject spawnedObject = Instantiate(_objectToChangePrefab);
        spawnedObject.transform.position = GetComponent<PointerFiller>().GetPointer(KinematicsPointerType.Body).position;

        RaycastHit2D hit = Physics2D.Raycast(_body.position, direction, distance, LayerMask.GetMask("Ground"));
        if (hit)
            transform.position += (Vector3)direction * Vector2.Distance(_body.transform.position, hit.point);
        else
            transform.position += (Vector3)direction * distance;

        Destroy(Instantiate(_particlesPrefab.gameObject, spawnedObject.transform.position, Quaternion.Euler(Vector3.zero)), _particlesPrefab.main.duration);
        Destroy(Instantiate(_particlesPrefab.gameObject, transform.position, Quaternion.Euler(Vector3.zero)), _particlesPrefab.main.duration);

        float lifetime = _particleTrailPrefab.main.duration + _particleTrailPrefab.startLifetime;
        Destroy(Instantiate(_particleTrailPrefab.gameObject, spawnedObject.transform.position, Quaternion.Euler(Vector3.zero), spawnedObject.transform), lifetime);
        Destroy(Instantiate(_particleTrailPrefab.gameObject, transform.position, Quaternion.Euler(Vector3.zero), _body.transform), lifetime);

        _healthManager.OnPlayerDeath -= BodyChange;
    }
}
