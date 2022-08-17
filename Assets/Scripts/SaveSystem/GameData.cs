using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int _seedRoad;
    public bool isRandom;
    public float giftProgress;
    public float X;
    public float Y;
    public bool IsSound;
    public GameData(Game data)
    {
        _seedRoad = data.Seed;
        isRandom = data.IsRandomGeneration;
        giftProgress = data.GiftProgress;
        X = data.RoadOffset[0];
        Y = data.RoadOffset[1];
        IsSound = data.IsSound;
    }


}
