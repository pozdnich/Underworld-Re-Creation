using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public Transform target; // ÷ель, за которой будет следовать камера (игрок)
    public Vector3 offset; // —мещение камеры относительно цели

    void Start()
    {
        // ¬ычисл€ем начальное смещение камеры
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // Ќова€ позици€ камеры с учетом смещени€
        Vector3 targetCamPos = target.position + offset;

        // ѕеремещение камеры непосредственно к новой позиции
        transform.position = targetCamPos;
    }

}
