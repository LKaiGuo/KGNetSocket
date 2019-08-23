using System;
using System.Collections.Generic;
using System.Linq;

using KGSocket;
using ChatNetData;

namespace ChatServer
{
    //各个客户端管理 业务逻辑都在这了
    public class ChatServe : KGSocketServe<ChatSession, ChatDatas>
    {
        #region 这里是单例
        private static ChatServe instance;
        public static ChatServe Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ChatServe();
                }
                return instance;
            }
            set => instance = value;
        }
        #endregion

        public static readonly object obj = "lock";

        public Queue<ChatDatasPack> DataPackQue = new Queue<ChatDatasPack>();



        #region 缓存
        private int SessionID = 0;

        public int GetSessionID()
        {
            if (SessionID == int.MaxValue)
            {
                SessionID = 0;
            }
            return SessionID += 1;
        }

        public bool IsUserOnLine(string name)
        {
            return SessionList.Select(v => v.PlayerName).ToList().Contains(name);
        }


        #endregion



        public void Update()
        {
            if (DataPackQue.Count > 0)
            {
                //多线程调用就要加个线程锁了避免同时调用
                lock (obj)
                {
                    DisposePack(DataPackQue.Dequeue());
                }
            }
        }

        /// <summary>
        /// 处理客户端发过来的消息
        /// </summary>
        /// <param name="pack"></param>
        public void DisposePack(ChatDatasPack pack)
        {
            switch ((CMD)pack.chatDatas.Cmd)
            {
                //登录指令
                case CMD.ReqLogin:
                    //判断在线的名字是否重复
                    if (IsUserOnLine(pack.chatDatas.PlayerName))
                    {
                        pack.chatDatas.Err = (int)ErrorInfo.NameRepeatsErr;

                    }
                    else
                    {
                        //返回指令
                        pack.chatDatas.Cmd = (int)CMD.RspLogin;
                        //存储角色的名字和头像图片数据
                        pack.chatSession.PlayerName = pack.chatDatas.PlayerName;
                        pack.chatSession.HeadData = pack.chatDatas.HeadData;
                     
                    }

                    //返回消息给客户端
                    pack.chatSession.SendData(pack.chatDatas);
                    break;
                //聊天指令
                case CMD.ReqChatInfo:
                    //返回指令
                    pack.chatDatas.Cmd = (int)CMD.RspChatInfo;
                    //赋值角色的名字和头像图片数据
                    pack.chatDatas.PlayerName = pack.chatSession.PlayerName;
                    pack.chatDatas.HeadData = pack.chatSession.HeadData;
                    //发回给自身聊天
                    pack.chatSession.SendData(pack.chatDatas);
                    //改成对方
                    pack.chatDatas.Chatdata.Islocal = 1;
                    //分发到各个客户端 聊天消息
                    SessionList.Where(v => v != pack.chatSession).ToList().ForEach(v =>
                        {
                            v.SendData(pack.chatDatas);
                        });
                    break;
            }


        }
        /// <summary>
        /// 这里是添加处理客户端消息任务队列
        /// </summary>
        /// <param name="session"></param>
        /// <param name="chatData"></param>
        public void AddDataPackQue(ChatSession session, ChatDatas chatData)
        {
            lock (obj)
            {
                DataPackQue.Enqueue(new ChatDatasPack { chatSession = session, chatDatas = chatData });
            }
        }


    }
}
