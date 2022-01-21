using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.Repositories
{
    /// <summary>
    /// 表示会话仓库
    /// </summary>
    public interface ISessionRepository
    {
        /// <summary>
        /// 加载索引
        /// </summary>
        /// <returns>会话索引</returns>
        Task<IEnumerable<string>> LoadKeys();

        /// <summary>
        /// 创建会话
        /// </summary>
        /// <param name="key">会话索引</param>
        /// <param name="identity">用户 ID</param>
        Task CreateSession(string key, int identity);

        /// <summary>
        /// 更新会话
        /// </summary>
        /// <param name="key">会话索引</param>
        Task UpdateSession(string key);

        /// <summary>
        /// 删除会话
        /// </summary>
        /// <param name="key">会话索引</param>
        Task RemoveSession(string key);

        /// <summary>
        /// 会话是否过期
        /// </summary>
        /// <param name="key">会话索引</param>
        /// <returns>如果过期，为 true；否则为 false</returns>
        Task<bool> IsTimeout(string key);

        /// <summary>
        /// 密码是否初始化
        /// </summary>
        /// <param name="key">会话索引</param>
        /// <returns>如果密码已初始化，为 true；否则为 false</returns>
        Task<bool> IsPasswordInitial(string key);

        /// <summary>
        /// 密码是否过期
        /// </summary>
        /// <param name="key">会话索引</param>
        /// <returns>如果密码已过期，为 true；否则为 false</returns>
        Task<bool> IsPasswordExpired(string key);

        /// <summary>
        /// 是否拥有权限
        /// </summary>
        /// <param name="key">会话索引</param>
        /// <param name="url">URL</param>
        /// <returns>如果有权限，为 true；否则为 false</returns>
        Task<bool> HasPermission(string key, string url);

        /// <summary>
        /// 会话身份信息
        /// </summary>
        /// <param name="key">会话索引</param>
        /// <returns>用户 ID</returns>
        Task<int?> GetIdentity(string key);

        /// <summary>
        /// 冲突会话
        /// </summary>
        /// <param name="key">会话索引</param>
        /// <returns>冲突的会话索引，如果未设置单点登录模式，则为 null</returns>
        Task<IEnumerable<string>> GetConflicts(string key);

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="key">会话索引</param>
        /// <returns>连接码</returns>
        Task<string> GetConnection(string key);

        /// <summary>
        /// 注册连接
        /// </summary>
        /// <param name="key">会话索引</param>
        /// <param name="id">连接码</param>
        /// <returns>如果注册成功，为 true；否则为 false</returns>
        Task<bool> RegisterConnection(string key, string id);

        /// <summary>
        /// 注销连接
        /// </summary>
        /// <param name="key">会话索引</param>
        /// <param name="id">连接码</param>
        /// <returns>如果注销成功，为 true；否则为 false</returns>
        Task<bool> UnregisterConnection(string key, string id);
    }
}
