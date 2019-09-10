using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

using System.Threading;

namespace KGSocket
{
    /// <summary>
    /// 单个的网络会话管理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract  class KGNetSession<T> where T:KGNetData
    {
        public Socket mSocket;



        public event Action<T>  OnReciveDataEvent;//接收到的数据委托事件
        public event Action OnDisReciveEvent;//关闭会话的事件
        public event Action OnStartReciveEvent;//首次开启连接的事件

        public KGNetPacket mKGNetPacket=new KGNetPacket();

        /// <summary>
        /// 这里开始数据接收
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="close"></param>
        public void StartReciveData(Socket socket,Action close=null)
        {
            try
            {
               // 初始化赋值
                mSocket = socket;
                OnDisReciveEvent+= close;

                //回调开启连接事件
                OnStartRecive();
                OnStartReciveEvent?.Invoke();
                //首先是接收头4个字节确认包长
                //4可能太小了
                mKGNetPacket.PacketBuff = new byte[4];
                mSocket.BeginReceive(mKGNetPacket.PacketBuff, 0, 4, SocketFlags.None, ReciveHeadData, null);
            }
            catch (Exception e)
            {
                ("StartReciveDataError：" + e).KLog(LogLevel.Err);

            }
          
        }

        /// <summary>
        /// 这里是判断接收标头的就是长度
        /// </summary>
        /// <param name="ar"></param>
        protected void ReciveHeadData(IAsyncResult ar)
        {
          
            try
            {

                //如果接收数据长度等于0就是断开连接了 
                //为啥要加这个勒 在断开的时候 异步会回调一次 直接调用EndReceive 会报错
                if (mSocket == null)
                    return;
                if (mSocket.Available == 0)
                {
                    Clear();
                    return;
                }
                
                int len = mSocket.EndReceive(ar);
                if (len > 0)
                {
                    mKGNetPacket.HeadIndex += len;
                    //这里如果是小于4的就是凑不成 就是分包了 要继续接收
                    if (mKGNetPacket.HeadIndex < mKGNetPacket.HeadLength)
                    {
                        //                                                                            4-？《=4

                        mSocket.BeginReceive(mKGNetPacket.PacketBuff, mKGNetPacket.HeadIndex, mKGNetPacket.HeadLength - mKGNetPacket.HeadIndex, SocketFlags.None, ReciveHeadData, null);
                    }
                    //这里已经取出长度了
                    else
                    {
                        //赋值从那四个字节获取的byte[]的长度
                        mKGNetPacket.SetPackLen();
                        //进行真正的数据接收处理
                        mSocket.BeginReceive(mKGNetPacket.PacketBuff, 0, mKGNetPacket.PacketBuffLength, SocketFlags.None, ReciveData, null);
                    }
                }
                else
                {
                  
                    Clear();
                    
                }
            }
            catch (Exception e)
            {

                ("ReciveHeadDataError：" + e).KLog(LogLevel.Err);
            }
           
         
        }

        /// <summary>
        /// 这里是接收到包里面的数据异步处理
        /// </summary>
        /// <param name="ar"></param>
        protected void ReciveData(IAsyncResult ar)
        {
            try
            {
                //结束接收获取长度
                int len = mSocket.EndReceive(ar);

                if (len>0)
                {
                    mKGNetPacket.PacketIndex += len;

                    //这里是如果接收到的包长和原本获取到的长度小，就是分包了 需要再次进行接收剩下的
                    if (mKGNetPacket.PacketIndex < mKGNetPacket.PacketBuffLength)
                    {

                        mSocket.BeginReceive(mKGNetPacket.PacketBuff, mKGNetPacket.PacketIndex, mKGNetPacket.PacketBuffLength - mKGNetPacket.PacketIndex, SocketFlags.None, ReciveData, null);
                    }
                    //已经接完一组数据了
                    else
                    {
                        //这里就可以进行回调函数了
                        OnReciveData(mKGNetPacket.PacketBuff.DeSerialization<T>());
                        OnReciveDataEvent?.Invoke(mKGNetPacket.PacketBuff.DeSerialization<T>());

                        //开始新一轮的从上往下接收了
                        mKGNetPacket.Refresh();
                        mKGNetPacket.PacketBuff = new byte[4];
                        mSocket.BeginReceive(mKGNetPacket.PacketBuff, 0, 4, SocketFlags.None, ReciveHeadData, null);
                    }
                }
                else
                {
                    Clear();
                }

            }
            catch (Exception e)
            {
                ("ReciveDataError：" + e).KLog(LogLevel.Err);

            }
        
        }
#region Send

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data"></param>
        public void SendData(T data)
        {
            //这里转回来 byte[]
            byte[] bytedata = data.PackNetData();
            //创建流准备异步写入发送
            NetworkStream network = null;

            try
            {
                //指定写入的socket
                network = new NetworkStream(mSocket);

                //判断是否可以支持写入消息
                if (network.CanWrite)
                {
                    //开始异步写入
                    network.BeginWrite(bytedata,0, bytedata.Length, SendDataAsync, network);
                }
            }
            catch (Exception e)
            {

                ("SendDataError：" + e).KLog(LogLevel.Err);
            }

        }

        /// <summary>
        /// 这里是异步写入回调
        /// </summary>
        /// <param name="ar"></param>
        protected void SendDataAsync(IAsyncResult ar)
        {
            //拿到写入时候的流
            NetworkStream network = (NetworkStream)ar.AsyncState;
            try
            {
                //结束写入 就是发送了  然后进行关闭流
                network.EndWrite(ar);
                network.Flush();
                network.Close();
              
            }
            catch (Exception e)
            {
                ("SendDataAsyncError：" + e).KLog(LogLevel.Err);
            }
        }
        #endregion
    /// <summary>
    /// 网络关闭
    /// </summary>
        public void Clear()
        {
            OnDisRecive();
            OnDisReciveEvent?.Invoke();
            mSocket.Close();
            mSocket = null;
        }


        protected virtual void OnReciveData(T data)
        {
            
        }
        
        protected virtual void OnDisRecive()
        {

        }

        protected virtual void OnStartRecive()
        {
            
        }


    }
}
