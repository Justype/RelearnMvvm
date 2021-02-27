using RelearnMvvm.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RelearnMvvm.Services
{
    /// <summary>
    /// 诗词存储
    /// </summary>
    public class PoetryStorage : IPoetryStorage
    {
        #region Name & Paths
        /// <summary>
        /// 数据库名
        /// </summary>
        private const string DbName = "poetrydb.sqlite3";

        /// <summary>
        /// 资源文件的路径
        /// </summary>
        private const string AssetDbName = nameof(RelearnMvvm) + ".Assets." + DbName;

        /// <summary>
        /// 要部署的数据库路径
        /// </summary>
        public static readonly string PoetryDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DbName);
        //public static readonly string PoetryDbPath = Path.Combine(FileSystem.AppDataDirectory, DbName);
        #endregion

        private SQLiteAsyncConnection _connection;
        /// <summary>
        /// 数据库连接
        /// </summary>
        private SQLiteAsyncConnection Connection => _connection ?? (_connection = new SQLiteAsyncConnection(PoetryDbPath));

        /// <summary>
        /// Preference 存储
        /// </summary>
        private IPreferencesStorage _preferencesStorage;

        public async Task InitializeAsync()
        {
            using (var dbFileStream = new FileStream(PoetryDbPath, FileMode.Create))
            using (var dbAssetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(AssetDbName))
            {
                await dbAssetStream.CopyToAsync(dbFileStream);
            }

            _preferencesStorage.Set(PoetryStorageConstants.VersionKey, PoetryStorageConstants.Version);
        }

        public bool IsInitialized()
        {
            return PoetryStorageConstants.Version == _preferencesStorage.Get(PoetryStorageConstants.VersionKey, PoetryStorageConstants.NoVersion);
        }

        public Task<List<Poetry>> GetPeotriesAsync(Expression<Func<Poetry, bool>> where, int skip, int take) =>
            Connection.Table<Poetry>().Where(where).Skip(skip).Take(take).ToListAsync();

        public Task<Poetry> GetPeotryAsync(int id) =>
            Connection.Table<Poetry>().FirstOrDefaultAsync(p => p.Id == id);

        /// <summary>
        /// 关闭 数据存储 连接
        /// </summary>
        /// <returns></returns>
        public Task CloseAsync() => Connection.CloseAsync();

        public PoetryStorage(IPreferencesStorage preferencesStorage)
        {
            _preferencesStorage = preferencesStorage;
        }
    }
}
