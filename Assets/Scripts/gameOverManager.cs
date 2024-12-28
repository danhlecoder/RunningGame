using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Text finalCoinsText;   // Text UI để hiển thị tổng số đồng xu kiếm được trong ván chơi này
    public Text finalDistanceText; // Text UI để hiển thị khoảng cách cuối cùng
    public Text totalCoinsText;   // Text UI để hiển thị tổng số đồng xu (qua các lần chơi)

    public player_manager playerManager; // Tham chiếu tới script player_manager

    void Start()
    {
        if (player_manager.gameOver)
        {
            // Lấy dữ liệu từ player_manager
            int finalCoins = player_manager.numberOfCoins;
            float finalDistance = playerManager.GetDistance();
            float playTime = playerManager.GetPlayTime();

            // Lấy tổng số đồng xu đã lưu từ trước
            int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

            // Cộng dồn số xu kiếm được trong ván chơi này vào tổng số xu
            totalCoins += finalCoins;

            // Lưu dữ liệu vào PlayerPrefs
            PlayerPrefs.SetInt("LastGameCoins", finalCoins);
            PlayerPrefs.SetFloat("LastGameDistance", finalDistance);
            PlayerPrefs.SetFloat("LastGameTime", playTime); // Thêm nếu cần lưu thời gian chơi
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            PlayerPrefs.Save();

            // Hiển thị thông tin lên giao diện
            finalCoinsText.text = "Coins: " + finalCoins;
            finalDistanceText.text = "Distance: " + Mathf.FloorToInt(finalDistance) + " m";
            totalCoinsText.text = "Total: " + totalCoins;
        }
    }
}
