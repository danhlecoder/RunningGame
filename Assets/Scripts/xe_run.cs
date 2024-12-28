using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float moveSpeed; // Tốc độ di chuyển
    public int direction = 1;  // Hướng di chuyển: 1 (Z dương)

    void Start()
    {
        // Nếu đi ngược lại (direction = -1), xoay xe 180 độ quanh trục Y
        if (direction == -1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void Update()
    {
        // Di chuyển xe theo hướng được chọn (luôn về phía trước nhân vật)
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}