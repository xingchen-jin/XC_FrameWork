using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XC_Framework
{
    /// <summary>
    /// 加密工具类
    /// </summary>
    public class EncryptionUtil
    {
        /// <summary>
        /// 获得随机密钥
        /// </summary>
        /// <returns></returns>
        public static int GetRandomKey()
        {
            return Random.Range(1, 10000) + 5;
        }
        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="value">明文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static int LockValue(int value, int key)
        {
            //主要采用异或加密
            value = value ^ (key % 9);
            value = value ^ 0xAEBE;
            value = value ^ (1 << 5);
            value += key;
            return value;
        }
        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="value">明文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static long LockValue(long value, int key)
        {
            //主要采用异或加密
            value = value ^ (key % 9);
            value = value ^ 0xAEBE;
            value = value ^ (1 << 5);
            value += key;
            return value;
        }
        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="value">密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static int UnLoackValue(int value, int key)
        {
            value -= key;
            value = value ^ (key % 9);
            value = value ^ 0xAEBE;
            value = value ^ (1 << 5);
            return value;
        }
        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="value">密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static long UnLoackValue(long value, int key)
        {
            value -= key;
            value = value ^ (key % 9);
            value = value ^ 0xAEBE;
            value = value ^ (1 << 5);
            return value;
        }
    }
}