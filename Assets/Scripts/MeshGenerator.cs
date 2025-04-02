using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MeshGenerator
{

    private int vertexNumber;
    private int indexNumber;
    private List<Vector3> vertices;
    private int[] indices;

    public MeshGenerator(int vertexNum)
    {
        vertexNumber = vertexNum;
        indexNumber = vertexNum * 3;
        vertices = new List<Vector3>();
        indices = new int[indexNumber];

    }

    public Mesh generateCircle()
    {
        Mesh circle = new Mesh();
        
        float deltaAngle = 2 * Mathf.PI / vertexNumber;
        float currentAngle = 0f;

        vertices.Add(Vector3.zero);

        //Create a circle with radius 0.8
        for (int i = 1; i < vertexNumber + 2; i++)
        {
            vertices.Add(new Vector3(0.8f * Mathf.Sin(currentAngle), 0.8f * Mathf.Cos(currentAngle), 0f));
            currentAngle += deltaAngle;
            //Debug.Log(vertices[i-1]);

            if (i > 1)
            {
                int j = (i - 2) * 3;
                indices[j + 0] = 0;
                indices[j + 1] = i - 1;
                indices[j + 2] = i;
            }

        }

        circle.SetVertices(vertices);
        circle.SetIndices(indices,MeshTopology.Triangles, 0);
        circle.RecalculateBounds();
        //circle.RecalculateNormals();


        return circle;
    }
    




     

}
