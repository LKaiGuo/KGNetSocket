using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KGSocket
{
    //传输的数据都必须打上可序列化的标签
    [Serializable]
  public abstract class KGNetData
    {
        public int Err;
        public int Cmd;
    }
}
