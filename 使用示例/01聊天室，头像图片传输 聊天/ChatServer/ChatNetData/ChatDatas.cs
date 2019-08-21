using KGSocket;
using System;

namespace ChatNetData
{
    [Serializable]
    public class ChatDatas:KGNetData
    {
        public byte[] HeadData;//头像图片的数据

        public string PlayerName;

        public SendChat Chatdata;//聊天消息的数据
    }
}
