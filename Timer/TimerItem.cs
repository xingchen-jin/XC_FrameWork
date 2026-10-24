using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XC_Framework
{
    public class TimerItem
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public int keyID;
        /// <summary>
        /// 计时器结束时的回调
        /// </summary>
        public UnityAction overCallBack;
        /// <summary>
        /// 计时器进行中的回调
        /// </summary>
        public UnityAction callBack;
        /// <summary>
        /// 显示计时器的总时间  单位：1s = 1000ms
        /// </summary>
        public int allTime;
        /// <summary>
        /// 显示计时器的间隔时间  单位：1s = 1000ms
        /// </summary>
        public int intervalTime;
        /// <summary>
        /// 显示计时器的最大时间  单位：1s = 1000ms
        /// </summary>
        public int maxAllTime;
        /// <summary>
        /// 显示计时器的最大间隔时间  单位：1s = 1000ms
        /// </summary>
        public int maxIntervalTime;
        /// <summary>
        /// 是否正在计时
        /// </summary>
        public bool isRuning;
        /// <summary>
        /// 初始化计时器信息
        /// </summary>
        /// <param name="keyID">唯一ID</param>
        /// <param name="allTime">总时间</param>
        /// <param name="overCallBack">结束回调</param>
        /// <param name="intervalTime">间隔时间</param>
        /// <param name="callBack">进行中回调</param>
        public void InitInfo(int keyID, int allTime, UnityAction overCallBack, int intervalTime = 0, UnityAction callBack = null)
        {
            this.keyID = keyID;
            this.maxAllTime = this.allTime = allTime;
            this.overCallBack = overCallBack;
            this.maxIntervalTime = this.intervalTime = intervalTime;
            this.callBack = callBack;
            this.isRuning = true;
        }
        public void ResetTimer()
        {
            allTime = maxAllTime;
            intervalTime = maxIntervalTime;
            this.isRuning = true;
        }
        public void ResetInfo()
        {
            keyID = 0;
            allTime = 0;
            intervalTime = 0;
            maxAllTime = 0;
            maxIntervalTime = 0;
            overCallBack = null;
            callBack = null;
            isRuning = false;
        }


    }
}