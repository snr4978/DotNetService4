namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// 表示数据库对象的附加标签
    /// 当进行联合查询时，可能连接多个相同的数据库对象，这时需要附加标签来区分
    /// </summary>
    public interface ITag
    {
    }
}
