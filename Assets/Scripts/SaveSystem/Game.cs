using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private int _seed;
    private bool _isRandomGeneration;
    private float _giftProgress;
    private Vector2 _roadOffset;
    private bool _isSound;

    public int Seed
    {
        get
        { return _seed; }
        set
        {
            if (_seed != value)
            {
                _seed = value;
                SaveData();
            }
        }
    }

    public bool IsRandomGeneration
    {
        get
        {
            return _isRandomGeneration;
        }
        set
        {
            if (_isRandomGeneration != value)
            {
                _isRandomGeneration = value;
                SaveData();
            }
        }
    }

    public float GiftProgress
    {
        get
        {
            return _giftProgress;
        }
        set
        {
            if (_giftProgress != value)
            {
                _giftProgress = value;
                SaveData();
            }
        }
    }

    public Vector2 RoadOffset => _roadOffset;
    public bool IsSound
    {
        get
        {
            return _isSound;
        }
        set
        {
            if (_isSound != value)
            {
                _isSound = value;
                SaveData();
            }
        }
    }
    private void Awake()
    {
        LoadData();
    }
    private void SaveData()
    {
        SaveSystem.SaveData(this);
    }
    private void LoadData()
    {
        GameData data = SaveSystem.LoadData();
        if (data == null)
        {
            _seed = 0;
            IsRandomGeneration = true;
            return;
        }
        _seed = data._seedRoad;
        _isRandomGeneration = data.isRandom;
        _giftProgress = data.giftProgress;
        _roadOffset = new Vector2(data.X, data.Y);
        _isSound = data.IsSound;
    }

    public void SetRoadOffset(Vector2 offset)
    {
        _roadOffset = offset;
        SaveData();
    }
}
