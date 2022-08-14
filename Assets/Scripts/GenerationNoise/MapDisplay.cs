using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class MapDisplay : MonoBehaviour
{

    public Renderer textureRender;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawTexture(Texture2D texture)
    {
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Material mat)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        if (!gameObject.GetComponent<MeshCollider>())
            meshFilter.gameObject.AddComponent<MeshCollider>();
        gameObject.GetComponent<MeshCollider>().sharedMesh = meshFilter.sharedMesh;
        meshRenderer.sharedMaterial = mat;
        transform.localScale = new Vector3(1.5f, 1.3f, 1.5f);
    }

    // public void DrawMesh(MeshData[] meshData, Material mat)
    // {
    //     for (int i = 0; i < meshData.Length; i++)
    //     {
    //         var child = new GameObject();
    //         child.transform.name = $"ChildRoad{i}";
    //         child.transform.parent = transform;

    //         child.AddComponent<MeshFilter>();
    //         child.AddComponent<MeshRenderer>();
    //         child.AddComponent<MeshCollider>();

    //         child.GetComponent<MeshFilter>().sharedMesh = meshData[i].CreateMesh();
    //         child.GetComponent<MeshCollider>().sharedMesh = child.GetComponent<MeshFilter>().sharedMesh;
    //         child.GetComponent<MeshRenderer>().sharedMaterial = mat;


    //     }

    //     transform.localScale = new Vector3(2, 1.3f, 2f);
    // }



}
