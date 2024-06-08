using UnityEngine;
using System.Linq;

public class ColorOnHover : MonoBehaviour
{

    public Color color;
    public Renderer meshRenderer;

    Color[] originalColours;

    void Start()
    {
        if (meshRenderer == null)
        {
            if (GetComponent<MeshRenderer>())
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
            else
            {
                meshRenderer = GetComponentInChildren<MeshRenderer>();
            }
        }
        originalColours = meshRenderer.materials.Select(x => x.color).ToArray();
    }

    void OnMouseEnter()
    {
        foreach (Material mat in meshRenderer.materials)
        {
            mat.color *= color;
        }

    }

    void OnMouseExit()
    {
        for (int i = 0; i < originalColours.Length; i++)
        {
            meshRenderer.materials[i].color = originalColours[i];
        }
    }

}
