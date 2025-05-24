using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TimeUtil
{

    public static long GetTimeStamp()
    {
        DateTime dt = System.DateTime.Now;
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        TimeSpan toNow = dt.Subtract(dtStart);
        long timeStamp = toNow.Ticks;
        return long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 4));
    }
}

