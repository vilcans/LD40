using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class MeshFromPolygon : MonoBehaviour {

    void Awake() {
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if(meshFilter == null) {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }
        Mesh mesh = meshFilter.mesh;

        List<Vector3> meshVertices = new List<Vector3>();
        List<int> meshIndices = new List<int>();
        List<Vector2> meshUVs = new List<Vector2>();

        for(int pathIndex = 0, pathCount = poly.pathCount; pathIndex < pathCount; ++pathIndex) {
            Vector2[] path = poly.GetPath(pathIndex);
            Debug.LogFormat("path {0}: {1} vertices", pathIndex, path.Length);
            Triangulator triangulator = new Triangulator(path);
            int[] indices = triangulator.Triangulate();

            int indexOffset = meshVertices.Count;
            for(int i = 0, len = path.Length; i < len; ++i) {
                meshVertices.Add(new Vector3(path[i].x, path[i].y, 0));
                meshUVs.Add(path[i]);
            }
            for(int i = 0, len = indices.Length; i < len; ++i) {
                meshIndices.Add(indices[i] + indexOffset);
            }
        }
        mesh.vertices = meshVertices.ToArray();
        mesh.uv = meshUVs.ToArray();
        mesh.triangles = meshIndices.ToArray();
    }
}
