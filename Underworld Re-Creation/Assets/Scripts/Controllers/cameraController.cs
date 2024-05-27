using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public Transform target; // ����, �� ������� ����� ��������� ������ (�����)
    public Vector3 offset; // �������� ������ ������������ ����

    void Start()
    {
        // ��������� ��������� �������� ������
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // ����� ������� ������ � ������ ��������
        Vector3 targetCamPos = target.position + offset;

        // ����������� ������ ��������������� � ����� �������
        transform.position = targetCamPos;
    }

}
