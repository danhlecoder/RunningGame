using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Events : MonoBehaviour
{
    public GameObject loadingScreen; // Giao diện màn hình chờ
    public Slider progressBar; // Thanh tiến trình
    public float smoothSpeed = 0.1f; // Tốc độ làm mượt thanh tiến trình

    public void ReplayGame()
    {
        StartCoroutine(LoadSceneAsync("Nhom5_BTL"));
    }

    public void QuitGame()
    {
        StartCoroutine(LoadSceneAsync("menu"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Kích hoạt màn hình chờ
        loadingScreen.SetActive(true);

        // Bắt đầu tải scene bất đồng bộ
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Ngăn chặn scene chuyển ngay lập tức

        float targetProgress = 0f; // Mục tiêu ban đầu cho tiến trình
        progressBar.value = 0f; // Đặt giá trị ban đầu của thanh tiến trình là 0

        // Cập nhật thanh tiến trình
        while (!asyncLoad.isDone)
        {
            // Tính toán giá trị tiến trình thực tế (0.9f là tối đa của asyncLoad.progress)
            targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // Làm mượt tiến trình thanh trượt với Lerp
            progressBar.value = Mathf.Lerp(progressBar.value, targetProgress, smoothSpeed);

            // Nếu đã tải gần xong (progress >= 0.9f), tiếp tục làm mượt đến 1
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
