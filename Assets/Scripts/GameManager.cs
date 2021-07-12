using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private Sprite cursorSprite;

    private void Start() {
        ChangeCursor();
    }

    private void ChangeCursor() {
        var croppedTexture = new Texture2D((int) cursorSprite.rect.width, (int) cursorSprite.rect.height);
        var pixels = cursorSprite.texture.GetPixels((int) cursorSprite.textureRect.x,
            (int) cursorSprite.textureRect.y,
            (int) cursorSprite.textureRect.width,
            (int) cursorSprite.textureRect.height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        Cursor.SetCursor(croppedTexture, Vector2.zero, CursorMode.Auto);
    }
}