using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XC_Framework
{
    /// <summary>
    /// 数学工具类
    /// </summary>
    public static class MathUtil
    {
    #region 三角函数
    /// <summary>
    /// 角度转弧度的方法
    /// </summary>
    /// <param name="deg">角度值</param>
    /// <returns>弧度值</returns>
    public static float Deg2Rad(float deg)
        {
            return deg * Mathf.Deg2Rad;
        }
        /// <summary>
        /// 弧度转角度的方法
        /// </summary>
        /// <param name="rad">弧度值</param>
        /// <returns>角度值</returns>
        public static float Rad2Deg(float rad)
        {
            return rad * Mathf.Rad2Deg;
        }
    #endregion

    #region 距离计算相关的
    /// <summary>
    /// 获取XZ平面上 两点的距离
    /// </summary>
    /// <param name="srcPos">点1</param>
    /// <param name="targetPos">点2</param>
    /// <returns>两点之间的距离</returns>
    public static float GetObjDistanceXZ(Vector3 srcPos, Vector3 targetPos)
        {
            srcPos.y = 0;
            targetPos.y = 0;
            return Vector3.Distance(srcPos, targetPos);
        }
        /// <summary>
        /// 判断两点之间距离 是否小于等于目标距离 XZ平面
        /// </summary>
        /// <param name="srcPos">点1</param>
        /// <param name="targetPos">点2</param>
        /// <param name="dis">目标距离</param>
        /// <returns>是否小于等于目标距离</returns>
        public static bool CheckObjDistanceXZ(Vector3 srcPos, Vector3 targetPos, float dis)
        {
            return GetObjDistanceXZ(srcPos, targetPos) <= dis;
        }
        /// <summary>
        /// 获取XY平面上 两点的距离
        /// </summary>
        /// <param name="srcPos">点1</param>
        /// <param name="targetPos">点2</param>
        /// <returns>两点之间的距离</returns>
        public static float GetObjDistanceXY(Vector3 srcPos, Vector3 targetPos)
        {
            srcPos.z = 0;
            targetPos.z = 0;
            return Vector3.Distance(srcPos, targetPos);
        }
        /// <summary>
        /// 判断两点之间距离 是否小于等于目标距离 XY平面
        /// </summary>
        /// <param name="srcPos">点1</param>
        /// <param name="targetPos">点2</param>
        /// <param name="dis">目标距离</param>
        /// <returns>是否小于等于目标距离</returns>
        public static bool CheckObjDistanceXY(Vector3 srcPos, Vector3 targetPos, float dis)
        {
            return GetObjDistanceXY(srcPos, targetPos) <= dis;
        }
    #endregion

    #region 位置判断相关
    /// <summary>
    /// 判断一个世界坐标点是否在屏幕内
    /// </summary>
    /// <param name="pos">世界坐标点</param>
    /// <param name="camera">相机</param>
    /// <returns>在屏幕内返回true，否则返回false</returns>
    public static bool IsInScreen(Vector3 pos, Camera camera)
        {
            Vector3 screenPos = camera.WorldToScreenPoint(pos);
            if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
                return false;
            return true;
        }
        /// <summary>
        /// 判断某一个位置是否在指定位置的扇形范围内
        /// </summary>
        /// <param name="srcPos">扇形中心点位置</param>
        /// <param name="forward">扇形前向</param>
        /// <param name="targetPos">目标位置</param>
        /// <param name="radius">扇形半径</param>
        /// <param name="angle">扇形角度</param>
        /// <returns></returns>
        public static bool IsInSectorRange(Vector3 srcPos,Vector3 forward,  Vector3 targetPos, float radius, float angle)
        {
            return Vector3.Distance(srcPos, targetPos) <= radius && Vector3.Angle(forward, targetPos - srcPos) <= angle / 2f;
        }
    #endregion
        #region 射线检测相关
        /// <summary>
        /// 射线检测 获取一个对象 指定距离 指定层级的
        /// </summary>
        /// <param name="ray">射线</param>
        /// <param name="callBack">回调函数（会把碰到的对象信息上挂在的指定脚本传递出去）</param>
        /// <param name="maxDistance">最大距离</param>
        /// <param name="layerMask">层级筛选</param>
        public static void RayCast<T>(Ray ray, UnityAction<T> callBack, float maxDistance, int layerMask) where T : Component
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, maxDistance, layerMask))
            {
                Type type = typeof(T);
                if (type == typeof(RaycastHit))
                {
                    callBack.Invoke(hitInfo as T);
                }
                else if (type == typeof(GameObject))
                {

                }
                else
                    callBack.Invoke(hitInfo.collider.gameObject.GetComponent<T>());
            }
                
        }

        /// <summary>
        /// 射线检测 获取到多个对象 指定距离 指定层级
        /// </summary>
        /// <param name="ray">射线</param>
        /// <param name="callBack">回调函数（会把碰到的RayCastHit信息传递出去） 每一个对象都会调用一次</param>
        /// <param name="maxDistance">最大距离</param>
        /// <param name="layerMask">层级筛选</param>
        public static void RayCastAll(Ray ray, UnityAction<RaycastHit> callBack, float maxDistance, int layerMask)
        {
            RaycastHit[] hitInfos = Physics.RaycastAll(ray, maxDistance, layerMask);
            for (int i = 0; i < hitInfos.Length; i++)
                callBack.Invoke(hitInfos[i]);
        }

        /// <summary>
        /// 射线检测 获取到多个对象 指定距离 指定层级
        /// </summary>
        /// <param name="ray">射线</param>
        /// <param name="callBack">回调函数（会把碰到的GameObject信息传递出去） 每一个对象都会调用一次</param>
        /// <param name="maxDistance">最大距离</param>
        /// <param name="layerMask">层级筛选</param>
        public static void RayCastAll(Ray ray, UnityAction<GameObject> callBack, float maxDistance, int layerMask)
        {
            RaycastHit[] hitInfos = Physics.RaycastAll(ray, maxDistance, layerMask);
            for (int i = 0; i < hitInfos.Length; i++)
                callBack.Invoke(hitInfos[i].collider.gameObject);
        }

        /// <summary>
        /// 射线检测 获取到多个对象 指定距离 指定层级
        /// </summary>
        /// <param name="ray">射线</param>
        /// <param name="callBack">回调函数（会把碰到的对象信息上依附的脚本传递出去） 每一个对象都会调用一次</param>
        /// <param name="maxDistance">最大距离</param>
        /// <param name="layerMask">层级筛选</param>
        public static void RayCastAll<T>(Ray ray, UnityAction<T> callBack, float maxDistance, int layerMask)
        {
            RaycastHit[] hitInfos = Physics.RaycastAll(ray, maxDistance, layerMask);
            for (int i = 0; i < hitInfos.Length; i++)
                callBack.Invoke(hitInfos[i].collider.gameObject.GetComponent<T>());
        }
        #endregion
        #region 范围检测
        /// <summary>
        /// 盒形范围检测
        /// </summary>
        /// <param name="center">中心点</param>
        /// <param name="rotation">旋转</param>
        /// <param name="halfExtents">半包围盒</param>
        /// <param name="layerMask">层级筛选</param>
        /// <param name="callBack">回调函数（会把碰到的对象信息上依附的脚本传递出去） 每一个对象都会调用一次</param>
        /// <param name="queryTriggerInteraction">查询触发器交互</param>
        public static void OverlapBox<T>(Vector3 center,Quaternion rotation,Vector3 halfExtents,int layerMask,UnityAction<T>callBack,QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) where T : class
        {
            Type type = typeof(T);
            Collider[] colliders = Physics.OverlapBox(center, halfExtents, rotation, layerMask, queryTriggerInteraction);
            if(type == typeof(Collider))
            {
                for (int i = 0; i < colliders.Length; i++)
                    callBack.Invoke(colliders[i] as T);
            }
            else if(type == typeof(GameObject))
            {
                for (int i = 0; i < colliders.Length; i++)
                    callBack.Invoke(colliders[i].gameObject as T);
            }
            else
            {
                for (int i = 0; i < colliders.Length; i++)
                    callBack.Invoke(colliders[i].gameObject.GetComponent<T>());
            }
        }
        /// <summary>
        /// 球形范围检测
        /// </summary>
        /// <typeparam name="T">要获取的组件类型</typeparam>
        /// <param name="center">中心点</param>
        /// <param name="radius">半径</param>
        /// <param name="layerMask">层级筛选</param>
        /// <param name="callBack">回调函数（会把碰到的对象信息上依附的脚本传递出去） 每一个对象都会调用一次</param>
        /// <param name="queryTriggerInteraction">查询触发器交互</param>
        public static void OverlapSphere<T>(Vector3 center, float radius, int layerMask, UnityAction<T> callBack, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) where T : class
        {
            Type type = typeof(T);
            Collider[] colliders = Physics.OverlapSphere(center, radius, layerMask, queryTriggerInteraction);
            if (type == typeof(Collider))
            {
                for (int i = 0; i < colliders.Length; i++)
                    callBack.Invoke(colliders[i] as T);
            }
            else if (type == typeof(GameObject))
            {
                for (int i = 0; i < colliders.Length; i++)
                    callBack.Invoke(colliders[i].gameObject as T);
            }
            else
            {
                for (int i = 0; i < colliders.Length; i++)
                    callBack.Invoke(colliders[i].gameObject.GetComponent<T>());
            }
        }
        /// <summary>
        /// 球形范围检测是否有碰撞
        /// </summary>
        /// <param name="center">中心点</param>
        /// <param name="radius">半径</param>
        /// <param name="layerMask">层级筛选</param>
        /// <param name="queryTriggerInteraction">查询触发器交互</param>
        /// <returns>是否在球形范围内有碰撞</returns>
        public static bool HasOverlapSphere(Vector3 center, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            Collider[] colliders = Physics.OverlapSphere(center, radius, layerMask, queryTriggerInteraction);
            return colliders.Length > 0;
        }
        #endregion
    }
}