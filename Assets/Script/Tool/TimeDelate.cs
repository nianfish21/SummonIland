using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

namespace Tool {


    /// <summary>
    /// 延迟方法
    /// </summary>
    public struct Timer
    {
        //属性跨域调用无GC

        public long Id { get; set; }

        public long Time { get; set; }

        public TaskCompletionSource<bool> Tcs { get; set; }

        public Action CallBack { get; set; }
    }

    /// <summary>
    /// 延迟类
    /// </summary>
    public class DelayEventComponent
    {
        public static DelayEventComponent Instance { get; private set; }

        private readonly Dictionary<long, Timer> timers = new Dictionary<long, Timer>();
        private readonly SortMultiMap<long, long> timeId = new SortMultiMap<long, long>();

        private readonly Queue<long> timeOutTime = new Queue<long>();
        private readonly Queue<long> timeOutTimerIds = new Queue<long>();

        private long minTime;

        public void Awake()
        {
            Instance = this;
        }

        public void Update()
        {
            if (this.timeId.Count == 0) return;

            long nowTime = TimeHelper.Now();

            if (nowTime < minTime) return;

            foreach (KeyValuePair<long, List<long>> kv in this.timeId.GetDictionary())
            {
                long k = kv.Key;
                if (k > nowTime)
                {
                    minTime = k;
                    break;
                }
                this.timeOutTime.Enqueue(k);
            }
            while (timeOutTime.Count > 0)
            {
                long time = this.timeOutTime.Dequeue();
                foreach (long timerId in this.timeId[time])
                {
                    this.timeOutTimerIds.Enqueue(timerId);
                }
                this.timeId.Remove(time);
            }
            while (this.timeOutTimerIds.Count > 0)
            {
                long timerId = this.timeOutTimerIds.Dequeue();
                Timer timer;
                if (!this.timers.TryGetValue(timerId, out timer))
                {
                    continue;
                }
                this.timers.Remove(timerId);
                //task or callback
                if (timer.CallBack != null)
                {
                    timer.CallBack.Invoke();
                }
                if (timer.Tcs != null)
                {


                    timer.Tcs.SetResult(default);

                }
                //TODO 回收timer
            }
        }

        public void RemoveTimer(long timerId)
        {
            Timer timer;
            if (timers.TryGetValue(timerId, out timer))
            {
                timeId.Remove(timer.Time);
                this.timers.Remove(timerId);
            }
        }

        public Timer RegisterTimeCallback(long time, Action action)
        {
            Timer timer = new Timer { Id = IdGenerater.GenerateId(), Time = TimeHelper.Now() + time, CallBack = action };
            this.timers[timer.Id] = timer;
            this.timeId.Add(timer.Time, timer.Id);
            if (timer.Time < this.minTime)
            {
                this.minTime = timer.Time;
            }

            return timer;
        }

        public Task<bool> WaitAsync(long time)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            Timer timer = new Timer { Id = IdGenerater.GenerateId(), Time = TimeHelper.Now() + time, Tcs = tcs };
            this.timers[timer.Id] = timer;
            this.timeId.Add(timer.Time, timer.Id);
            if (timer.Time < this.minTime)
            {
                this.minTime = timer.Time;
            }
            return tcs.Task;
        }
    }

}
