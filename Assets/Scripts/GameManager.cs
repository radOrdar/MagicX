using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private int targetFPS = 120;

    void Start() {
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width/2, cursorTexture.height/2), CursorMode.Auto);
        Application.targetFrameRate = targetFPS;
    }

    void Update() {
        int fps =(int) (1 / Time.deltaTime);
        fpsText.text = $"FPS: {fps}";
    }
}