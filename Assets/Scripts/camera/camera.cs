using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform target; // Nhân vật
    private Vector3 offset; // Khoảng cách ban đầu giữa camera và nhân vật

    [Range(1, 20)]
    public float smoothSpeed = 10f; // Tốc độ chuyển động của camera

    void Start()
    {
        // Lưu lại khoảng cách ban đầu giữa camera và nhân vật
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // Tính toán vị trí mới cho camera
        Vector3 desiredPosition = target.position + offset;

        // Sử dụng nội suy (Lerp) để camera di chuyển mượt mà đến vị trí mới
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Cập nhật vị trí camera
        transform.position = smoothedPosition;
    }
}
