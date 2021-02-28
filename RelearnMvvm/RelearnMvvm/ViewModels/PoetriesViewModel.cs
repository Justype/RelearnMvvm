using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RelearnMvvm.Models;
using RelearnMvvm.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RelearnMvvm.ViewModels
{
    public class PoetriesViewModel : ViewModelBase
    {
        /// <summary>
        /// 创建 诗词页 的ViewModel
        /// </summary>
        /// <param name="poetryStorage">诗词存储</param>
        public PoetriesViewModel(IPoetryStorage poetryStorage)
        {
            _poetryStorage = poetryStorage;


        }

        /// <summary>
        /// 诗词存储
        /// </summary>
        private readonly IPoetryStorage _poetryStorage;

        /// <summary>
        /// 诗词集合
        /// </summary>
        public ObservableCollection<Poetry> PoetryCollection { get; } = new ObservableCollection<Poetry>();

        #region 初始加载
        public RelayCommand PageAppearingCommand => _pageAppearingCommand ??
            (_pageAppearingCommand = new RelayCommand(async () => await PageAppearingCommandExecute()));
        private RelayCommand _pageAppearingCommand;

        public async Task PageAppearingCommandExecute()
        {
            if (!_isNewQuery) return;
            _isNewQuery = false;

            PoetryCollection.Clear();
            await LoadMorePoetries();
        }

        /// <summary>
        /// 是否为新查询
        /// </summary>
        private bool _isNewQuery;
        #endregion

        #region 诗词加载 + 无限滚动
        /// <summary>
        /// Where 条件
        /// </summary>
        public Expression<Func<Poetry, bool>> Where
        {
            get => _where;
            set
            {
                ResetThreshold(); // 更改查询条件时，重置 阈值
                _isNewQuery = true;
                Set(ref _where, value);
            }
        }
        private Expression<Func<Poetry, bool>> _where;

        /// <summary>
        /// 还剩几个诗词的时候加载
        /// </summary>
        public int PoetryThreshold
        {
            get => _poetryTreshold;
            set => Set(ref _poetryTreshold, value);
        }
        private int _poetryTreshold = 0;

        /// <summary>
        /// 加载更多的 诗词
        /// </summary>
        public async Task LoadMorePoetries()
        {
            Status = Loading;

            List<Poetry> poetries = await _poetryStorage.GetPeotriesAsync(Where, PoetryCollection.Count, PageSize);
            if (poetries.Count == PageSize) // 取出的数目只能 小于等于 PageSize
                Status = string.Empty;
            else if (poetries.Count == 0 && PoetryCollection.Count == 0) // 取回的结果 和 原来都为0，一定搜索结果为0
                OnNoResult();
            else // 取出的数目 小于 每次取出的数目，所以没有更多了
                OnNoMoreResult();

            foreach (var poetry in poetries)
                PoetryCollection.Add(poetry);
        }

        /// <summary>
        /// 没有更多的结果时，将阈值设为-1，更改状态
        /// </summary>
        public void OnNoMoreResult()
        {
            PoetryThreshold = -1; // 将 Treshold 设为-1，所以不再触发 ThresholdReached
            Status = NoMoreResult;
        }

        /// <summary>
        /// 没有结果时，更改状态
        /// </summary>
        public void OnNoResult() => Status = NoResult;

        /// <summary>
        /// 重置阈值为0
        /// </summary>
        public void ResetThreshold() => PoetryThreshold = 0;

        /// <summary>
        /// 每次加载诗词的数目
        /// </summary>
        public const int PageSize = 20;
        #endregion

        #region 状态相关
        private string _status;
        /// <summary>
        /// 展示当前的状态
        /// </summary>
        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }


        /// <summary>
        /// 正在载入
        /// </summary>
        public const string Loading = "正在载入";
        /// <summary>
        /// 没有结果
        /// </summary>
        public const string NoResult = "没有结果";
        /// <summary>
        /// 没有更多的结果
        /// </summary>
        public const string NoMoreResult = "没有更多的结果";
        #endregion
    }
}
