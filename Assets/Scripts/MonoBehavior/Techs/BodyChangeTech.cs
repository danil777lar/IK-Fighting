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
        _healthManager.OnPlayerDeath += RunBodyChange;
    }

    private void RunBodyChange(int damage, Vector2 direction) 
    {
        StartCoroutine(BodyChange(damage, direction));
        _healthManager.OnPlayerDeath -= RunBodyChange;
    }

    private IEnumerator BodyChange(int damage, Vector2 direction) 
    {
        _healthManager.SetHeal(damage);

        float distance = 6f;
        Rigidbody2D _body = GetComponent<PointerFiller>().GetPointer(KinematicsPointerType.Body);

        GameObject spawnedObject = Instantiate(_objectToChangePrefab);
        spawnedObject.transform.position = GetComponent<PointerFiller>().GetPointer(KinematicsPointerType.Body).position;

        float lifetime = _particleTrailPrefab.main.duration + _particleTrailPrefab.startLifetime;
        Destroy(Instantiate(_particlesPrefab.gameObject, spawnedObject.transform.position, Quaternion.Euler(Vector3.zero)), _particlesPrefab.main.duration);
        Destroy(Instantiate(_particleTrailPrefab.gameObject, spawnedObject.transform.position, Quaternion.Euler(Vector3.zero), spawnedObject.transform), lifetime);

        RaycastHit2D hit = Physics2D.Raycast(_body.position, direction, distance, LayerMask.GetMask("Ground"));
        if (hit)
            transform.position += (Vector3)direction * Vector2.Distance(_body.transform.position, hit.point);
        else
            transform.position += (Vector3)direction * distance;
        transform.localScale = Vector2.zero;

        yield return new WaitForSeconds(1f);

        transform.localScale = Vector2.one;
        Destroy(Instantiate(_particlesPrefab.gameObject, transform.position, Quaternion.Euler(Vector3.zero)), _particlesPrefab.main.duration);
        Destroy(Instantiate(_particleTrailPrefab.gameObject, transform.position, Quaternion.Euler(Vector3.zero), _body.transform), lifetime);
    }
}
