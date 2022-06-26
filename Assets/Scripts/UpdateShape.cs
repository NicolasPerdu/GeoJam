using UnityEngine;

public class UpdateShape : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    double buffer = 0, timeMax = 0.15;
    float rangeMin = -4f, rangeMax = 4f;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
    }

    void Update()
    {
        if(buffer > timeMax) {
            for (var i = 0; i < vertices.Length; i++) {
                Vector3 vec = new Vector3(Random.Range(rangeMin, rangeMax), Random.Range(rangeMin, rangeMax), Random.Range(rangeMin, rangeMax));
                vertices[i] = vec;
            }

            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            buffer = 0;
        }
        buffer = buffer + Time.deltaTime;
    }
}
