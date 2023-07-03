using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain
{
    /// <summary>
    /// 命令处理程序
    /// </summary>
    /// <typeparam name="T">命令模型</typeparam>
    public abstract class CommandHandler<T> : ICommandHandler<T> where T : class, ICommand
    {
        private T _command; // 命令实例

        /*
         * 隐藏接口 MediatR.IRequestHandler<T>.Handle，表示命令处理
         * 将实际处理程序转移到 Kean.Domain.ICommandHandler<T>.Handle 方法
         */
        async Task IRequestHandler<T>.Handle(T request, CancellationToken cancellationToken)
        {
            // Command 不一定都继承 Kean.Domain.CommandValidator，如果继承则在此执行 Validate，结果交给实际业务处置，更灵活
            if (request is CommandValidator<T> validator)
            {
                validator.Validation();
                validator.Validate();
            }
            await Handle(_command = request, cancellationToken).ConfigureAwait(false);
        }

        /*
         * 抽象实现接口 Kean.Domain.ICommandHandler<T>.Handle，表示命令处理
         * 覆盖了 MediatR.IRequestHandler<T>.Handle
         */
        public abstract Task Handle(T command, CancellationToken cancellationToken);

        /// <summary>
        /// 命令输出
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="value">输出值</param>
        protected void Output(string property, object value) =>
            typeof(T).GetProperty(property).SetValue(_command, value);
    }
}
