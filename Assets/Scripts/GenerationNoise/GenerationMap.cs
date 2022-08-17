using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationMap : MonoBehaviour
{
    [SerializeField] private bool _isRandom;
    [SerializeField] private Material _mat;
    public enum DrawMode { NoiseMap, ColourMap, Mesh };
    public DrawMode drawMode;
    [SerializeField] private int _xSize;
    [SerializeField] private int _zSize;
    [SerializeField] private float _sizeMeshGrid;
    public const int mapChunkSize = 241;
    [Range(0, 6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    [field: SerializeField] public int Seed { get; private set; }
    [field: SerializeField] public Vector2 Offset { get; private set; }

    public float meshHeightMultiplier;
    [SerializeField] private AnimationCurve _meshHeightCurve;
    [SerializeField] private AnimationCurve _curveRoad;
    [SerializeField] private AnimationCurve _curveHeightRoad;
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
        float[,] noiseMap = Noise.GenerateNoiseMap(_xSize, _zSize, Seed, noiseScale, octaves, persistance, lacunarity, Offset);

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
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            var y = Mathf.RoundToInt(1 * _zSize * 1 * .05f);
            _mat.mainTextureScale = new Vector2(_mat.mainTextureScale.x, y);
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(transform.position, noiseMap, _sizeMeshGrid, meshHeightMultiplier, _meshHeightCurve, _curveRoad, _curveHeightRoad, levelOfDetail, _spawnCoin), _mat);
            //            SliceManager.instance.StartCut();
        }
    }

    void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
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
