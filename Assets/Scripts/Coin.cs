using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    private float _currentPosY;
    private float _posY;
    private float _maxPosY = 2;
    private float _minPosY = -2;
    private float _time;
    private void Start()
    {
        StartCoroutine(StartRotate());
        _posY = transform.position.y;
        _maxPosY = transform.position.y + 2;
        _minPosY = transform.position.y - 2;
        _currentPosY = _maxPosY;
    }
    private void Update()
    {
        if (transform.position.y >= _maxPosY)
        {
            _currentPosY = -2;
        }
        else if (transform.position.y <= _minPosY)
        {
            _currentPosY = 2;
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(0, _currentPosY * Time.deltaTime, 0, Space.World);
    }
    private IEnumerator StartRotate()
    {
        while (true)
        {
            yield return null;
            transform.Rotate(Vector3.forward, 2f);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.activeSelf) return;
        if (other.transform.root.TryGetComponent(out CarController car))
        {
            UiManager.instance.CoinAdd();
            gameObject.SetActive(false);
        }
    }

}
