using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeshFromSkinnedMesh : MonoBehaviour
{
    //����� ������ ���� ������ ��������� �������� SkinnedMeshRenderer �� MeshRenderer
    [ContextMenu("Convert to regularMesh")]
    void Start()
    {
        if (GetComponent<SkinnedMeshRenderer>())
        {
            SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            MeshFilter filter = gameObject.AddComponent<MeshFilter>();
            MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
            filter.mesh = skinnedMeshRenderer.sharedMesh;
            renderer.sharedMaterials = skinnedMeshRenderer.sharedMaterials;
            DestroyImmediate(skinnedMeshRenderer);
            DestroyImmediate(this);
        }
    }

}