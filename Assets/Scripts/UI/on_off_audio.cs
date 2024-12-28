using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    public Button muteButton;      // Nút Mute
    public Sprite muteIcon;        // Biểu tượng loa tắt
    public Sprite unmuteIcon;      // Biểu tượng loa bật
    private Image buttonImage;     // Thành phần hình ảnh của nút
    private bool isMuted;          // Trạng thái âm thanh

    void Awake()
    {
        // Gắn hình ảnh của nút
        buttonImage = muteButton.GetComponent<Image>();

        // Khởi tạo trạng thái âm thanh
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1; // Lấy trạng thái từ PlayerPrefs
        AudioListener.pause = isMuted; // Tắt/bật âm thanh toàn bộ dựa trên trạng thái

        // Cập nhật biểu tượng nút
        UpdateMuteButtonIcon();

        // Gắn sự kiện cho nút
        muteButton.onClick.AddListener(ToggleAudio);
    }

    // Hàm bật/tắt trạng thái âm thanh
    private void ToggleAudio()
    {
        isMuted = !isMuted;
        AudioListener.pause = isMuted; // Tắt/bật âm thanh toàn bộ
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0); // Lưu trạng thái vào PlayerPrefs
        PlayerPrefs.Save(); // Lưu lại cài đặt
        UpdateMuteButtonIcon(); // Cập nhật biểu tượng
    }

    // Cập nhật biểu tượng trên nút
    private void UpdateMuteButtonIcon()
    {
        if (isMuted)
        {
            buttonImage.sprite = muteIcon; // Biểu tượng loa tắt
        }
        else
        {
            buttonImage.sprite = unmuteIcon; // Biểu tượng loa bật
        }
    }
}
