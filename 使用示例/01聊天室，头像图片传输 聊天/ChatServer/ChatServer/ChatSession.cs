
using KGSocket;
using ChatNetData;

namespace ChatServer
{
    /// <summary>
    /// 这里是每个客户端会话管理
    /// </summary>
    public class ChatSession : KGNetSession<ChatDatas>
    {

        public int SessionID = 0;
        public string PlayerName;
        public byte[] HeadData;

        protected override void OnDisRecive()
        {
            ("名字：" + PlayerName + "已下线").KLog();

        }
        protected override void OnReciveData(ChatDatas data)
        {
            ("收到ID：" + SessionID + "的请求"+(CMD)data.Cmd).KLog();
            ChatServe.Instance.AddDataPackQue(this,data);
        }

        protected override void OnStartRecive()
        {
            SessionID = ChatServe.Instance.GetSessionID();
            ("ID：" + SessionID + "已连接").KLog();
       
          
        }
    }
}
