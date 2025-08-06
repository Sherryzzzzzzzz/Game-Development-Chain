using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameBegin : MonoBehaviour
{
    public VideoPlayer video;

    public void Awake()
    {
        video = GetComponent<VideoPlayer>();
    }

    public void Start()
    {
        StartCoroutine(ToNextScene());
    }

    public IEnumerator ToNextScene()
    {
        float time = (float)video.length;
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene("01Dream");
    }
}
