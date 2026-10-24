using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace XC_Framework
{
    /// <summary>
    /// 计时器管理器
    /// </summary>
    public class TimerManager : BaseManager<TimerManager>
    {
        private int TIMER_KEY = 0;
        private Dictionary<int, TimerItem> timerDic = new Dictionary<int, TimerItem>();
        private Dictionary<int, TimerItem> realTimerDic = new Dictionary<int, TimerItem>();
        private List<TimerItem> delList = new List<TimerItem>();
        private Coroutine timer;
        private Coroutine realTimer;

        private float intervalTime = 0.1f;
        /// <summary>
        /// 默认开启计时器管理器
        /// </summary>
        private void TimeManager()
        {
            Start();
        }
        /// <summary>
        /// 开启计时器管理器
        /// </summary>
        public void Start()
        {
            timer = MonoManager.Instance.StartCoroutine(StartTiming(false, timerDic));
            realTimer = MonoManager.Instance.StartCoroutine(StartTiming(true, realTimerDic));
        }
        /// <summary>
        /// 关闭计时器管理器
        /// </summary>
        public void Stop()
        {
            MonoManager.Instance.StopCoroutine(timer);
            MonoManager.Instance.StopCoroutine(realTimer);
        }
        IEnumerator StartTiming(bool isRealTime, Dictionary<int, TimerItem> dic)
        {
            while (true)
            {
                if (isRealTime)
                    yield return new WaitForSecondsRealtime(intervalTime);
                else
                    yield return new WaitForSeconds(intervalTime);
                foreach (var item in dic)
                {
                    if (item.Value.isRuning)
                    {
                        item.Value.allTime -= (int)(intervalTime * 1000);
                        if(item.Value.callBack != null)
                        {
                            item.Value.intervalTime -= (int)(intervalTime * 1000);
                            if (item.Value.intervalTime <= 0)
                            {
                                item.Value.callBack?.Invoke();
                                item.Value.intervalTime = item.Value.maxIntervalTime;
                            }
                        }
                        if (item.Value.allTime <= 0)
                        {
                            item.Value.overCallBack?.Invoke();
                            delList.Add(item.Value);
                        }
                    }
                }
                //移除计时器
                foreach (var item in delList)
                {
                    dic.Remove(item.keyID);
                }
                delList.Clear();
            }
        }
        /// <summary>
        /// 添加计时器
        /// </summary>
        /// <param name="allTime">总时间</param>
        /// <param name="overCallBack">总时间结束后的回调</param>
        /// <param name="intervalTime">间隔时间</param>
        /// <param name="callBack">间隔时间结束后的回调</param>
        /// <param name="isRealTime">是否不受Time.timeScale影响</param>
        /// <returns></returns>
        public int AddTimer(int allTime, UnityAction overCallBack, int intervalTime = 0, UnityAction callBack = null, bool isRealTime = false)
        {
            TimerItem item = new TimerItem();
            item.InitInfo(TIMER_KEY, allTime, overCallBack, intervalTime, callBack);
            if (isRealTime)
                realTimerDic.Add(TIMER_KEY, item);
            else
                timerDic.Add(TIMER_KEY, item);
            return TIMER_KEY++;
        }
        /// <summary>
        /// 移除计时器
        /// </summary>
        /// <param name="keyID"></param>
        public void RemoveTimer(int keyID)
        {
            if (timerDic.ContainsKey(keyID))
            {
                timerDic.Remove(keyID);
            }
            else if (realTimerDic.ContainsKey(keyID))
            {
                realTimerDic.Remove(keyID);
            }
        }
        /// <summary>
        /// 重置计时器
        /// </summary>
        /// <param name="keyID"></param>
        public void ResetTimer(int keyID)
        {
            if (timerDic.ContainsKey(keyID))
            {
                timerDic[keyID].ResetTimer();
            }
            else if (realTimerDic.ContainsKey(keyID))
            {
                realTimerDic[keyID].ResetTimer();
            }
        }
        /// <summary>
        /// 开启计时器
        /// </summary>
        /// <param name="keyID"></param>
        public void StartTimer(int keyID)
        {
            if (timerDic.ContainsKey(keyID))
            {
                timerDic[keyID].isRuning = true;
            }
            else if (realTimerDic.ContainsKey(keyID))
            {
                realTimerDic[keyID].isRuning = true;
            }
        }
        /// <summary>
        /// 停止计时器
        /// </summary>
        /// <param name="keyID"></param>
        public void StopTimer(int keyID)
        {
            if (timerDic.ContainsKey(keyID))
            {
                timerDic[keyID].isRuning = false;
            }
            else if (realTimerDic.ContainsKey(keyID))
            {
                realTimerDic[keyID].isRuning = false;
            }
        }
    }
}
