using NUnit.Framework;
using RelearnMvvm.Services;
using RelearnMvvm.UnitTest.Helpers;
using RelearnMvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RelearnMvvm.UnitTest.ViewModels
{
    /// <summary>
    /// 测试 PoetriesViewModel
    /// </summary>
    public class PoetriesViewModelTest
    {
        /// <summary>
        /// 删除 数据库文件
        /// </summary>
        [SetUp, TearDown]
        public static void DeleteDatabaseFile() =>
            PoetrySotrageHelper.DeleteDbFile();

        /// <summary>
        /// 测试取出 正常数目的情况
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestLoadMorePoetries()
        {
            PoetryStorage poetryStorage = await PoetrySotrageHelper.GetInitializedPoetryStorageAsync();
            var where = Expression.Lambda<Func<Models.Poetry, bool>>(Expression.Constant(true), Expression.Parameter(typeof(Models.Poetry), "p"));

            PoetriesViewModel poetriesViewModel = new PoetriesViewModel(poetryStorage);

            poetriesViewModel.Where = where;

            #region 记录每次 Status的变化

            var statusList = new List<string>();
            poetriesViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(PoetriesViewModel.Status)) // 如果变化的属性为 Status
                    statusList.Add(poetriesViewModel.Status);
            };
            #endregion

            Assert.AreEqual(0, poetriesViewModel.PoetryCollection.Count);
            //poetriesViewModel.PageAppearingCommand.Execute(null); // Execute 为 异步执行，无法在线性的Test里面测试，需要剥离出函数
            await poetriesViewModel.PageAppearingCommandExecute();
            Assert.AreEqual(PoetriesViewModel.PageSize, poetriesViewModel.PoetryCollection.Count);

            // 调用了一次 PageAppearingCommandExecute，Status会有两种状态
            // 1. 先加载：Loading
            // 2. 加载完毕：空字符

            Assert.AreEqual(2, statusList.Count);
            Assert.AreEqual(PoetriesViewModel.Loading, statusList[0]);
            Assert.AreEqual(string.Empty, statusList[1]);

            await poetryStorage.CloseAsync(); // 关闭连接，使其能被删除。
        }

        /// <summary>
        /// 测试 没从数据库取出结果的情况
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestLoadNoPoetry()
        {
            PoetryStorage poetryStorage = await PoetrySotrageHelper.GetInitializedPoetryStorageAsync();
            var where = Expression.Lambda<Func<Models.Poetry, bool>>(Expression.Constant(false), Expression.Parameter(typeof(Models.Poetry), "p"));

            PoetriesViewModel poetriesViewModel = new PoetriesViewModel(poetryStorage);

            poetriesViewModel.Where = where;

            #region 记录每次 Status的变化

            var statusList = new List<string>();
            poetriesViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(PoetriesViewModel.Status)) // 如果变化的属性为 Status
                    statusList.Add(poetriesViewModel.Status);
            };
            #endregion

            Assert.AreEqual(0, poetriesViewModel.PoetryCollection.Count);
            await poetriesViewModel.PageAppearingCommandExecute();
            Assert.AreEqual(0, poetriesViewModel.PoetryCollection.Count);

            // 调用了一次 PageAppearingCommandExecute，Status会有两种状态
            // 1. 先加载：Loading
            // 2. 无结果：NoResult

            Assert.AreEqual(2, statusList.Count);
            Assert.AreEqual(PoetriesViewModel.Loading, statusList[0]);
            Assert.AreEqual(PoetriesViewModel.NoResult, statusList[1]);

            await poetryStorage.CloseAsync(); // 关闭连接，使其能被删除。
        }

        /// <summary>
        /// 测试 Where 不发生改变时，PageAppearingCommandExecute，无法再执行 LoadMorePoetries 两次
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestPageAppearingCommandExecute()
        {
            PoetryStorage poetryStorage = await PoetrySotrageHelper.GetInitializedPoetryStorageAsync();
            var where = Expression.Lambda<Func<Models.Poetry, bool>>(Expression.Constant(false), Expression.Parameter(typeof(Models.Poetry), "p"));

            PoetriesViewModel poetriesViewModel = new PoetriesViewModel(poetryStorage);

            await poetriesViewModel.PageAppearingCommandExecute(); // 调用第一次

            // 如果集合变化了，LoadMorePoetries 一定被调用了
            bool isCollectionChanged = false;
            poetriesViewModel.PoetryCollection.CollectionChanged += (sender, e) =>
                isCollectionChanged = true;

            await poetriesViewModel.PageAppearingCommandExecute(); // 调用第二次

            Assert.IsFalse(isCollectionChanged);
        }
    }
}
