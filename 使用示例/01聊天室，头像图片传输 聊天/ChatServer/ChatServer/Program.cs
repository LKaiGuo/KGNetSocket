using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatServe chatServe= ChatServe.Instance;
            chatServe.StartCreate("127.0.0.1",8895);
            while (true)
            {
                chatServe.Update();
            }

        }
    }
}
