using MediatR;

namespace Kean.Domain
{
    /// <summary>
    /// 表示命令
    /// </summary>
    public interface ICommand : IRequest
    {
        // 该接口仅作为一个标识
    }
}
