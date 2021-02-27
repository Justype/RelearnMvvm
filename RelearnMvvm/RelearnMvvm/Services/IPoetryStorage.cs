using RelearnMvvm.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RelearnMvvm.Services
{
    /// <summary>
    /// 诗词存储接口
    /// </summary>
    public interface IPoetryStorage
    {
        /// <summary>
        /// 初始化 诗词存储
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// 判断是否初始化
        /// </summary>
        /// <returns></returns>
        bool IsInitialized();

        /// <summary>
        /// 获取一个诗词
        /// </summary>
        /// <param name="id">诗词Id</param>
        /// <returns>诗词</returns>
        Task<Poetry> GetPeotryAsync(int id);

        /// <summary>
        /// 获取满足条件的诗词集合
        /// </summary>
        /// <param name="where">查询语句</param>
        /// <param name="skip">跳过的数量</param>
        /// <param name="take">返回结果的数量</param>
        /// <returns>诗词集合</returns>
        Task<List<Poetry>> GetPeotriesAsync(Expression<Func<Poetry,bool>> where, int skip, int take);
    }

    /// <summary>
    /// 诗词存储 常量
    /// </summary>
    public static class PoetryStorageConstants
    {
        /// <summary>
        /// 诗词数据库版本号
        /// </summary>
        public const int Version = 1;

        /// <summary>
        /// 数据库版本的 Key
        /// </summary>
        public const string VersionKey = nameof(PoetryStorageConstants) + "." + nameof(Version);

        /// <summary>
        /// 没有数据库 值为-1
        /// </summary>
        public const int NoVersion = -1;
    }
}
