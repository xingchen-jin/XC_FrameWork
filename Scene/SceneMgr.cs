using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace XC_Framework
{
    public class SceneMgr : BaseManager<SceneMgr>
    {
        private SceneMgr() { }
        private readonly float _progressThreshold = 0.1f; // 进度阈值，避免频繁触发事件
        #region 公共方法
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="callBack"></param>
        /// <param name="loadSceneMode"></param>
        public void LoadSceneAsync(string sceneName, UnityAction callBack = null,LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            MonoManager.Instance.StartCoroutine(LoadSceneAsyncCoroutine(sceneName, callBack, loadSceneMode));
        }
        private IEnumerator LoadSceneAsyncCoroutine(string sceneName, UnityAction callBack, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            SceneLoadChangeEvent sceneLoadChangeEvent = new SceneLoadChangeEvent(0);
            //每帧检测加载进度并触发事件
            while (!async.isDone)
            {
                //呼叫事件中心发送加载进度
                if (Math.Abs(sceneLoadChangeEvent.progress - async.progress/0.9f) > _progressThreshold)
                {
                    sceneLoadChangeEvent.progress = async.progress/0.9f;
                    EventCenter.EventTrigger<SceneLoadChangeEvent>(sceneLoadChangeEvent);
                }
                yield return null;
            }
            //结束加载传送最后一帧的进度
            sceneLoadChangeEvent.progress = 1;
            EventCenter.EventTrigger<SceneLoadChangeEvent>(sceneLoadChangeEvent);
            callBack?.Invoke();
        }

        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="callBack"></param>   
        /// <param name="loadSceneMode"></param>
        public void LoadScene(string sceneName, UnityAction callBack = null, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(sceneName, loadSceneMode);
            callBack?.Invoke();
        }
        /// <summary>
        /// 异步卸载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="callBack"></param>
        public void UnloadSceneAsync(string sceneName, UnityAction callBack = null)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            callBack?.Invoke();
        }
        public void SetActiveScene(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.IsValid())
            {
                SceneManager.SetActiveScene(scene);
            }
            else
            {
                Debug.LogWarning($"场景 {sceneName} 无效，无法设置为活动场景。");
            }
        }
        public Scene GetActiveScene()
        {
            return SceneManager.GetActiveScene();
        }
        #endregion
    }
}
