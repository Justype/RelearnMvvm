using Moq;
using RelearnMvvm.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RelearnMvvm.UnitTest.Helpers
{
    public static class PoetrySotrageHelper
    {
        /// <summary>
        /// 数据库中诗词的总数量
        /// </summary>
        public const int PoetryNumber = 139;

        /// <summary>
        /// 获得已经初始化的诗词存储
        /// </summary>
        /// <returns></returns>
        public async static Task<PoetryStorage> GetInitializedPoetryStorageAsync()
        {
            IPreferencesStorage preferencesStorage = new Mock<IPreferencesStorage>().Object;
            PoetryStorage poetryStorage = new PoetryStorage(preferencesStorage);
            await poetryStorage.InitializeAsync();

            return poetryStorage;
        }

        /// <summary>
        /// 删除数据库文件
        /// </summary>
        public static void DeleteDbFile() =>
            File.Delete(PoetryStorage.PoetryDbPath);
    }
}
