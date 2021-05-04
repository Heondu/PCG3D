using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combine : MonoBehaviour
{
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;

    [ContextMenu("Combine")]
    public void MeshCombine()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(false);
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>(false);

        List<Material> materials = new List<Material>();

        for (int i = 1; i < meshRenderers.Length; i++)
        {
            if (!materials.Contains(meshRenderers[i].sharedMaterial))
            {
                materials.Add(meshRenderers[i].sharedMaterial);
            }
        }

        List<Mesh> meshes = new List<Mesh>();
        for (int i = 0; i < materials.Count; i++)
        {
            List<CombineInstance> combine = new List<CombineInstance>();
            for (int j = 1; j < meshFilters.Length; j++)
            {
                if (materials[i] == meshRenderers[j].sharedMaterial)
                {
                    CombineInstance ci = new CombineInstance();

                    ci.mesh = meshFilters[j].sharedMesh;
                    ci.transform = meshFilters[j].transform.localToWorldMatrix;
                    ci.subMeshIndex = 0;
                    combine.Add(ci);
                    meshFilters[j].gameObject.SetActive(false);
                }
            }
            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combine.ToArray(), true);
            meshes.Add(mesh);
        }

        List<CombineInstance> combineFinal = new List<CombineInstance>();
        for (int i = 0; i < meshes.Count; i++)
        {
            CombineInstance ci = new CombineInstance();
            ci.mesh = meshes[i];
            ci.transform = Matrix4x4.identity;
            ci.subMeshIndex = 0;
            combineFinal.Add(ci);
        }

        meshFilter.mesh.CombineMeshes(combineFinal.ToArray(), false);
        meshRenderer.materials = materials.ToArray();
        gameObject.SetActive(true);
    }
}
