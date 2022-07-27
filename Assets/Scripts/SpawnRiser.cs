using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRiser : MonoBehaviour
{
    [SerializeField] private Building[] _builds;
    [SerializeField] private int _width;
    [SerializeField] private int _length;
    private float[] _rotY = new float[] { 0, 90, 270, 45, 180 };

    private void Start()
    {
        Spawn();
    }
    private void Spawn()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _length; j++)
            {
                var build = _builds[Random.Range(0, _builds.Length)];
                var Mesh = build.GetComponent<MeshRenderer>();
                var MeshSize = Mesh.bounds.size + new Vector3(Random.Range(20f, 55f), 0, Random.Range(20f, 55f));


                var position = new Vector3(transform.position.x + i * MeshSize.x, transform.position.y, transform.position.z + j * MeshSize.z);
                var riser = Instantiate(build, position, Quaternion.Euler(0, _rotY[Random.Range(0, _rotY.Length)], 0));
                riser.transform.localScale = new Vector3(riser.transform.localScale.x * 2,
                riser.transform.localScale.y * 2, riser.transform.localScale.z * 2);
                riser.name = $"x:{i} z:{j}";
                riser.transform.parent = transform;
            }
        }
    }
}
