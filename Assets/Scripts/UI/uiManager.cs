using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public class UIControl
    {
        public Button button;       // Nút để mở giao diện
        public GameObject panel;    // Giao diện tương ứng
        public Button closeButton;  // Nút đóng giao diện (tuỳ chọn)
    }

    public UIControl[] uiControls; // Mảng chứa các cặp nút và giao diện

    void Start()
    {
        foreach (var control in uiControls)
        {
            if (control == null)
            {
                Debug.LogError("UIControl is null in the array.");
                continue;
            }

            if (control.button == null || control.panel == null)
            {
                Debug.LogError("Button or Panel is not assigned in UIControl.");
                continue;
            }

            // Tắt giao diện ban đầu
            control.panel.SetActive(false);

            // Gán sự kiện mở giao diện
            control.button.onClick.AddListener(() => OpenPanel(control.panel));

            // Gán sự kiện nút đóng nếu có
            if (control.closeButton != null)
            {
                control.closeButton.onClick.AddListener(() => ClosePanel(control.panel));
            }
        }
    }

    // Hàm mở giao diện và tắt các giao diện khác
    public void OpenPanel(GameObject panelToOpen)
    {
        foreach (var control in uiControls)
        {
            if (control.panel == null) continue; // Bỏ qua panel null
            control.panel.SetActive(control.panel == panelToOpen);
        }
    }

    // Hàm đóng giao diện
    public void ClosePanel(GameObject panelToClose)
    {
        if (panelToClose != null)
        {
            panelToClose.SetActive(false);
        }
    }

    // Hàm đóng tất cả các giao diện
    public void CloseAllPanels()
    {
        foreach (var control in uiControls)
        {
            if (control.panel != null)
            {
                control.panel.SetActive(false);
            }
        }
    }
}
