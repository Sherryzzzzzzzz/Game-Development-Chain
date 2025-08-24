using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class LrcLine
{
    public float time;   // 秒
    public string text;  // 歌词
}

public class LrcParser
{
    public static List<LrcLine> Parse(string lrcText)
    {
        List<LrcLine> lines = new List<LrcLine>();
        string[] rawLines = lrcText.Split('\n');

        foreach (string raw in rawLines)
        {
            string line = raw.Trim();
            if (string.IsNullOrEmpty(line)) continue;

            // 跳过元数据行（作曲、作词等）
            if (line.StartsWith("[0") && line.Contains(":]")) continue;

            // 匹配时间戳 [mm:ss.xx]
            int startBracket = line.IndexOf('[');
            int endBracket = line.IndexOf(']');

            if (startBracket >= 0 && endBracket > startBracket)
            {
                string timeStr = line.Substring(startBracket + 1, endBracket - startBracket - 1);
                string lyric = line.Substring(endBracket + 1).Trim();

                // 正确解析LRC时间格式 [分:秒.百分秒]
                if (TryParseLrcTime(timeStr, out float seconds))
                {
                    lines.Add(new LrcLine { time = seconds, text = lyric });
                }
            }
        }

        // 按时间排序
        lines.Sort((a, b) => a.time.CompareTo(b.time));
        return lines;
    }

    private static bool TryParseLrcTime(string timeStr, out float seconds)
    {
        seconds = 0f;

        // 分割分:秒.百分秒
        string[] parts = timeStr.Split(':', '.');
        if (parts.Length != 3) return false;

        if (int.TryParse(parts[0], out int minutes) &&
            int.TryParse(parts[1], out int secs) &&
            int.TryParse(parts[2], out int hundredths))
        {
            seconds = minutes * 60 + secs + hundredths * 0.01f;
            return true;
        }

        return false;
    }
}