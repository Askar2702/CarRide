using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoin : MonoBehaviour
{
    [SerializeField] private int _count;
    [SerializeField] private float _distance;
    [SerializeField] private Coin _coin;
    [SerializeField] private Transform _parent;
    private Vector3 _lastPos;
    private int _currentCoin;
    private float[] _sides;
    private float _side;
    private void Awake()
    {
        _sides = new float[] { 0f, -5f, 5f };
    }
    private void Start()
    {
        _currentCoin = 0;
        _lastPos = transform.position;
    }
    public void Spawn(Vector3 pos)
    {
        var p = new Vector3((pos.x * 1.5f) + _side, (pos.y * 1.3f) + 3f, pos.z * 1.5f);

        if (_currentCoin <= 3)
        {
            var coin = Instantiate(_coin, p, Quaternion.identity);
            _currentCoin++;
            _lastPos = p;
            coin.transform.parent = _parent;
        }
        if (_lastPos.z - p.z >= _distance)
        {
            _currentCoin = 0;
            _side = _sides[Random.Range(0, _sides.Length)];
        }
    }


}
