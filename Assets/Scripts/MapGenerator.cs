using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private MeshFilter _mesh;
    [SerializeField] private RoadCreator _creator;
    [SerializeField] private bool _isRandom;
    [Range(0, 6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    [field: SerializeField] public int Seed { get; private set; }
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;
    private void Start()
    {
        // SetGeneration();
        // RandomGenerated();
        // GenerateMap();
    }
    public void GenerateMap()
    {

        _creator.Points = _mesh.sharedMesh.vertices.Length / _creator.QuantityPerSide;
        float[,] noiseMap = Noise.GenerateNoiseMap(_creator.QuantityPerSide, _creator.Points, Seed, noiseScale, octaves, persistance, lacunarity, offset);


        RoadNoise.GenerateTerrainMesh(_mesh.sharedMesh, noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail);
        _mesh.gameObject.AddComponent<MeshCollider>();

    }


    // юнитевская фукция для иеспектора
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
            offset = new Vector2(Random.Range(0, 10000), (Random.Range(0, 10000)));
        }
    }

    private void SetGeneration()
    {
        var load = GameManager.instance.LoadData();
        _isRandom = load.Item1;
        Seed = load.Item2;
    }
}

