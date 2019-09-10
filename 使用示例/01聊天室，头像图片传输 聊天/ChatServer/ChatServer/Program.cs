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
            ChatServe chatServe = ChatServe.Instance;
            string qq = Console.ReadLine();
            chatServe.StartCreate(qq, 8895);
            while (true)
            {
                chatServe.Update();
            }

        }
    }
}
