using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoin : MonoBehaviour
{
    public int CountCoins { get; private set; }
    [SerializeField] private float _distance;
    [SerializeField] private Score[] _coins;
    [SerializeField] private Transform _parent;
    private Vector3 _lastPos;
    private int _currentCoin;
    private float[] _sides;
    private float _side;
    private float _chanse = 95f;
    private void Awake()
    {
        _sides = new float[] { 0f, -3f, 3f };
        _currentCoin = 0;
        CountCoins = 0;
        _lastPos = transform.position;
    }

    public void Spawn(Vector3 pos)
    {
        var p = new Vector3((pos.x * 1f) + _side, (pos.y * 1.3f) + 3f, pos.z * 1.5f);
        int i = 0;
        if (Random.Range(0, 100) > _chanse) i = 1;
        if (_currentCoin <= 3)
        {
            var coin = Instantiate(_coins[i], p, Quaternion.identity);
            _currentCoin++;
            if (i == 0)
                CountCoins++;
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
