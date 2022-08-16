using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int _seedRoad;
    public bool isRandom;
    public float giftProgress;
    public GameData(Game data)
    {
        _seedRoad = data.Seed;
        isRandom = data.IsRandomGeneration;
        giftProgress = data.giftProgress;
    }


}
