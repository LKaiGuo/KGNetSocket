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
            if (Math.Abs(lastHeartTime - this.NowTimeSpan) > MaxLostTime)
            {
               
                lastHeartTime = this.NowTimeSpan;
                Lostcount++;
            }
        }


        /// <summary>
        /// 更新心跳包
        /// </summary>
        public virtual void UpdateHeat()
        {
            lastHeartTime = this.NowTimeSpan;
            Lostcount = 0;
        }

        public virtual T InitMax<T>(double maxlosttime = 2, int maxlost = 3) where T: KGHeartBeat
        {
            MaxLostTime = maxlosttime;
            MaxLostcount = maxlost;
            //第一次赋值肯定要刷新
            lastHeartTime = this.NowTimeSpan;
            return this as T;
        }

        public virtual void InitMax(double maxlosttime = 2, int maxlost = 3) 
        {
            MaxLostTime = maxlosttime;
            MaxLostcount = maxlost;
            //第一次赋值肯定要刷新
            lastHeartTime = this.NowTimeSpan;
        }

    }
}
