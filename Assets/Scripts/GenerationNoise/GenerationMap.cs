using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationMap : MonoBehaviour
{
    [SerializeField] private bool _isRandom;
    [SerializeField] private Material _mat;
    public enum DrawMode { NoiseMap, ColourMap, Mesh };
    [SerializeField] private DrawMode _drawMode;
    [SerializeField] private int _xSize;
    [SerializeField] private int _zSize;
    [SerializeField] private float _sizeMeshGrid;
    public const int mapChunkSize = 241;
    [Range(0, 6)]
    [SerializeField] private int _levelOfDetail;
    [SerializeField] private float _noiseScale;

    [SerializeField] private int _octaves;
    [Range(0, 1)]
    [SerializeField] private float _persistance;
    [SerializeField] private float _lacunarity;

    [field: SerializeField] public int Seed { get; private set; }
    [field: SerializeField] public Vector2 Offset { get; private set; }

    [SerializeField] private float _meshHeightMultiplier;
    [SerializeField] private RoadCurveSetting _roadCurveSetting;
    [SerializeField] private AnimationCurve _meshHeightCurve;
    private SpawnCoin _spawnCoin;

    public bool autoUpdate;

    public TerrainType[] regions;
    private void Start()
    {
        _spawnCoin = GetComponent<SpawnCoin>();
        SetGeneration();
        RandomGenerated();
        GenerateMap();
    }
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(_xSize, _zSize, Seed, _noiseScale, _octaves, _persistance, _lacunarity, Offset);

        Color[] colourMap = new Color[_xSize * _zSize];
        for (int y = 0; y < _zSize; y++)
        {
            for (int x = 0; x < _xSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * _xSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (_drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (_drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
        else if (_drawMode == DrawMode.Mesh)
        {
            var y = Mathf.RoundToInt(1 * _zSize * 1 * .05f);
            _mat.mainTextureScale = new Vector2(_mat.mainTextureScale.x, y);
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(transform.position, noiseMap, _sizeMeshGrid,
             _meshHeightMultiplier, _meshHeightCurve, _roadCurveSetting.RoadSettings[Game.instance.IndexRoad].XCurveRoad,
             _roadCurveSetting.RoadSettings[Game.instance.IndexRoad].YCurveRoad,
             _levelOfDetail, _spawnCoin), _mat);
        }
    }

    void OnValidate()
    {
        if (_lacunarity < 1)
        {
            _lacunarity = 1;
        }
        if (_octaves < 0)
        {
            _octaves = 0;
        }
    }

    private void RandomGenerated()
    {
        if (_isRandom)
        {
            Seed = Random.Range(0, 10000);
            Offset = new Vector2(Random.Range(0, 10000), (Random.Range(0, 10000)));
        }
    }

    private void SetGeneration()
    {
        var load = GameManager.instance.LoadData();
        _isRandom = load.Item1;
        Seed = load.Item2;
        Offset = load.Item3;
    }


}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}
