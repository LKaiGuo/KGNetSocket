
using KGSocket;
using ChatNetData;

namespace ChatServer
{
    public class ChatSession : KGNetSession<ChatDatas>
    {

        public int SessionID = 0;
        public string PlayerName;//缓存用户的名字
        public byte[] HeadData;//缓存用户的头像图片数据

        protected override void OnDisRecive()
        {
            ("名字：" + PlayerName + "已下线").KLog();

        }
        protected override void OnReciveData(ChatDatas data)
        {
            ("收到名字：" + data.PlayerName + "的请求" + (CMD)data.Cmd).KLog();
            ChatServe.Instance.AddDataPackQue(this, data);
        }

        protected override void OnStartRecive()
        {
            SessionID = ChatServe.Instance.GetSessionID();
            ("ID：" + SessionID + "已连接").KLog();


        }
    }
}
