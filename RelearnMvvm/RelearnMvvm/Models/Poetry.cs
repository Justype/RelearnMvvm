using System;
using System.Collections.Generic;
using System.Text;

namespace RelearnMvvm.Models
{
    /// <summary>
    /// 诗词类
    /// </summary>
    [SQLite.Table("works")]
    public class Poetry
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SQLite.PrimaryKey, SQLite.Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [SQLite.Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// 作者名字
        /// </summary>
        [SQLite.Column("author_name")]
        public string AuthorName { get; set; }

        /// <summary>
        /// 朝代
        /// </summary>
        [SQLite.Column("dynasty")]
        public string Dynasty { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        [SQLite.Column("正文")]
        public string Content { get; set; }

        /// <summary>
        /// 译文
        /// </summary>
        [SQLite.Column("译文")]
        public string Translation { get; set; }

        /// <summary>
        /// 诗词的布局
        /// </summary>
        [SQLite.Column("layout")]
        public string Layout { get; set; }

        #region Constants
        /// <summary>
        /// 居中布局
        /// </summary>
        public const string CenterLayout = "center";

        /// <summary>
        /// 缩进布局
        /// </summary>
        public const string IndentLayout = "indent";
        #endregion

        #region SQLite Ignore

        private string _snippet;

        /// <summary>
        /// 预览
        /// </summary>
        [SQLite.Ignore]
        public string Snippet =>
            _snippet ?? (_snippet = Content.Split('。')[0].Replace("\r\n", " "));
        // 以 。 进行切分，选择第一项。   可能有换行，所以替换
        #endregion
    }
}
