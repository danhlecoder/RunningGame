using UnityEngine;
using UnityEngine.UI;

public class player_manager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;

    public static bool isGameStarted;
    public GameObject startingText;

    public static int numberOfCoins; // Số xu trong ván chơi hiện tại
    public Text coinsText;

    public Text distanceText;
    public Text timeText;

    private float distance;
    private float playTime;
    public float speed = 5f;

    void Start()
    {
        Time.timeScale = 1;
        gameOver = false;
        isGameStarted = false;
        numberOfCoins = 0;
        distance = 0;
        playTime = 0;
    }

    void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
            return;
        }

        if (isGameStarted)
        {
            playTime += Time.deltaTime;
            distance += speed * Time.deltaTime;

            distanceText.text = Mathf.FloorToInt(distance) + " m";
            timeText.text = Mathf.FloorToInt(playTime) + " s";
        }

        coinsText.text = numberOfCoins.ToString();

        if (SwipeManager.tap)
        {
            isGameStarted = true;
            Destroy(startingText);
        }
    }

    public float GetDistance()
    {
        return distance; // Trả về khoảng cách đã đi
    }

    public float GetPlayTime()
    {
        return playTime; // Trả về thời gian chơi
    }
}
