using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatNetData
{
    
    public enum CMD
    {
        None,
        HeartBeat,

        ReqLogin,
        ReqChatInfo,

        RspLogin,
        RspChatInfo,


    }
}
