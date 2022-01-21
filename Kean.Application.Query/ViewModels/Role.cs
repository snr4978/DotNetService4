namespace Kean.Application.Query.ViewModels
{
    /// <summary>
    /// 角色信息视图
    /// </summary>
    public sealed class Role
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
