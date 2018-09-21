using System;
using System.Collections.Generic;
using System.Text;

public struct TimeFormatHelper
{
    public string GetTimeFormat(float t)
    {
        int time = (int)t;

        int s = time % 60;
        int m = time / 60;

        if (m > 60)
            m = 60;

        var sb = new StringBuilder();
        sb.Append(m.ToString("00"));
        sb.Append(":");
        sb.Append(s.ToString("00"));

        return sb.ToString();
    }
}

