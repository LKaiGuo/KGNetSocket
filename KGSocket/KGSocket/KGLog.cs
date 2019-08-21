using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KGSocket
{
    /// <summary>
    /// 打印消息的工具拓展类
    /// </summary>
    public static class KGLog
    {
        public static bool RunLog = true;

        //留的一个打印事件委托
        private static  Action<string, LogLevel> LogEvent = null;

        //这里是打印消息的方法
        public static void KLog(this string Logdata,LogLevel logLevel=LogLevel.Common)
        {
            if (!RunLog)
                return;

            LogEvent?.Invoke(Logdata,logLevel);

            Console.WriteLine("{0}-----------------{1}", Logdata,logLevel.ToString());
         

        }

        public static void SetLog(this Action<string, LogLevel> log,bool Run=true)
        {
            LogEvent = log;
            RunLog = Run;
        }
    }

    //打印等级
    public enum  LogLevel
    {
        None=0,
        Common=1,
        Warn=2,
        Err=3

    }
}
