using Kean.Domain.Basic.Events;
using Kean.Domain.Basic.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Basic.EventHandlers
{
    /// <summary>
    /// 删除角色成功时，清理用户角色
    /// </summary>
    public sealed class DeleteRoleSuccessEventHandler_User : EventHandler<DeleteRoleSuccessEvent>
    {
        private readonly IRoleRepository _roleRepository; // 角色仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public DeleteRoleSuccessEventHandler_User(
            IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(DeleteRoleSuccessEvent @event, CancellationToken cancellationToken)
        {
            foreach (var item in @event.Id)
            {
                await _roleRepository.ClearUserRole(item);
            }
        }
    }
}
