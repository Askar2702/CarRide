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
        transform.localScale = new Vector3(2f, 1.3f, 2f);
    }

}
