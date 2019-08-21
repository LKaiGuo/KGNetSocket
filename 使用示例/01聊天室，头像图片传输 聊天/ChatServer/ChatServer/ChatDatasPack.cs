
using ChatNetData;


namespace ChatServer
{
    /// <summary>
    /// 这个是分辨这个数据包是哪个客户端发的用的 数据结构
    /// </summary>
    public class ChatDatasPack
    {
        public ChatSession chatSession;

        public ChatDatas chatDatas;

    }
}
