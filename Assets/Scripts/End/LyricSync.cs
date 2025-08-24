using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class LyricSync : MonoBehaviour
{
    public AudioSource audioSource;   // 音乐
    public Text lyricText;            // UI 文本
    public TextAsset lrcFile;         // LRC 歌词文件（拖到 Inspector）
    public Text text;
    private List<LrcLine> lines;
    private int currentIndex = 0;
    void Start()
    {
        lines = LrcParser.Parse(lrcFile.text);
        lines.Sort((a, b) => a.time.CompareTo(b.time)); // 按时间排序
        audioSource.Play();
        text.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-530, 2000), 180f).SetEase(Ease.Linear).OnComplete(() =>
        {
            SceneManager.LoadScene("MainScene");
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DOTween.KillAll();
            SceneManager.LoadScene("MainScene");
        }
        if (lines == null || lines.Count == 0) return;
        float t = audioSource.time;

        // 如果到达下一行歌词时间，就更新显示
        if (currentIndex < lines.Count - 1 && t >= lines[currentIndex + 1].time)
        {
            currentIndex++;
        }
        lyricText.text = lines[currentIndex].text;
    }
}
