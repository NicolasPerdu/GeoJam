using UnityEngine;

public class UpdateShape : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    double buffer = 0;
    public double timeMax = 0.15;
    float rangeMin = -4f, rangeMax = 4f;

    float waitTillNextBlurk = .5F;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
    }

    void FixedUpdate()
    {

        if(buffer > waitTillNextBlurk) {
            for (var i = 0; i < vertices.Length; i++) {
                Vector3 vec = new Vector3(Random.Range(rangeMin, rangeMax), Random.Range(rangeMin, rangeMax), Random.Range(rangeMin, rangeMax));
                vertices[i] = vec;
            }

            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            buffer = 0;
            waitTillNextBlurk = Random.Range(.15F, (float)timeMax);
        }
        buffer = buffer + Time.deltaTime;
    }
}
