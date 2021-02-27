using Moq;
using NUnit.Framework;
using RelearnMvvm.Services;
using RelearnMvvm.UnitTest.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RelearnMvvm.UnitTest.Services
{
    /// <summary>
    /// 诗词存储测试
    /// </summary>
    public class PoetryStorageTest
    {

        /// <summary>
        /// 删除 数据库文件
        /// </summary>
        [SetUp, TearDown]
        public static void DeleteDatabaseFile() =>
            PoetrySotrageHelper.DeleteDbFile();

        /// <summary>
        /// 诗词存储是否可以成功初始化
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestInitializeAsync()
        {
            Assert.IsFalse(File.Exists(PoetryStorage.PoetryDbPath));

            var preferencesStorageMock = new Mock<IPreferencesStorage>();  // 生成Mock对象
            IPreferencesStorage mockPreferencesSotrage = preferencesStorageMock.Object;

            PoetryStorage poetryStorage = new PoetryStorage(mockPreferencesSotrage);
            await poetryStorage.InitializeAsync(); // 调用初始化

            Assert.IsTrue(File.Exists(PoetryStorage.PoetryDbPath));

            // Mock 验证
            preferencesStorageMock.Verify(
                p => p.Set(PoetryStorageConstants.VersionKey, PoetryStorageConstants.Version),
                Times.Once
            );
        }

        /// <summary>
        /// 测试 是否初始化了
        /// </summary>
        [Test]
        public void TestIsInitialized()
        {
            // 测试 返回 当前的版本
            var preferencesStorageMock = new Mock<IPreferencesStorage>();  // 生成Mock对象
            preferencesStorageMock.Setup(p => p.Get(PoetryStorageConstants.VersionKey, PoetryStorageConstants.NoVersion))
                .Returns(PoetryStorageConstants.Version);
            IPreferencesStorage mockpreferencesStorage = preferencesStorageMock.Object;
            PoetryStorage poetryStorage = new PoetryStorage(mockpreferencesStorage);

            Assert.IsTrue(poetryStorage.IsInitialized());
        }

        /// <summary>
        /// 测试 诗词存储没有初始化的情况
        /// </summary>
        [Test]
        public void TestIsNotInitialized()
        {
            var preferencesStorageMock = new Mock<IPreferencesStorage>();
            preferencesStorageMock.Setup(p => p.Get(PoetryStorageConstants.VersionKey, PoetryStorageConstants.NoVersion))
                .Returns(PoetryStorageConstants.Version - 1); // 返回不是当前的版本
            IPreferencesStorage mockpreferencesStorage = preferencesStorageMock.Object;
            PoetryStorage poetryStorage = new PoetryStorage(mockpreferencesStorage);

            Assert.IsFalse(poetryStorage.IsInitialized());
        }

        [Test]
        public async Task TestGetPoetryAsync()
        {
            PoetryStorage poetryStorage = await PoetrySotrageHelper.GetInitializedPoetryStorageAsync();

            Models.Poetry poetry = await poetryStorage.GetPeotryAsync(10001);

            Assert.AreEqual("临江仙 · 夜归临皋", poetry.Name);

            await poetryStorage.CloseAsync(); // 关闭连接，使其能被删除。
        }

        [Test]
        public async Task TestGetPoetriesAsync()
        {
            PoetryStorage poetryStorage = await PoetrySotrageHelper.GetInitializedPoetryStorageAsync();

            var where = Expression.Lambda<Func<Models.Poetry, bool>>(Expression.Constant(true), Expression.Parameter(typeof(Models.Poetry), "p"));
            // 相当于 p => true 吧

            List<Models.Poetry> poetries = 
                await poetryStorage.GetPeotriesAsync(where, 0, int.MaxValue); // 取出数据库中全部的诗词

            Assert.AreEqual(PoetrySotrageHelper.PoetryNumber, poetries.Count);


            await poetryStorage.CloseAsync(); // 关闭连接，使其能被删除。
        }

    }
}
