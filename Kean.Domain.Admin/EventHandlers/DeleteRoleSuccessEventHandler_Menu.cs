using Kean.Domain.Admin.Events;
using Kean.Domain.Admin.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Admin.EventHandlers
{
    /// <summary>
    /// 删除角色成功时，清理菜单权限信息
    /// </summary>
    public sealed class DeleteRoleSuccessEventHandler_Menu : EventHandler<DeleteRoleSuccessEvent>
    {
        private readonly IRoleRepository _roleRepository; // 角色仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public DeleteRoleSuccessEventHandler_Menu(
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
                await _roleRepository.ClearMenuPermission(item);
            }
        }
    }
}
