using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFullScreen : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        // 获取 Sprite 的世界尺寸（忽略 scale）
        float spriteWidth = sr.sprite.rect.width / sr.sprite.pixelsPerUnit;
        float spriteHeight = sr.sprite.rect.height / sr.sprite.pixelsPerUnit;

        // 获取相机可见范围
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * Screen.width / Screen.height;

        // 计算缩放比
        Vector3 scale = transform.localScale;
        scale.x = worldScreenWidth / spriteWidth;
        scale.y = worldScreenHeight / spriteHeight;
        transform.localScale = scale;
        transform.position = Vector3.zero;
    }
}
