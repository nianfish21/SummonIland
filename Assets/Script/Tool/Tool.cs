using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;  //系统进程 事件日志 和性能计数器进行交互

using System.Collections.Concurrent;
using System.Threading;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;


namespace Tool {

    public static class Tool
    {

        /*

        键值对的拆分

           private const char colon=';';
           private const char equals = '=';
           public IEnumerator<global::ETHotfix.Struct_Property> GetKeyValue(string data)
           {
               string[] datas = data.Split(colon);
               foreach (string keyvalue in datas)
               {
                   string[] rate = keyvalue.Split(equals);
                   int key = int.Parse(rate[0].Trim());
                   int value = int.Parse(rate[1].Trim());

                   var pair = new global::ETHotfix.Struct_Property();
                   pair.Value = value;
                   pair.PropID = key;
                   yield return pair;
               }
           }




        // 编辑器编写带提示的按钮

            static GUIContent gc_ButtonGenerateMeshBakers = new GUIContent("Generate Mesh Bakers", "Organize a TextureBakers objects to be combined into groups and generate a MeshBaker child for each group.");


        //结构体
        Pair<ConsignmentMsg.Struct_Role_Status, int>

        //取值只读
            public bool isconnected => isConnected;

        接收端，阻塞接收回包 （看了半天才看懂是在线程里阻塞运行的)
        socket.ReceiveAsync



        方法变量的设置方法

            private Action<int> readCallback;

            public event Action<int> ReadCallback
            {
                add
                {
                    this.readCallback += value;
                }
                remove
                {
                    this.readCallback -= value;
                }
            }


        partial 拆分类的定义可以在多个文件里定义
        public partial class Request{}


        protobuf迭代器
        Google.Protobuf.Collections


        全局给类加方法
        public static void Set(this Type self, int x)
        {
            self.PositionX = x;
        }


        //----------申请安卓权限---------
            /// <summary>
            /// 申请多个权限
            /// </summary>
            public void RequestPermission(string permission, Action callback = null)
            {


                PermissionCallbacks m_PermissionCallbacks;
                //申请回调
                m_PermissionCallbacks = new PermissionCallbacks();
                m_PermissionCallbacks.PermissionDenied += (str) =>
                {
                    Debug.Log("没通过：" + permission);
                };
                m_PermissionCallbacks.PermissionGranted += (str) =>
                {
                    Debug.Log("通过：" + permission);
                    if (Permission.HasUserAuthorizedPermission(permission))
                    {
                        if (callback != null) callback();
                    }
                };
                m_PermissionCallbacks.PermissionDeniedAndDontAskAgain += (str) =>
                {
                    Debug.Log("没通过不再询问：" + permission);
                };

                //执行申请多个权限
                Permission.RequestUserPermission(permission, m_PermissionCallbacks);

            }


             */



        private static string getMac()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                //Debug.Log("----------adapter Name:" + adapter.Name);    //网卡适配名称：“本地连接”
                //Debug.Log("adapter Description:" + adapter.Description);  //适配器描述信息
                //IPInterfaceProperties ip = adapter.GetIPProperties();     //IP配置信息
                //if (ip.UnicastAddresses.Count > 0)
                //{
                //    Debug.Log("UnicastAddresses Address:" + ip.UnicastAddresses[0].Address.ToString());  //IP地址
                //    Debug.Log("UnicastAddresses IPv4Mask:" + ip.UnicastAddresses[0].IPv4Mask.ToString()); //子网掩码
                //}
                //if (ip.GatewayAddresses.Count > 0)
                //    Debug.Log("GatewayAddresses Address:" + ip.GatewayAddresses[0].Address.ToString());  //默认网关
                //if (ip.DnsAddresses.Count > 0)
                //{
                //    Debug.Log("DnsAddresses DnsAddresses0:" + ip.DnsAddresses[0].ToString());      //首选DNS服务器地址
                //    if (ip.DnsAddresses.Count > 1)
                //        Debug.Log("DnsAddresses DnsAddresses1:" + ip.DnsAddresses[1].ToString()); //备用DNS服务器地址
                //}

                if (adapter.NetworkInterfaceType.ToString().Equals("Ethernet") || adapter.NetworkInterfaceType.ToString().Equals("wlan0"))
                {
                    //获取物理地址
                    byte[] macBytes = adapter.GetPhysicalAddress().GetAddressBytes();

                    if (macBytes != null)
                    {
                        StringBuilder macAddressBuilder = new StringBuilder();
                        foreach (byte b in macBytes)
                        {
                            //处理为二进制
                            macAddressBuilder.Append(string.Format("{0:X0002}", b) + ":");
                        }
                        if (macAddressBuilder.Length > 0)
                        {
                            macAddressBuilder.Remove(macAddressBuilder.Length - 1, 1);
                        }
                        string macAddress = macAddressBuilder.ToString();
                        //Debug.Log($"macAddress:{macAddress} + org {macBytes.Length} + hash {adapter.GetHashCode()} "); // mac地址
                        return macAddress;
                    }
                }

            }
            return "";

            //Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ：Wifi链接

            //Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ：移动网络

            //Application.internetReachability == NetworkReachability.NotReachable ：没有网络

        }


        /// <summary>
        /// 异步方法
        /// </summary>
        /// <param name="callBack"></param>
        public static async void asyncCallBack(Action callBack)
        {
            if (callBack == null) return;
            Task task = new Task(() =>
            {
                callBack();
            });
            task.Start();
            await task;

        }


        public class TimeCheck
        {

            public System.Diagnostics.Stopwatch stopwatch = new Stopwatch();

            public string output = "";

            public int index = 0;


            public TimeCheck()
            {
            }



            public void Start()
            {

                this.stopwatch.Reset();

                this.stopwatch.Start();

            }


            public void Stop(string info)
            {

                this.stopwatch.Stop();

                TimeSpan timespan = stopwatch.Elapsed;

                double milliseconds = timespan.TotalMilliseconds;

                this.output = info + "   耗时   " + milliseconds.ToString() + "   ms.";

                this.index = this.index + 1;

                DebugLog(output); ;

            }



            public void Reset()
            {

                this.stopwatch.Reset();

            }



        }


        /// <summary>
        /// --------------unity 自带存储 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void PlayerPrefs_SaveString(string key, string value)
        {

            PlayerPrefs.SetString(key, value);
        }

        /// <summary>
        /// unity 自带存储取出------------
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string PlayerPrefs_GetString(string key)
        {

            var s = PlayerPrefs.GetString(key, string.Empty);
            return s;
        }



        /// <summary>
        /// -------------------------线程任务分配---------------------------
        /// </summary>
        public class OneThreadSynchronizationContext : SynchronizationContext
        {
            public static OneThreadSynchronizationContext Instance { get; } = new OneThreadSynchronizationContext();

            private readonly int mainThreadId = Thread.CurrentThread.ManagedThreadId;

            private readonly ConcurrentQueue<Action> queue = new ConcurrentQueue<Action>();

            private Action a;

            public void Update()
            {
                while (true)
                {
                    if (!this.queue.TryDequeue(out a)) return;

                    a();
                }
            }
            public override void Post(SendOrPostCallback callback, object state)
            {
                if (Thread.CurrentThread.ManagedThreadId == this.mainThreadId)
                {
                    callback(state);
                    return;
                }

                this.queue.Enqueue(() => { callback(state); });
            }

        }


        /// <summary>
        /// 单例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class single<T> where T : single<T>, new()
        {
            private static T _Instance;
            public static T ins
            {
                get
                {
                    if (_Instance == null)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new T();
                            //  singleMgr.singles.Add(_Instance);
                        }
                    }
                    return _Instance;
                }
            }
        }


        #region 迭代器解释

        /// <summary>
        /// 迭代器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface IList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
        {
            T this[int index] { get; set; }

            int IndexOf(T item);

            void Insert(int index, T item);

            void RemoveAt(int index);
        }


        /// <summary>
        /// 迭代器本器 
        /// </summary>
        public interface IEnumerable
        {
            IEnumerator GetEnumerator();
        }
        #endregion



        /// <summary>
        /// 检查多个包含文字
        /// </summary>
        /// <param name="go"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool ContainFormats(this string go, params string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (go.Contains(args[i]))
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 打印日志
        /// </summary>
        public static void DebugLog(string value)
        {
            UnityEngine.Debug.Log(value);
        }

    }

}




