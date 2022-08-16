using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int Seed { get; private set; }
    public bool IsRandomGeneration { get; private set; }
    public float giftProgress { get; private set; }

    private void Awake()
    {
        LoadData();
    }
    public void SaveData(int seed, bool isRandom, float progress)
    {
        Seed = seed;
        IsRandomGeneration = isRandom;
        giftProgress = progress;
        SaveSystem.SaveData(this);
    }
    public void LoadData()
    {
        GameData data = SaveSystem.LoadData();
        if (data == null)
        {
            Seed = 0;
            IsRandomGeneration = true;
            return;
        }
        Seed = data._seedRoad;
        IsRandomGeneration = data.isRandom;
        giftProgress = data.giftProgress;

    }
}
