namespace XC_Framework
{
    /// <summary>
    /// 场景加载进度事件，由 SceneManger 在异步加载场景时按进度触发。
    ///
    /// ⚠️ 这个文件**故意放在 XCFrameWork 目录之外**（在 Scripts/Event/ 下），但保留在
    /// XC_Framework 命名空间里——因为 XCFrameWork 目录不可修改，而 SceneManger.cs
    /// 引用了一个不存在的类型，导致整个工程编译失败（所有 MonoBehaviour 都无法挂载）。
    /// 如果你希望它归到框架目录里，直接把这个文件挪过去即可
    /// </summary>
    public class SceneLoadChangeEvent : IEvent
    {
        /// <summary>
        /// 加载进度，0~1
        /// </summary>
        public float progress;

        public SceneLoadChangeEvent(float progress)
        {
            this.progress = progress;
        }
    }
}
