using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

namespace XC_Framework.UI
{
    public enum ButtonClickType
    {
        leftClick,   // 左键点击
        rightClick,  // 右键点击
        middleClick, // 中键点击
        leftDoubleClick,  // 左键双击
        rightDoubleClick  // 右键双击
    }
    /// <summary>
    /// 自定义按钮组件，继承自 MonoBehaviour，提供按钮的基本功能和扩展功能。
    /// </summary>
    public class XC_Button : MonoBehaviour,
        IPointerClickHandler,   // 实现点击事件接口
        IPointerEnterHandler, IPointerExitHandler  //悬停接口 
    {
        [Header("按钮组件")]
        [Tooltip("按钮的 Image 组件")]
        private Image buttonImage; // 按钮的 Image 组件
        [Tooltip("按钮的 Text 组件")]
        public TextMeshProUGUI buttonText;   // 按钮的 Text 组件

        [Header("按钮状态颜色")]
        [Tooltip("按钮正常状态的颜色")]
        public Color normalColor = Color.white; // 按钮正常状态的颜色
        [Tooltip("按钮按下状态的颜色")]
        public Color pressedColor = Color.gray; // 按钮按下状态的颜色
        [Tooltip("按钮悬停状态的颜色")]
        public Color hoverColor = Color.yellow; // 按钮悬停状态的颜色
        [Tooltip("按钮悬停时的缩放因子")]
        [SerializeField]private float scaleFactor = 1.1f; // 按钮悬停时的缩放因子

        private bool isPressed = false; // 按钮是否被按下
        private bool isHovered = false; // 鼠标是否悬停在按钮上
        #region 按钮事件
        private UnityEvent onLeftClick; // 按钮左键点击事件
        private UnityEvent onRightClick; // 按钮右键点击事件
        private UnityEvent onMiddleClick; // 按钮中键点击事件
        private UnityEvent onLeftDoubleClick; // 按钮左键双击事件
        private UnityEvent onRightDoubleClick; // 按钮右键双击事件
        #endregion
        #region Unity生命周期
        /// <summary>
        /// Awake 方法在脚本实例被加载时调用，用于初始化按钮组件。
        /// </summary>
        void Awake()
        {
            // 获取按钮的 Image 组件
            buttonImage = GetComponent<Image>();
            // 设置按钮的初始颜色为正常状态颜色
            if (buttonImage != null)
            {
                buttonImage.color = normalColor;
            }
        }
        #endregion
        #region 按钮事件注册方法
        /// <summary>
        /// 注册按钮点击事件监听器。
        /// </summary>
        /// <param name="clickType"></param>
        /// <param name="action"></param>
        public void AddListener(ButtonClickType clickType, UnityAction action)
        {
            switch (clickType)
            {
                case ButtonClickType.leftClick:
                    if (onLeftClick == null) onLeftClick = new UnityEvent();
                    onLeftClick.AddListener(action);
                    break;
                case ButtonClickType.rightClick:
                    if (onRightClick == null) onRightClick = new UnityEvent();
                    onRightClick.AddListener(action);
                    break;
                case ButtonClickType.middleClick:
                    if (onMiddleClick == null) onMiddleClick = new UnityEvent();
                    onMiddleClick.AddListener(action);
                    break;
                case ButtonClickType.leftDoubleClick:
                    if (onLeftDoubleClick == null) onLeftDoubleClick = new UnityEvent();
                    onLeftDoubleClick.AddListener(action);
                    break;
                case ButtonClickType.rightDoubleClick:
                    if (onRightDoubleClick == null) onRightDoubleClick = new UnityEvent();
                    onRightDoubleClick.AddListener(action);
                    break;
            }
        }
        /// <summary>
        /// 移除按钮点击事件监听器。
        /// </summary>
        /// <param name="clickType"></param>
        /// <param name="action"></param>
        public void RemoveListener(ButtonClickType clickType, UnityAction action)
        {
            switch (clickType)
            {
                case ButtonClickType.leftClick:
                    if (onLeftClick != null) onLeftClick.RemoveListener(action);
                    break;
                case ButtonClickType.rightClick:
                    if (onRightClick != null) onRightClick.RemoveListener(action);
                    break;
                case ButtonClickType.middleClick:
                    if (onMiddleClick != null) onMiddleClick.RemoveListener(action);
                    break;
                case ButtonClickType.leftDoubleClick:
                    if (onLeftDoubleClick != null) onLeftDoubleClick.RemoveListener(action);
                    break;
                case ButtonClickType.rightDoubleClick:
                    if (onRightDoubleClick != null) onRightDoubleClick.RemoveListener(action);
                    break;
            }
        }

        #endregion
        #region  按钮事件响应
        /// <summary>
        /// 当鼠标点击按钮时调用的方法。
        /// </summary>
        /// <param name="eventData">指针事件数据</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            //触摸长按已经转成右键，抬手补发的左键直接忽略
            if (eventData.button == PointerEventData.InputButton.Left && TouchLongPressRouter.ConsumeSuppressedLeftClick())
            {
                return;
            }

            // 所有使用 XC_Button 的按钮统一播放 UI 点击音效,导出框架包时删除
            SoundEffectManager.Instance.Play(SoundEffectType.UIClick);

            isPressed = true;
            // 显示效果
            if (buttonImage != null)
            {
                buttonImage.color = pressedColor;
            }
            // 根据点击类型触发相应的事件
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    if (eventData.clickCount >= 2 && onLeftDoubleClick != null)
                    {
                        //如果点击次数大于等于2，并且有注册双击事件，则触发双击事件
                        onLeftDoubleClick?.Invoke();
                    }
                    else
                    {
                        onLeftClick?.Invoke();
                    }
                    break;
                case PointerEventData.InputButton.Right:
                    if (eventData.clickCount >= 2 && onRightDoubleClick != null)
                    {
                        //如果点击次数大于等于2，并且有注册双击事件，则触发双击事件
                        onRightDoubleClick?.Invoke();
                    }
                    else
                    {
                        onRightClick?.Invoke();
                    }
                    break;
                case PointerEventData.InputButton.Middle:
                    onMiddleClick?.Invoke();
                    break;
                
            }
        }
        /// <summary>
        /// 当鼠标悬停在按钮上时调用的方法。
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovered = true;
            //显示效果
            if (buttonImage != null)
            {
                buttonImage.color = hoverColor;
            }
            
        }

        /// <summary>
        /// 当鼠标离开按钮时调用的方法。
        /// </summary>
        /// <param name="eventData"></param>    
        public void OnPointerExit(PointerEventData eventData)
        {
            isHovered = false;
            //显示效果
            if (buttonImage != null)
            {
                buttonImage.color = normalColor;
            }
        }
        #endregion
    }
}