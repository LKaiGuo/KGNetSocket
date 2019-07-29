
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KGSocket
{
    public  class KGSocketServe<T, R> : KGBaseNet where T : KGNetSession<R>, new() where R : KGNetData
    {
        public List<T> SessionList=new List<T>();//储存会话管理的
        public  int NetListen=10;//监听数



        public override void StartCreate(string ip, int port)
        {
            try
            {
                //绑定地址
                mSocket.Bind(new IPEndPoint(IPAddress.Parse(ip),port));
                //监听数
                mSocket.Listen(NetListen);
                //进行异步监听
                mSocket.BeginAccept(ConnectAsync, null);
                ("建立服务器........").KLog();
            }
            catch (Exception e)
            {
                ("StartCreateError：" + e).KLog(LogLevel.Err);

            }
        }

        //异步回调
        public override void ConnectAsync(IAsyncResult ar)
        {
          
            try
            {
                T Client = new T();
                //这里结束接收 获取刚刚连接的socket
              Socket sk=  mSocket.EndAccept(ar);

                //开始监听  第二个是加入结束事件
                Client.StartReciveData(sk,
                    ()=> 
                    {
                        SessionList.Remove(Client);
                    });
                //添加进SessionList储存
                SessionList.Add(Client);
                //开始新一轮接收连接
                mSocket.BeginAccept(ConnectAsync, null);
            }
            catch (Exception e)
            {
                ("ConnectAsyncError：" + e).KLog(LogLevel.Err);

            }
        }
    }
}
