using System;

namespace Kean.Domain
{
    /// <summary>
    /// 实体对象
    /// </summary>
    public abstract class Entity<T> where T : struct
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public T Id { get; protected set; }

        /// <summary>
        /// 相等运算符
        /// </summary>
        /// <param name="left">运算符左侧值对象</param>
        /// <param name="right">运算符右侧值对象</param>
        /// <returns>运算结果</returns>
        public static bool operator ==(Entity<T> left, Entity<T> right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }
            return left.Equals(right);
        }

        /// <summary>
        /// 不等运算符
        /// </summary>
        /// <param name="left">运算符左侧值对象</param>
        /// <param name="right">运算符右侧值对象</param>
        /// <returns>运算结果</returns>
        public static bool operator !=(Entity<T> left, Entity<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 指定的值对象是否等于当前值对象
        /// </summary>
        /// <param name="obj">与当前值对象比较的值对象</param>
        /// <returns>比较结果</returns>
        public override bool Equals(object obj)
        {
            if (obj is Entity<T> entity)
            {
                return Id.Equals(entity.Id);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 返回此值对象的哈希代码
        /// </summary>
        /// <returns>32 位有符号整数哈希代码</returns>
        public override int GetHashCode() =>
            HashCode.Combine(Id);
    }
}
