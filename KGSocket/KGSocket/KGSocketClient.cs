
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KGSocket
{
    /// <summary>
    /// 建立客户端的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    public  class KGSocketClient<T, R> : KGBaseNet where T : KGNetSession<R>, new() where R : KGNetData
    {
        public T Client;

        public override void StartCreate(string ip, int port)
        {

            try
            {
                Client = new T();
                //异步连接服务器
                mSocket.BeginConnect(IPAddress.Parse(ip), port, ConnectAsync, Client);
                ("正在连接服务器").KLog();
            }
            catch (Exception e)
            {
                ("StartCreateError：" + e).KLog(LogLevel.Err);

            }
           

        }
        public override void ConnectAsync(IAsyncResult ar)
        {
            try
            {
                mSocket.EndConnect(ar);
                //连接完成开始接收数据
                Client.StartReciveData(mSocket,()=> { Client = null; });
            
            }
            catch (Exception e)
            {
                ("ConnectAsyncError：" + e).KLog(LogLevel.Err);
                
            }
         
        }

      
    }
}
