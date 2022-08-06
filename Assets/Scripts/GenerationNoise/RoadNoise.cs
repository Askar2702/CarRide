using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoadNoise
{

    public static void GenerateTerrainMesh(Mesh mesh, float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
    {
        Vector3[] v = new Vector3[mesh.vertices.Length];
        for (var i = 0; i < v.Length; i++)
        {
            v[i] = mesh.vertices[i];
        }
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
        int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;
        int horizontalPerlinLine = (height - 1) / meshSimplificationIncrement + 1;

        int vertIndex = 0;

        for (int y = 0; y < height; y += meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x += meshSimplificationIncrement)
            {
                v[vertIndex] = new Vector3(v[vertIndex].x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, v[vertIndex].z);

                vertIndex++;
            }
        }

        mesh.vertices = v;
        mesh.RecalculateNormals();

    }
}
