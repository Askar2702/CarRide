using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PathCreator))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RoadCreator : MonoBehaviour
{
    public int QuantityPerSide => (_quantityPerSide * 2) + 1;
    public int Points;
    [SerializeField] int _quantityPerSide = 2;
    [Range(0.05f, 1.5f)]
    [SerializeField] private float spacing = 1f;
    [SerializeField] private Material _mat;
    public bool autoUpdate;
    private float _tilingY = 1;
    private float _tilingX = 1;
    [Range(0.05f, 1.5f)]
    [SerializeField] private float _widthMultiplier = 1f;
    private GameObject _road;

    private Vector3[] p;
    private void Awake()
    {
        Points = GetComponent<PathCreator>().path.CalculateEvenlySpacedPoints(spacing).Length;
    }
    public void UpdateRoad()
    {
        if (!_road)
        {
            _road = new GameObject();
            _road.AddComponent<MeshFilter>();
            _road.AddComponent<MeshRenderer>();
            _road.GetComponent<MeshRenderer>().sharedMaterial = _mat;
            _road.transform.position = transform.position;
            _road.transform.name = "Road";
        }

        Path path = GetComponent<PathCreator>().path;
        Vector2[] points = path.CalculateEvenlySpacedPoints(spacing);
        p = new Vector3[points.Length];
        _road.GetComponent<MeshFilter>().mesh = CreateRoadMesh(points, path.IsClosed);

        int textureRepeat = Mathf.RoundToInt(_tilingY * points.Length * spacing * .05f);
        float X = _tilingX / (_quantityPerSide * 2);
        _road.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(X, textureRepeat);
    }

    // тут с помощь % points.Length пытаются циклировать оно если меньше чем  points.Length то вернет число которое он делил а если больше то все равно начнет по циклу
    Mesh CreateRoadMesh(Vector2[] points, bool isClosed)
    {
        Vector3[] verts = new Vector3[points.Length * ((_quantityPerSide * 2) + 1)];
        Vector2[] uvs = new Vector2[verts.Length];
        // сюда добавляются еще две точки если она зациклена хвостом и началом 
        // int numTris = 5 * (points.Length - 1) + ((isClosed) ? 2 : 0);
        List<int> tris = new List<int>();
        int vertIndex = 0;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 forward = Vector2.zero;
            // в общем тут не для последнего и не первого делается 
            //чтоб получится усредненый значение которое потом нормализуется дабы получить нправление
            if (i < points.Length - 1 || isClosed)
            {
                forward += points[(i + 1) % points.Length] - points[i];
            }
            if (i > 0 || isClosed)
            {
                forward += points[i] - points[(i - 1 + points.Length) % points.Length];
            }

            forward.Normalize();
            Vector2 left = new Vector2(-forward.y, forward.x);

            SetVertex(verts, left, points, i, _quantityPerSide, vertIndex);

            // дерьмо снизу это uv для материала
            float completionPercent = i / (float)(points.Length - 1);
            float v = 1 - Mathf.Abs(2 * completionPercent - 1);

            SetUv(uvs, vertIndex, v);
            // uvs[vertIndex] = new Vector2(0, v);
            // uvs[vertIndex + 1] = new Vector2(1, v);

            // тут создаются 2 треугольника с одной точки до другой и берут координату они от вершин в verts
            if (i < points.Length - 1 || isClosed)
            {
                SetTriangles(tris, vertIndex, verts);
            }

            vertIndex += (_quantityPerSide * 2) + 1;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }


    private void SetVertex(Vector3[] vert, Vector2 left, Vector2[] points, int current, int count, int vertIndex)
    {
        for (int i = 0; i < points.Length; i++)
            p[i] = new Vector3(points[i].x, transform.position.y, points[i].y);
        //left
        for (int i = 0; i < count; i++)
        {
            vert[vertIndex + i] = p[current] + (new Vector3(left.x, 0, left.y) * _widthMultiplier) * (count - i);
        }

        vert[vertIndex + count] = p[current];
        // center and right
        for (int i = 1; i <= (count); i++)
        {
            vert[vertIndex + count + i] = p[current] - (new Vector3(left.x, 0, left.y) * _widthMultiplier) * i;
        }

    }


    private void SetTriangles(List<int> tris, int vertIndex, Vector3[] verts)
    {
        var sum = (_quantityPerSide * 2) + 1;
        // отнимаю один чтоб не было лишней итераций а то появятся лишиние вертексы
        for (int i = 0; i < (sum - 1); i++)
        {
            tris.Add(vertIndex + i);
            tris.Add((vertIndex + sum + i) % verts.Length);
            tris.Add(vertIndex + i + 1);


            tris.Add(vertIndex + i + 1);
            tris.Add((vertIndex + sum + i) % verts.Length);
            tris.Add((vertIndex + sum + i + 1) % verts.Length);


        }
    }

    private void SetUv(Vector2[] uvs, int vertIndex, float v)
    {
        for (int i = 0; i < _quantityPerSide * 2 + 1; i++)
        {
            uvs[vertIndex + i] = new Vector2(i, v);
        }
    }
}
