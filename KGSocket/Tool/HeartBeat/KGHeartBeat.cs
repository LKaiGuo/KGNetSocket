using System;



namespace KGSocket.Tool
{
    public class KGHeartBeat
    {

        //保存上一次心跳的时间
        protected long lastHeartTime;

        //超时次数
        public int Lostcount;

        //心跳包最大超时多少次
        public int MaxLostcount;

        //超时多久算一次
        public double MaxLostTime;

        //计算时间差的
        public long NowTimeSpan => Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);


        /// <summary>
        /// 检查心跳包延时
        /// </summary>
        public virtual void CheckHeat()
        {
            if (Math.Abs(lastHeartTime - NowTimeSpan) > MaxLostTime)
            {
                lastHeartTime = NowTimeSpan;
                Lostcount++;
            }
        }


        /// <summary>
        /// 更新心跳包
        /// </summary>
        public virtual void UpdateHeat()
        {
            lastHeartTime = NowTimeSpan;
            Lostcount = 0;
        }


        public  KGHeartBeat(double maxlosttime = 2, int maxlost = 3)
        {
          
            MaxLostTime = maxlosttime;
            MaxLostcount = maxlost;
            lastHeartTime = NowTimeSpan;

        }

        public KGHeartBeat()
        {


        }
    }
}
