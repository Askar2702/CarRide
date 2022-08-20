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
    public int CountOpenCar;
    public int CountGem;
    public List<int> ShopItems;
    public int CurrentCar;
    public int Level;
    public int IndexRoad;
    public GameData(Game data)
    {
        _seedRoad = data.Seed;
        isRandom = data.IsRandomGeneration;
        giftProgress = data.GiftProgress;
        X = data.RoadOffset[0];
        Y = data.RoadOffset[1];
        IsSound = data.IsSound;
        CountOpenCar = data.CountOpenCar;
        CountGem = data.CountGem;
        ShopItems = data.ShopItems;
        CurrentCar = data.CurrentCar;
        Level = data.Level;
        IndexRoad = data.IndexRoad;
    }


}
