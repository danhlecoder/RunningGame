using UnityEngine;
using UnityEngine.UI;

public class DistanceTracker : MonoBehaviour
{
    public Transform player; // Nhân vật hoặc đối tượng cần theo dõi
    public Text distanceText; // Text UI để hiển thị khoảng cách
    private Vector3 startPoint; // Điểm bắt đầu của nhân vật

    void Start()
    {
        // Lưu lại vị trí bắt đầu của nhân vật
        startPoint = player.position;
    }

    void Update()
    {
        // Tính toán khoảng cách đã chạy
        float distance = Vector3.Distance(new Vector3(0, 0, startPoint.z), new Vector3(0, 0, player.position.z));

        // Hiển thị lên UI, chuyển đổi sang mét
        distanceText.text = $"{distance:F1} M";
    }
}
