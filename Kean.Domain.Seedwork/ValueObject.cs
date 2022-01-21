using System.Linq;

namespace Kean.Domain
{
    /// <summary>
    /// 值对象
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// 相等运算符
        /// </summary>
        /// <param name="left">运算符左侧值对象</param>
        /// <param name="right">运算符右侧值对象</param>
        /// <returns>运算结果</returns>
        public static bool operator ==(ValueObject left, ValueObject right)
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
        public static bool operator !=(ValueObject left, ValueObject right)
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
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            else
            {
                return ReferenceEquals(this, obj) || GetType().GetProperties().All(p => p.GetValue(this) == p.GetValue(obj));
            }
        }

        /// <summary>
        /// 返回此值对象的哈希代码
        /// </summary>
        /// <returns>32 位有符号整数哈希代码</returns>
        public override int GetHashCode() =>
            GetType().GetProperties().Select(p =>
            {
                var v = p.GetValue(this);
                return p == null ? 0 : p.GetHashCode();
            }).Aggregate((a, s) => a ^ s);
    }
}
