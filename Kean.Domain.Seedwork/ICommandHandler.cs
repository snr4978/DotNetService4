using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain
{
    /// <summary>
    /// 表示命令处理程序
    /// </summary>
    /// <typeparam name="T">命令模型</typeparam>
    internal interface ICommandHandler<T> : IRequestHandler<T> where T : class, ICommand
    {
        /// <summary>
		/// 处理命令
		/// </summary>
		/// <param name="command">命令</param>
        /// <param name="cancellationToken">取消标记</param>
        new Task Handle(T command, CancellationToken cancellationToken);
    }
}
