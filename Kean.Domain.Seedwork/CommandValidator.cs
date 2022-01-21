using FluentValidation;
using FluentValidation.Results;

namespace Kean.Domain
{
    /// <summary>
    /// 验证
    /// </summary>
    public abstract class CommandValidator<T> : AbstractValidator<T>, ICommand where T : class, ICommand
    {
        /// <summary>
        /// 获取验证结果
        /// </summary>
        public virtual ValidationResult ValidationResult { get; protected set; }

        /// <summary>
        /// 验证项
        /// </summary>
        internal protected abstract void Validation();

        /// <summary>
        /// 命令验证
        /// </summary>
        internal void Validate() => 
            ValidationResult = Validate(this as T);
    }
}
