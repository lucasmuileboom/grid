using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshUtils : MonoBehaviour
{
    public static Mesh CreateEmptyQuadMesh(int quadCount)
    {
        Vector3[] vertices = new Vector3[4 * quadCount];
        Vector2[] uvs = new Vector2[4 * quadCount];
        int[] triangles = new int[6 * quadCount];

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        return mesh;
    }

    public static Mesh AddQuadToMesh(Mesh mesh, int index, Vector3 position, Vector3 quadSize, Vector2 uvStart, Vector2 uvEnd) //beter naam verzinnen voor uv vectors
    {       
        int vIndex0 = index * 4;
        int vIndex1 = vIndex0 + 1;
        int vIndex2 = vIndex0 + 2;
        int vIndex3 = vIndex0 + 3;

        Vector3 halfQuadSize = quadSize * 0.5f;

        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = mesh.uv;
        int[] triangles = mesh.triangles;

        vertices[vIndex0] = position + new Vector3(-halfQuadSize.x, -halfQuadSize.y);
        vertices[vIndex1] = position + new Vector3(-halfQuadSize.x, halfQuadSize.y);
        vertices[vIndex2] = position + new Vector3(halfQuadSize.x, halfQuadSize.y);
        vertices[vIndex3] = position + new Vector3(halfQuadSize.x, -halfQuadSize.y);
      
        uvs[vIndex0] = new Vector2(uvStart.x, uvStart.y);
        uvs[vIndex1] = new Vector2(uvStart.x, uvEnd.y);
        uvs[vIndex2] = new Vector2(uvEnd.x, uvEnd.y);
        uvs[vIndex3] = new Vector2(uvEnd.x, uvStart.y);

        int tIndex = index * 6;

        triangles[tIndex + 0] = vIndex0;
        triangles[tIndex + 1] = vIndex1;
        triangles[tIndex + 2] = vIndex2;
        triangles[tIndex + 3] = vIndex0;
        triangles[tIndex + 4] = vIndex2;
        triangles[tIndex + 5] = vIndex3;

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        return mesh;
    }
}
