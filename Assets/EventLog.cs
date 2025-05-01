using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventLog : MonoBehaviour {
    public static EventLog Instance { get; private set; }

    [Header("UI References")]
    public RectTransform contentContainer;     // Scroll view's Content
    public GameObject logEntryPrefab;          // TMP Text prefab
    public ScrollRect scrollRect;              // To auto-scroll to bottom

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public void Log(string message) {
        if (logEntryPrefab == null || contentContainer == null) {
            Debug.LogWarning("EventLog is not properly configured.");
            return;
        }

        GameObject entryObj = Instantiate(logEntryPrefab, contentContainer);
        TMP_Text entryText = entryObj.GetComponent<TMP_Text>();

        if (entryText != null) {
            entryText.text = message;
        }

        Canvas.ForceUpdateCanvases(); // Ensure layout updates immediately

        // Scroll to bottom
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
