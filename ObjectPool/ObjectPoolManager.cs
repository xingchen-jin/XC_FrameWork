using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace XC_Framework
{
    /// <summary>
    /// 通用对象池管理器：按 key 管理多个 <see cref="ObjectPool{T}"/>。
    ///
    /// 适合复用组件对象（AudioSource、子弹、特效、UI 行等）。
    /// 在原来的 Get/Release 基础上补齐了：预 warm、清空、释放、统计和覆盖注册，
    /// 现有 RegisterPool/Get/Release 的签名保持不变。
    /// </summary>
    public class ObjectPoolManager<TKey, T> : IDisposable where T : Component
    {
        private readonly Dictionary<TKey, ObjectPool<T>> pools = new Dictionary<TKey, ObjectPool<T>>();
        private bool disposed;

        /// <summary>已经注册的对象池数量</summary>
        public int PoolCount => pools.Count;

        /// <summary>某个 key 是否已经注册</summary>
        public bool Contains(TKey key)
        {
            return pools.ContainsKey(key);
        }

        /// <summary>
        /// 注册一个对象池。
        /// collectionCheck 打开后，重复 Release 同一个对象会报错，调试时建议开启。
        /// overwrite=true 时会把旧池 Dispose 掉再重新注册。
        /// </summary>
        public void RegisterPool(TKey key, Func<T> createFunc,
                                 Action<T> onGet = null, Action<T> onRelease = null, Action<T> onDestroy = null,
                                 bool collectionCheck = false, int defaultCapacity = 10, int maxSize = 10000,
                                 bool overwrite = false)
        {
            if (createFunc == null)
            {
                Debug.LogError($"[对象池] 注册 {key} 失败：createFunc 为空");
                return;
            }

            if (pools.ContainsKey(key))
            {
                if (!overwrite)
                {
                    Debug.LogWarning($"[对象池] {key} 已存在，无法重复注册");
                    return;
                }
                pools[key].Dispose();
                pools.Remove(key);
            }

            pools.Add(key, new ObjectPool<T>(
                createFunc,
                onGet,
                onRelease,
                onDestroy,
                collectionCheck,
                Mathf.Max(1, defaultCapacity),
                Mathf.Max(1, maxSize)));
        }

        /// <summary>尝试取一个对象；key 不存在时返回 false。</summary>
        public bool TryGet(TKey key, out T obj)
        {
            if (!pools.TryGetValue(key, out ObjectPool<T> pool))
            {
                obj = null;
                return false;
            }
            obj = pool.Get();
            return true;
        }

        /// <summary>取一个对象；池不存在时返回 null。</summary>
        public T Get(TKey key)
        {
            if (!pools.TryGetValue(key, out ObjectPool<T> pool))
            {
                Debug.LogWarning($"[对象池] {key} 不存在，无法获取对象");
                return null;
            }
            return pool.Get();
        }

        /// <summary>归还对象。</summary>
        public void Release(TKey key, T obj)
        {
            if (obj == null) return;
            if (!pools.TryGetValue(key, out ObjectPool<T> pool))
            {
                Debug.LogWarning($"[对象池] {key} 不存在，无法释放对象");
                return;
            }
            pool.Release(obj);
        }

        /// <summary>
        /// 预热：先创建再立即归还，避免第一次使用时集中 Instantiate。
        /// </summary>
        public void Prewarm(TKey key, int count)
        {
            if (!pools.TryGetValue(key, out ObjectPool<T> pool))
            {
                Debug.LogWarning($"[对象池] {key} 不存在，无法预热");
                return;
            }

            int safeCount = Mathf.Max(0, count);
            for (int i = 0; i < safeCount; i++)
            {
                pool.Release(pool.Get());
            }
        }

        /// <summary>销毁某个池里所有未激活对象，池本身保留。</summary>
        public void Clear(TKey key)
        {
            if (pools.TryGetValue(key, out ObjectPool<T> pool))
            {
                pool.Clear();
            }
        }

        /// <summary>销毁所有池里未激活的对象，池本身保留。</summary>
        public void ClearAll()
        {
            foreach (ObjectPool<T> pool in pools.Values)
            {
                pool.Clear();
            }
        }

        /// <summary>注销并释放某个池（包括其内部对象）。</summary>
        public bool Unregister(TKey key)
        {
            if (!pools.TryGetValue(key, out ObjectPool<T> pool))
            {
                return false;
            }
            pool.Dispose();
            pools.Remove(key);
            return true;
        }

        /// <summary>某池当前总对象数（激活 + 未激活）。</summary>
        public int CountAll(TKey key)
        {
            return pools.TryGetValue(key, out ObjectPool<T> pool) ? pool.CountAll : 0;
        }

        /// <summary>某池当前激活对象数。</summary>
        public int CountActive(TKey key)
        {
            return pools.TryGetValue(key, out ObjectPool<T> pool) ? pool.CountActive : 0;
        }

        /// <summary>某池当前未激活对象数。</summary>
        public int CountInactive(TKey key)
        {
            return pools.TryGetValue(key, out ObjectPool<T> pool) ? pool.CountInactive : 0;
        }

        /// <summary>释放所有池。之后的 Get/Release 会失效。</summary>
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            foreach (ObjectPool<T> pool in pools.Values)
            {
                pool.Dispose();
            }
            pools.Clear();
        }
    }
}
