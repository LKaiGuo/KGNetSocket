using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGSocket
{
    public class KGNetPacket
    {


        public byte[] PacketBuff;


        public int HeadLength = 4;//这里是标头的长度
        public int HeadIndex;//这里有时候分包接收到一两个 所以要进行记录已经接收到两个了 还差几个

        public int PacketBuffLength ;//数据包的长度
        public int PacketIndex;

        /// <summary>
        /// 获取四个字节转成的int长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public  void SetPackLen()
        {
            PacketBuffLength = BitConverter.ToInt32(PacketBuff, 0);
            PacketBuff =new byte[PacketBuffLength];
        }


        public void Refresh()
        {
            PacketBuff = null;
            PacketIndex = 0;
            HeadIndex = 0;
        }
    }
}
