using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XC_Framework
{
    public class MusicManager : BaseManager<MusicManager>
    {
    MusicManager() { }
    private AudioSource audioSource;
    private readonly String path = "Music/";
    private float volume = 0.3f;
    public float Volume => volume;
    private float maxVolume = 0.5f;
    public float MaxVolume => maxVolume;

    #region 公有方法
    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="name">音乐文件名</param>
    public void PlayMusic(String name)
        {
            if(audioSource == null)
            {
                GameObject obj = new GameObject();
                obj.name = "Music";
                GameObject.DontDestroyOnLoad(obj);
                audioSource = obj.AddComponent<AudioSource>();
            }

            //通过ResManager加载音乐资源
            audioSource.clip = ResManager.Instance.Load<AudioClip>( string.Format("{0}{1}", path, name));
            audioSource.volume = volume;
            audioSource.loop = true;
            audioSource.Play();
        }
        /// <summary>
        /// 停止音乐
        /// </summary>
        public void StopMusic()
        {
            if(audioSource == null)
            {
                return;
            }
            audioSource.Stop();
        }
        /// <summary>
        /// 暂停音乐
        /// </summary>
        public void PauseMusic()
        {
            if(audioSource == null)
            {
                return;
            }
            audioSource.Pause();
        }
        /// <summary>
        /// 改变音乐音量
        /// </summary>
        /// <param name="v">音量值(0-1)</param>
        /// <param name="isPercentage">是否为百分比</param>
        public void ChangeMusicValue(float v,bool isPercentage = true)
        {
            if(isPercentage)
            {
                v = v * maxVolume;
            }
            //限制音量范围
            volume = Mathf.Clamp(v, 0, maxVolume);
            if(audioSource == null)
            {
                return;
            }
            audioSource.volume = volume;
        }
        /// <summary>
        /// 获取当前音乐音量
        /// </summary>
        /// <returns></returns>
        public float GetMusicValue()
        {
            return volume;
        }
        /// <summary>
        /// 获取当前音乐音量百分比
        /// </summary>
        /// <returns></returns>
        public float GetMusicaValuePercentage()
        {
            return volume / maxVolume;
        }
    #endregion

    #region 私有方法
    #endregion
    }
}