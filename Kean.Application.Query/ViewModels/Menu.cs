namespace Kean.Application.Query.ViewModels
{
    /// <summary>
    /// 菜单信息视图
    /// </summary>
    public sealed class Menu
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public int Parent { get; set; }

        /// <summary>
        /// 标头
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
    }
}
