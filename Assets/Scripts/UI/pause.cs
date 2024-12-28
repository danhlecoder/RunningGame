using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // Giao diện tạm dừng
    public GameObject loadingScreen; // Giao diện màn hình chờ
    public Slider progressBar; // Thanh tiến trình tải
    public float smoothSpeed = 0.1f; // Tốc độ làm mượt thanh tiến trình
    private bool isPaused = false;

    // Nút bật/tắt âm thanh
    public Button muteButton; // Nút Mute
    public Sprite muteIcon; // Biểu tượng loa tắt
    public Sprite unmuteIcon; // Biểu tượng loa bật
    private Image buttonImage; // Thành phần hình ảnh của nút
    private bool isMuted; // Trạng thái âm thanh

    // **Đếm ngược**
    public Text countdownText; // Text UI để hiển thị đếm ngược

    void Start()
    {
        // Khởi tạo trạng thái âm thanh
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1; // Lấy trạng thái từ PlayerPrefs
        AudioListener.pause = isMuted; // Tắt/bật âm thanh toàn bộ dựa trên trạng thái

        // Gắn sự kiện và cập nhật biểu tượng
        buttonImage = muteButton.GetComponent<Image>();
        muteButton.onClick.AddListener(ToggleAudio); // Gắn sự kiện cho nút
        UpdateMuteButtonIcon(); // Cập nhật biểu tượng ban đầu

        // Ẩn Text đếm ngược khi bắt đầu
        countdownText.gameObject.SetActive(false);
    }

    // Hàm bật/tắt trạng thái âm thanh
    public void ToggleAudio()
    {
        isMuted = !isMuted;
        AudioListener.pause = isMuted; // Tắt/bật âm thanh toàn bộ
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0); // Lưu trạng thái vào PlayerPrefs
        PlayerPrefs.Save();
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

    // Hàm bật/tắt tạm dừng
    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0; // Dừng thời gian
        }
        else
        {
            // Bắt đầu đếm ngược trước khi tiếp tục game
            StartCoroutine(CountdownBeforeResume());
        }
    }

    // Coroutine để đếm ngược trước khi tiếp tục
    private IEnumerator CountdownBeforeResume()
    {
        // Hiển thị đếm ngược và ẩn giao diện tạm dừng
        pauseMenu.SetActive(false);
        countdownText.gameObject.SetActive(true);

        for (int i = 5; i > 0; i--)
        {
            countdownText.text = i.ToString(); // Hiển thị số đếm ngược
            yield return new WaitForSecondsRealtime(1); // Đợi 1 giây (không bị ảnh hưởng bởi Time.timeScale)
        }

        countdownText.gameObject.SetActive(false); // Ẩn Text đếm ngược
        Time.timeScale = 1; // Tiếp tục thời gian
    }

    public void ReplayGame()
    {
        StartCoroutine(LoadSceneAsync("Nhom5_BTL"));
    }

    // Hàm thoát về màn hình chính với hiệu ứng loading
    public void QuitToMainMenu()
    {
        StartCoroutine(LoadSceneAsync("menu"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Kích hoạt màn hình chờ
        loadingScreen.SetActive(true);

        // Bắt đầu tải scene bất đồng bộ
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Ngăn scene chuyển ngay lập tức

        float targetProgress = 0f; // Mục tiêu ban đầu cho tiến trình
        progressBar.value = 0f; // Đặt giá trị ban đầu của thanh tiến trình là 0

        // Cập nhật thanh tiến trình
        while (!asyncLoad.isDone)
        {
            // Tính toán giá trị tiến trình thực tế
            targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // Làm mượt tiến trình thanh trượt với Lerp
            progressBar.value = Mathf.Lerp(progressBar.value, targetProgress, smoothSpeed);

            // Nếu đã tải gần xong, tiếp tục làm mượt đến 1
            if (asyncLoad.progress >= 0.9f && progressBar.value >= 0.99f)
            {
                // Khi thanh tiến trình đạt đến 1, cho phép scene chuyển
                progressBar.value = 1f;
                asyncLoad.allowSceneActivation = true;
            }

            yield return null; // Đợi đến frame tiếp theo
        }
    }
}
