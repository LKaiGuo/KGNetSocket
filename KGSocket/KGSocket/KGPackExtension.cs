using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace KGSocket
{
    /// <summary>
    /// 打包消息的拓展工具类
    /// </summary>
    public static class KGPackExtension
    {
        


 #region 打包消息包的

        /// <summary>
        /// 反序列化消息包返回自定义数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeSerialization<T>(this byte[] data) where T:KGNetData
        {
            //using 创建完会自动释放内存 创建流MemoryStream 读消息
            using (MemoryStream ms=new MemoryStream(data))
            {
                //BinaryFormatter 用来反,序列化流的
                BinaryFormatter binary = new BinaryFormatter();
                //反序列成自定义数据类型
                T netdata =  (T) binary.Deserialize(ms);
                return netdata;
            }
        }

        /// <summary>
        /// 把自定义数据类型序列化成byte[] 数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] Serialization<T>(this T data) where T : KGNetData
        {
            //using 创建完会自动释放内存 创建流MemoryStream 
            using (MemoryStream ms = new MemoryStream())
            {
                //BinaryFormatter 用来反,序列化流的
                BinaryFormatter binary = new BinaryFormatter();
                //把数据写进去流里面
               binary.Serialize(ms,data);
                ms.Seek(0, SeekOrigin.Begin);
                //转换byte[]返回
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 为消息包前头增加消息包长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] PackNetDataLen(this byte[] data)
        {
            //获取消息包长度返回一个byte[4]          //连接合并数据包   进行最终返回
            return BitConverter.GetBytes(data.Length).Concat(data).ToArray();
        }

     
        /// <summary>
        /// 打包自定义数据类的消息包 加上包长
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] PackNetData<T>(this T data) where T : KGNetData
        {
            return data.Serialization().PackNetDataLen();
        }
#endregion
    }
}
