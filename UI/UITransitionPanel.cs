using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XC_Framework.UI
{
    /// <summary>
    /// UI过渡面板,继承此类的面板在显示和隐藏时会有过渡动画效果
    /// 例如淡入淡出
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public  class UITransitionPanel : UIBasePanel
    {
        protected CanvasGroup canvasGroup;
        [Header("过渡参数")]
        [SerializeField] private float fadeInDuration = 0.1f; // 淡入持续时间
        [SerializeField] private float fadeOutDuration = 0.5f; // 淡出持续时间

        public override void ShowMe()
        {
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
        public override void HideMe()
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
        private IEnumerator FadeIn()
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
                yield return null;
            }
            canvasGroup.alpha = 1f; // 确保最终alpha为1
            OnFadeInComplete(); // 调用过渡完成回调
        }
        private IEnumerator FadeOut()
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeOutDuration));
                yield return null;
            }
            canvasGroup.alpha = 0f; // 确保最终alpha为0
            OnFadeOutComplete(); // 调用过渡完成回调
        }
        #region Unity生命周期
        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                //预制体上忘了挂 CanvasGroup 就补一个。
                //不补的话过渡动画（alpha）和交互控制（interactable/blocksRaycasts）全都会空引用
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
        #endregion
        //留给子类重写的过渡完成回调
        protected virtual void OnFadeInComplete()
        {
        }
        protected virtual void OnFadeOutComplete()
        {
        }
        /// <summary>
        /// 每次显示（SetActive(true)）都恢复交互，
        /// 防止上一次禁用交互后没恢复（比如取消确认框、或者面板隐藏时正处于禁用状态）
        /// </summary>
        private void OnEnable()
        {
            EnablePanelInteraction();
        }
        #region 面板交互控制
        /// <summary>
        /// 禁用面板交互。
        /// blocksRaycasts 必须一起关：interactable 只影响 Selectable（原生Button等），
        /// 像 XC_Button 这种自己实现 IPointerClickHandler 的组件不吃这一套，
        /// 不关射线的话子按钮照样能点
        /// </summary>
        protected  void DisablePanelInteraction()
        {
            if (canvasGroup == null) return;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        /// <summary>
        /// 启用面板交互
        /// </summary>
        protected void EnablePanelInteraction()
        {
            if (canvasGroup == null) return;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        #endregion
    }
}
