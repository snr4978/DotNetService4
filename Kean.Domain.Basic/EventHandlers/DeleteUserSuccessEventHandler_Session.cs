using Kean.Domain.Basic.Events;
using Kean.Domain.Basic.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Basic.EventHandlers
{
    /// <summary>
    /// 删除用户成功时，清理会话及相关缓存
    /// </summary>
    public sealed class DeleteUserSuccessEventHandler_Session : EventHandler<DeleteUserSuccessEvent>
    {
        private readonly IUserRepository _userRepository; // 用户仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public DeleteUserSuccessEventHandler_Session(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(DeleteUserSuccessEvent @event, CancellationToken cancellationToken)
        {
            foreach (var item in @event.Id)
            {
                await _userRepository.ClearSession(item);
            }
        }
    }
}
