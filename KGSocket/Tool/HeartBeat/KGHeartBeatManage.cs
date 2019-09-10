using System;
using System.Collections.Generic;
using System.Linq;




namespace KGSocket.Tool
{
    public class KGHeartBeatManage<T,R>:IDisposable where R:KGHeartBeat,new()
    {
     

        //主定时器
        System.Timers.Timer timer;

        private readonly object mlock = new object();


        //发送心跳包事件
        public event Action<T> SendHearEvent;

        //心跳包超时事件
        public event Action<T> ConnectLostEvent;

        //每个会话对应一个心跳KGHeartBeat
        private Dictionary<T, R> connectDic = new Dictionary<T, R>();

        public Dictionary<T, R> ConnectDic { get => connectDic; protected set => connectDic = value; }

        public KGHeartBeatManage(Action<T> sendHearEvent, Action<T> connectLostEvent, double timeinterval=1000)
        {


            //这里是赋值每过多少秒执行一次事件
            timer = new System.Timers.Timer(timeinterval);

            SendHearEvent = sendHearEvent;
            ConnectLostEvent = connectLostEvent;

            //定时执行事件
            timer.Elapsed +=(v,f) => 
            {
                //遍历每个会话回调一次发送心跳包
                lock (mlock)
                {
                    if (ConnectDic.Count > 0)
                    {
                        List<T> RemoveList = new List<T>();
                        foreach (KeyValuePair<T, R> item in ConnectDic)
                        {
                            SendHearEvent?.Invoke(item.Key);

                            //检查 心跳包超时 如果超时满次数就移除并回调事件
                            item.Value.CheckHeat();
                            if (item.Value.Lostcount >= item.Value.MaxLostcount)
                            {
                                RemoveList.Add(item.Key);
                               
                            }
                            RemoveList.ForEach(remove=> 
                            {
                                ConnectLostEvent(remove);
                                ConnectDic.Remove(remove);
                            });
                        }
                    }
                }
              
            };

            timer.Start();
        }
      
        
        /// <summary>
        /// 添加存储新的心跳包
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="heart"></param>
        /// <param name="maxlosttime"></param>
        /// <param name="maxlost"></param>
        public virtual KGHeartBeatManage<T,R> AddConnectDic(T obj,R heart=null, double maxlosttime = 2, int maxlost = 3)
        {
            R heartBeat;
            heartBeat = heart==null? new KGHeartBeat(maxlosttime, maxlost) as R : heart;

            lock (mlock)
            {
                ConnectDic.Add(obj, heartBeat);
            }

            return this;
        }


        public void RemoveConnectDic(T obj)
        {
            lock (mlock)
            {
                ConnectDic.Remove(obj);
            }
        }

        /// <summary>
        /// 遍历更新某个心跳包
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateOneHeat(T obj)
        {
            lock (mlock)
            {
                ConnectDic[obj].UpdateHeat();
            }
        }

        public void Dispose()
        {
            timer.Dispose();
            
        }

    }
}

