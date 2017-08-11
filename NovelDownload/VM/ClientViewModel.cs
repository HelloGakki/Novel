using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;

namespace NovelDownload.ViewModel
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        #region "接口实现"

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region "私有变量"

        private List<Novel> novelList;
        private string searchNovelName;
        private int selectNovelIndex;
        private string message;
        private bool isCanDownload = false, isDownloading = false, isStopThread = false;

        #endregion

        #region "属性定义"

        public List<Novel> NovelList
        {
            get
            {
                return novelList;
            }
            set
            {
                novelList = value;
                OnPropertyChange("NovelList");
            }
        }

        public string SearchNovelName
        {
            get
            {
                return searchNovelName;
            }

            set
            {
                searchNovelName = value;
                OnPropertyChange("SearchNovelName");
            }
        }

        public int SelectNovelIndex
        {
            get
            {
                return selectNovelIndex;
            }

            set
            {
                selectNovelIndex = value;
                OnPropertyChange("SelectNovelIndex");
            }
        }

        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
                OnPropertyChange("Message");
            }
        }

        public bool IsCanDownload
        {
            get
            {
                return isCanDownload;
            }

            set
            {
                isCanDownload = value;
                OnPropertyChange("IsCanDownload");
            }
        }

        #endregion

        #region " 方法 "

        private void SearchNovel()
        {
            Message = "正在搜索...";
            NovelList = NovelHtmlHelper.SearchNovel(SearchNovelName);
            if (NovelList.Count != 0)
            {
                Message = "获取小说列表成功...";
                IsCanDownload = true;
            }
            else
            {
                Message = "获取小说列表失败...";
                IsCanDownload = false;
            }
        }

        private void GetChapter()
        {
            Message = "正在获取章节...";
            NovelHtmlHelper.GetChapter(NovelList[SelectNovelIndex]);
            Message = "获取成功...";
        }

        private void SaveNovel()
        {
            try
            {
                isDownloading = true;
                Novel selectNovel = NovelList[SelectNovelIndex];
                GetChapter();
                FileStream novelText = new FileStream(Environment.CurrentDirectory + @"\" + selectNovel.NovelName + ".txt", FileMode.Create);
                StreamWriter novelTextWriter = new StreamWriter(novelText, Encoding.Unicode);
                Message = "准备下载...";
                for (var chapterIndex = 0; chapterIndex < selectNovel.ChapterUrl.Count; chapterIndex++)
                {
                    if (isStopThread)
                    {
                        selectNovel.ChapterUrl.Clear();
                        break;
                    }
                    string url = selectNovel.ChapterUrl[chapterIndex];
                    string textString = selectNovel.NovelChapter[chapterIndex] + NovelHtmlHelper.GetText(url) + "\r\n";
                    byte[] textByte = Encoding.Unicode.GetBytes(textString);
                    novelTextWriter.Write(textString);
                    Message = "下载进度：" + (chapterIndex + 1).ToString() + "/" + (selectNovel.ChapterUrl.Count).ToString();
                }
                novelText.Close();
                isDownloading = false;
                if (isStopThread)
                    Message = "已取消下载";
                else
                    Message = "下载完成";

                isStopThread = false;
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        public void DelaySaveNovel()
        {
            if (isDownloading)
            {
                MessageBox.Show("小说正在小在中, " + "请稍后...", "Message");
                return;
            }
            Thread saveNovelThread = new Thread(new ThreadStart(SaveNovel));
            saveNovelThread.IsBackground = true;
            saveNovelThread.Start();
        }

        public void DelaySearchNovel()
        {
            Thread searchNovelThread = new Thread(new ThreadStart(SearchNovel));
            searchNovelThread.IsBackground = true;
            searchNovelThread.Start();
        }

        public void StopDownload()
        {
            isStopThread = true;
        }

        #endregion
    }
}
