
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KGSocket
{
    public abstract class KGBaseNet
    {
        public Socket mSocket;

        public KGBaseNet()
        {
            //这里就是new一个socket出来 指定地址类型 和套接字类型（ 就是传送数据类型），还有协议
            mSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        }

        //开启建立
        public abstract void StartCreate(string ip,int port);

        //建立的回调
        public abstract void ConnectAsync(IAsyncResult ar);

        //打印的调用
        public void SetLog(Action<string, LogLevel> LogEvent,bool run=true)
        {
            LogEvent.SetLog(run);
        }
    }
}
