using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Burst;
using UnityEngine;
using UnityEngine.Events;

namespace XC_Framework
{
    public  class TextUtil
    {
        #region 分割字符串相关
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str">要分割的字符串</param>
        /// <param name="type">分割类型;其中:1-; 2-, 3-% 4-: 5-空格 6-| 7-_ </param>
        /// <returns></returns>
        public static string[] SplitStr(string str, int type)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            switch (type)
            {
                case 1:
                    while (str.IndexOf("；") != -1)
                        str = str.Replace("；", ";");
                    return str.Split(';');
                case 2:
                    while (str.IndexOf("，") != -1)
                        str = str.Replace("，", ",");
                    return str.Split(',');
                case 3:
                    return str.Split('%');
                case 4:
                    while (str.IndexOf("：") != -1)
                        str = str.Replace("：", ":");
                    return str.Split(':');
                case 5:
                    return str.Split(' ');
                case 6:
                    return str.Split('|');
                case 7:
                    return str.Split('_');
                default:
                    return null;
            }
        }
        /// <summary>
        /// 分割字符串并转换为int数组
        /// </summary>
        /// <param name="str">要分割的字符串</param>
        /// <param name="type">分割类型;其中:1-; 2-, 3-% 4-: 5-空格 6-| 7-_ </param>
        /// <returns></returns>
        public static int[] SplitStrToInt(string str, int type)
        {
            string[] strArr = SplitStr(str, type);
            if (strArr == null || strArr.Length == 0)
                return null;
            return Array.ConvertAll(strArr, s => int.Parse(s));
        }
        /// <summary>
        /// 分割字符串为二维int数组
        /// </summary>
        /// <param name="str">要分割的字符串</param>
        /// <param name="typeOne">第一层分割类型;其中:1-; 2-, 3-% 4-: 5-空格 6-| 7-_ </param>
        /// <param name="typeTwo">第二层分割类型;其中:1-; 2-, 3-% 4-: 5-空格 6-| 7-_ </param>
        /// <param name="result">输出的二维int数组</param>
        public static void SplitStrToIntArrTwice(string str,int typeOne,int typeTwo,out int[][] result)
        {
            string[] strArr = SplitStr(str, typeOne);
            if (strArr == null || strArr.Length == 0)
            {
                result = null;
                return;
            }
            result = new int[strArr.Length][];
            for(int i = 0; i < strArr.Length; i++)
            {
                result[i] = SplitStrToInt(strArr[i], typeTwo);
            }
        }
        /// <summary>
        /// 分割字符串为对应的键值对形式 并以int返回
        /// </summary>
        /// <param name="str">要分割的字符串</param>
        /// <param name="typeOne">第一层分割类型</param>
        /// <param name="typeTwo">第二层分割类型</param>
        /// <param name="callBack">回调函数</param>
        public static void SplitStrToIntArrTwice(string str,int typeOne,int typeTwo,UnityAction<int,int> callBack)
        {
            string[] strArr = SplitStr(str, typeOne);
            if (strArr == null || strArr.Length == 0)
                return;
            int[] ints;
            for (int i = 0; i < strArr.Length; i++)
            {
                ints = SplitStrToInt(strArr[i], typeTwo);
                if (ints != null && ints.Length >= 2)
                    callBack?.Invoke(ints[0], ints[1]);
            }
        }

        /// <summary>
        /// 分割字符串为二维字符串数组
        /// </summary>
        /// <param name="str">要分割的字符串</param>
        /// <param name="typeOne">第一层分割类型;其中:1-; 2-, 3-% 4-: 5-空格 6-| 7-_ </param>
        /// <param name="typeTwo">第二层分割类型;其中:1-; 2-, 3-% 4-: 5-空格 6-| 7-_ </param>
        /// <param name="result">输出的二维字符串数组</param>
        public static void SplitStrTwice(string str, int typeOne, int typeTwo, out string[][] result)
        {
            string[] strArr = SplitStr(str, typeOne);
            if (strArr == null || strArr.Length == 0)
            {
                result = null;
                return;
            }
            result = new string[strArr.Length][];
            for (int i = 0; i < strArr.Length; i++)
            {
                result[i] = SplitStr(strArr[i], typeTwo);
            }
        }
        /// <summary>
        /// 分割字符串为对应的键值对形式 并以string返回
        /// </summary>
        /// <param name="str">要分割的字符串</param>
        /// <param name="typeOne">第一层分割类型；其中:1-; 2-, 3-% 4-: 5-空格 6-| 7-_ </param>
        /// <param name="typeTwo">第二层分割类型；其中:1-; 2-, 3-% 4-: 5-空格 6-| 7-_ </param>
        /// <param name="callBack">回调函数</param>
        public static void SplitStrTwice(string str, int typeOne, int typeTwo, UnityAction<string, string> callBack)
        {
            string[] strArr = SplitStr(str, typeOne);
            if (strArr == null || strArr.Length == 0)
                return;
            string[] strs2;
            for (int i = 0; i < strArr.Length; i++)
            {
                strs2 = SplitStr(strArr[i], typeTwo);
                if (strs2 != null && strs2.Length >= 2)
                    callBack?.Invoke(strs2[0], strs2[1]);
            }
        }
        #endregion
        #region 数字转字符串相关
        /// <summary>
        /// 得到指定长度的数字字符串，不足的前面补0
        /// </summary>
        /// <param name="value">数字</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string IntToStr(int value,int len)
        {
            return value.ToString($"D{len}");
        }
        /// <summary>
        /// 让指定浮点数保留小数点后n位
        /// </summary>
        /// <param name="value">具体的浮点数</param>
        /// <param name="len">保留小数点后n位</param>
        /// <returns></returns>
        public static string FloatToStr(float value,int len)
        {
            return value.ToString($"F{len}");
        }
        /// <summary>
        /// 将大数字转换为带单位的字符串表示形式，例如将10000转换为"1万"，将100000000转换为"1亿"
        /// </summary>
        /// <param name="value">要转换的大数字</param>
        /// <returns></returns>
        public static string GetBigDataStr(int value)
        {
            if (value < 10000)
                return value.ToString();
            else if (value < 100000000)
                return BigDataChange(value, 10000, "万", "千");
            else
                return BigDataChange(value, 100000000, "亿", "千万");
        }
        /// <summary>
        /// 根据指定的公司规模，将数字转换为相应的单位
        /// </summary>
        /// <param name="num">要转换的数字</param>
        /// <param name="company">分割单位</param>
        /// <param name="bigCompany">大单位</param>
        /// <param name="littleCompany">小单位</param>
        /// <returns></returns>
        private static string BigDataChange(int num,int company,string bigCompany,string littleCompany)
        {
            resultStr.Clear();
            resultStr.Append(num / company).Append(bigCompany);
            int tmpNum = num % company;
            tmpNum /= (company / 10);
            if (tmpNum != 0)
            {
                resultStr.Append(tmpNum).Append(littleCompany);
            }
            return resultStr.ToString();
        }
        #endregion
        #region 时间相关
        private static StringBuilder resultStr = new StringBuilder("");
        /// <summary>
        /// 将秒数转换为小时、分钟、秒的格式
        /// </summary>
        /// <param name="seconds">秒数</param>
        /// <param name="egZero">是否忽略0</param>
        /// <param name="hourStr">小时</param>
        /// <param name="minuteStr">分钟</param>
        /// <param name="secondStr">秒</param>
        /// <returns></returns>
        public static string SecondsToHMS(int seconds,bool egZero = false,string hourStr = "时",string minuteStr = "分",string secondStr = "秒")
        {
            int h = seconds / 3600;
            int m = (seconds % 3600) / 60;
            int s = seconds % 60;
            resultStr.Clear();
            if(h != 0 || !egZero)
            {
                resultStr.Append(h).Append(hourStr);
            }
            if(m != 0 || !egZero || h != 0)
            {
                resultStr.Append(m).Append(minuteStr);
            }
            if(s != 0 || !egZero || h != 0 || m != 0)
            {
                resultStr.Append(s).Append(secondStr);
            }
            if(resultStr.Length == 0)
            {
                resultStr.Append("0").Append(secondStr);
            }
            return resultStr.ToString();
        }
        /// <summary>
        /// 将秒数转换为时分秒格式，格式为 hh:mm:ss
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="egZero"></param>
        /// <returns></returns>
        public static string SecondsToHMS_Normal(int seconds,bool egZero = false)
        {
            return SecondsToHMS(seconds,egZero,"",":","");
        }
        #endregion
    }
}