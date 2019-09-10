## 更新日志

v0.1.0   添加了一个心跳包工具，修复了一些小问题，服务端添加了添加/移除客户端事件，示例增加了心跳包的使用     *2019.9.10*

v0.0.2   增加了一个使用示例   数据处理     *2019.8.21*

v0.0.1   修复分包BUG     *2019.8.21*

v0.0.0   基础的功能创建TCP客户/服务端端   数据处理     *2019.7.29*
## -------------------------------------------------------------------------------------------------------------

## 使用方法


## KGSocket

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

## Tool 

### 1.心跳包工具

- #### 开启心跳包

  - ```

   new KGHeartBeatManage<T, R>(T=> { T就是传进来的会话管理 这里主要做发送心跳包的 },T=> 
            {第二个委托 就是超过心跳包超时一定次数触发，直接调用会话管理关掉就好}
            ,每多少毫秒触发一次 发送心跳包和检测心跳包);
     ```

- #### 添加 删除 监听中的心跳包
 - ```
   KGHeartBeatManage.AddConnectDic/RemoveConnectDic(T obj)   T就是KGHeartBeatManage定义的T 会话管理类
   ```

## -------------------------------------------------------------------------------------------------------------

### 更新计划

可能没打算写框架  应该写成工具那样自己拿来用

- 增加简易的心跳包工具 √ 2019.9.10
- 示例增加简易的断线重连 
- unity 自动烘焙的寻路数据上传 
