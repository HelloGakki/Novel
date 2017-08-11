using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace NovelDownload
{
    public class Novel : INotifyPropertyChanged
    {
        #region " 私有变量 "

        private string novelName, novelAuthor, novelUrl;
        private string lastNovelChapter, lastChapterUrl;
        private List<string> novelChapter, chapterUrl;

        #endregion

        #region "接口实现"

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region "属性定义"

        public string NovelName
        {
            get
            {
                return novelName;
            }

            set
            {
                novelName = value;
                OnPropertyChange("NovelName");
            }
        }

        public string NovelAuthor
        {
            get
            {
                return novelAuthor;
            }

            set
            {
                novelAuthor = value;
                OnPropertyChange("NovelAuthor");
            }
        }

        public string NovelUrl
        {
            get
            {
                return novelUrl;
            }

            set
            {
                novelUrl = value;
                OnPropertyChange("NovelUrl");
            }
        }

        public string LastNovelChapter
        {
            get
            {
                return lastNovelChapter;
            }

            set
            {
                lastNovelChapter = value;
                OnPropertyChange("LastNovelChapter");
            }
        }

        public string LastChapterUrl
        {
            get
            {
                return lastChapterUrl;
            }

            set
            {
                lastChapterUrl = value;
                OnPropertyChange("LastChapterUrl");
            }
        }

        public List<string> NovelChapter
        {
            get
            {
                return novelChapter;
            }

            set
            {
                novelChapter = value;
                OnPropertyChange("NovelChapter");
            }
        }

        public List<string> ChapterUrl
        {
            get
            {
                return chapterUrl;
            }

            set
            {
                chapterUrl = value;
                OnPropertyChange("ChapterUrl");
            }
        }

        #endregion

        #region " 构造函数 "

        public Novel()
        {
            novelChapter = new List<string>();
            chapterUrl = new List<string>();
        }

        #endregion
    }
}
