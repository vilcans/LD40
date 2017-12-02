using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class MeshFromPolygon : MonoBehaviour {

    private Mesh inEditorMesh;

    private float uvScale = 1.0f / 16;

    void Awake() {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if(meshFilter == null) {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }
        UpdateMesh(meshFilter.mesh);
    }

    private void UpdateMesh(Mesh mesh) {
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();

        List<Vector3> meshVertices = new List<Vector3>();
        List<int> meshIndices = new List<int>();
        List<Vector2> meshUVs = new List<Vector2>();

        for(int pathIndex = 0, pathCount = poly.pathCount; pathIndex < pathCount; ++pathIndex) {
            Vector2[] path = poly.GetPath(pathIndex);
            Triangulator triangulator = new Triangulator(path);
            int[] indices = triangulator.Triangulate();

            int indexOffset = meshVertices.Count;
            for(int i = 0, len = path.Length; i < len; ++i) {
                meshVertices.Add(new Vector3(path[i].x + poly.offset.x, path[i].y + poly.offset.y, 0));
                meshUVs.Add(path[i] * uvScale);
            }
            for(int i = 0, len = indices.Length; i < len; ++i) {
                meshIndices.Add(indices[i] + indexOffset);
            }
        }
        mesh.vertices = meshVertices.ToArray();
        mesh.uv = meshUVs.ToArray();
        mesh.triangles = meshIndices.ToArray();
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos() {
        if(Application.isPlaying) {
            return;
        }
        if(inEditorMesh == null) {
            inEditorMesh = new Mesh();
        }
        UpdateMesh(inEditorMesh);
        Transform t = transform;
        Gizmos.DrawMesh(inEditorMesh, t.position, t.rotation, t.lossyScale);
        Gizmos.DrawWireMesh(inEditorMesh, t.position, t.rotation, t.lossyScale);
    }
}
