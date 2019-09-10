
## 使用方法




### 1.网络消息自定义类

都必须继承KGNetData 然后打上可序列化标签[Serializable]

```
[Serializable]
public class NetData : KGNetData {
    public string dataname;
}
```



### 2.创建客户端/服务器端

```
 //  KGSocketClient<KGNetSession<KGNetData>, KGNetData> kg =    new KGSocketClient<KGNetSession<NetData>, NetData>();
            KGSocketServe<KGNetSession<KGNetData>, KGNetData> kg=    new KGSocketServe<KGNetSession<NetData>,NetData>();

       //都是调用这个创建
            kg.StartCreate("127.0.0.1", 8897);
```

### 3.发送数据

就是调用KGNetSession里面的SendData(T)

```
kg.Client.SendData(new KGNetData { dataname = "123456" });
```



### 4.接收网络消息 

这里留了一个回调事件和回调函数OnReciveDataEvent/OnReciveData  

重写OnReciveData  就好了  如果别的要加事件可以往OnReciveDataEvent加

```
 protected override void OnReciveData(T data)
        {
            OnReciveDataEvent?.Invoke(data);
            ("接收到了一条消息："+data).KLog();
        }

```

### 5.打印数据的

在KGBaseNet里面的   这里是给 在另外一些 Console.WriteLine打印不了留出来用的 

```

        public void SetLog(Action<string, LogLevel> LogEvent,bool run=true)
        {
            LogEvent.SetLog(run);
        }
```

### -------------------------------------------------------------------------------------------------------------

### 更新计划

可能没打算写框架  应该写成工具那样自己拿来用

- 增加简易的心跳包工具
- 示例增加简易的断线重连 
- unity 自动烘焙的寻路数据上传 
