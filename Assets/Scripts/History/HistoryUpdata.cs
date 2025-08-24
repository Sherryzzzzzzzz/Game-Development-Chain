using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryUpdata : MonoBehaviour
{
    private bool isProgrammaticChange = false;

    public void NeedUpdate(Scrollbar scrollbar)
    {
        // 避免程序修改值时的递归调用
        if (isProgrammaticChange) return;

        float value = scrollbar.value;

        if (value >= 0.8f)
        {
            // 向上滚动内容（显示更早的记录）
            if (DialogLogManager.instance.UpdateForUp())
            {
                isProgrammaticChange = true;
                StartCoroutine(SetValueAfterDelay(0.5f));
                isProgrammaticChange = false;
            }
        }
        else if (value <= 0.2f)
        {
            // 向下滚动内容（显示更新的记录）
            if (DialogLogManager.instance.UpdateForDown())
            {
                isProgrammaticChange = true;
                StartCoroutine(SetValueAfterDelay(0.5f));
                isProgrammaticChange = false;
            }
        }
        IEnumerator SetValueAfterDelay(float newValue)
        {
            yield return null; // 等待一帧，跳出当前事件循环
            scrollbar.value = newValue;
        }
    }
}
