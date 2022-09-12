using UnityEngine;
using System.Collections;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(Vector3 point, float[,] heightMap, float size, float heightMultiplier, AnimationCurve heightCurve, AnimationCurve curveRoad, AnimationCurve curveHeightRoad, int levelOfDetail, SpawnCoin spawn)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
        int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;
        int horizontalPerlinLine = (height - 1) / meshSimplificationIncrement + 1;

        MeshData meshData = new MeshData(verticesPerLine, horizontalPerlinLine);
        int vertexIndex = 0;
        var posZ = point.z + (height / size);

        float zPoint = 0;
        for (int y = 0; y < height; y += meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x += meshSimplificationIncrement)
            {
                var pos = new Vector3(point.x + (x / size + curveRoad.Evaluate(y)),
                   ((curveHeightRoad.Evaluate(y) + heightCurve.Evaluate(heightMap[x, y])) * heightMultiplier), posZ - (y / size));

                meshData.vertices[vertexIndex] = pos;
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }
                if ((y - zPoint) >= 1f && (width / 2 == x) && y > 5)
                {
                    zPoint = y;
                    spawn.Spawn(pos);
                }

                vertexIndex++;
            }

        }

        return meshData;

    }

    // тут по частям спавнять
    // public static MeshData[] GenerateTerrainMesh(Vector3 point, float[,] heightMap, int size, float heightMultiplier, AnimationCurve heightCurve, AnimationCurve curveRoad, AnimationCurve curveHeightRoad, int levelOfDetail, SpawnCoin spawn)
    // {
    //     int width = heightMap.GetLength(0);
    //     int height = heightMap.GetLength(1);

    //     int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
    //     int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;
    //     int horizontalPerlinLine = (height - 1) / meshSimplificationIncrement + 1;

    //     int countChild = 10;
    //     int index = 0;
    //     MeshData[] meshData = new MeshData[countChild];
    //     for (int i = 0; i < meshData.Length; i++)
    //         meshData[i] = new MeshData(verticesPerLine, horizontalPerlinLine / countChild);
    //     int vertexIndex = 0;
    //     var posZ = point.z + (height / size);

    //     float zPoint = 0;

    //     for (int y = 0; y < height; y += meshSimplificationIncrement)
    //     {

    //         for (int x = 0; x < width; x += meshSimplificationIncrement)
    //         {
    //             var pos = new Vector3(point.x + (x / size + curveRoad.Evaluate(y)),
    //                ((curveHeightRoad.Evaluate(y) + heightCurve.Evaluate(heightMap[x, y])) * heightMultiplier), posZ - (y / size));
    //             //   Debug.Log($"{index}:{width}:{x += meshSimplificationIncrement}");

    //             meshData[index].vertices[vertexIndex] = pos;
    //             meshData[index].uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

    //             if (x < width - 1 && y < ((height / 10) * (index + 1)) - 1)
    //             {
    //                 meshData[index].AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
    //                 meshData[index].AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
    //             }
    //             if ((y - zPoint) >= 1f && (width / 2 == x))
    //             {
    //                 zPoint = y;
    //                 spawn.Spawn(pos);
    //             }
    //             vertexIndex++;
    //             if (vertexIndex >= (verticesPerLine * (horizontalPerlinLine / countChild)))
    //                 vertexIndex = 0;

    //         }
    //         if (y == ((height / 10) * (index + 1)))
    //         {
    //             index++;
    //             if (index >= 10) index = 9;
    //         }
    //     }

    //     return meshData;

    // }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }

}