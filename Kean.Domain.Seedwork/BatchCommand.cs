using System.Collections.Generic;

namespace Kean.Domain
{
    /// <summary>
    /// 批量命令
    /// </summary>
    public sealed class BatchCommand : ICommand
    {
        /// <summary>
        /// 初始化 Kean.Domain.BatchCommand 类的新实例
        /// </summary>
        /// <param name="commands">命令</param>
        public BatchCommand(IEnumerable<ICommand> commands) =>
            Commands = commands;

        /// <summary>
        /// 初始化 Kean.Domain.BatchCommand 类的新实例
        /// </summary>
        /// <param name="commands">命令</param>
        public BatchCommand(params ICommand[] commands) =>
            Commands = commands;

        /// <summary>
        /// 命令
        /// </summary>
        internal IEnumerable<ICommand> Commands { get; }
    }
}
