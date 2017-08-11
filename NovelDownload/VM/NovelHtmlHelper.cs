using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace NovelDownload
{
    public static class NovelHtmlHelper
    {
        #region " 私有静态变量 "

        private static string novelURL = "http://www.snwx8.com/modules/article/search.php";

        #endregion

        #region " 方法 "

        /// <summary>
        /// 获取搜索小说的列表
        /// </summary>
        /// <param name="novelName">小说名</param>
        /// <returns>符合要求的小说列表</returns>
        public static List<Novel> SearchNovel(string novelName)
        {
            List<Novel> novelList = new List<Novel>();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(GetResponse(novelName));

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//*[@id=\"newscontent\"]/div[1]/ul/li");
            Regex regex = new Regex("http://www.snwx8.com/book/\\d{1,}/\\d{4,}/");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {

                    string url = node.ChildNodes[1].ChildNodes[0].Attributes["href"].Value;
                    if (regex.IsMatch(url))
                    {
                        Novel novel = new Novel();
                        novel.NovelUrl = url;
                        novel.NovelName = node.ChildNodes[1].InnerText;
                        novel.NovelAuthor = node.ChildNodes[3].InnerText;
                        novel.LastNovelChapter = node.ChildNodes[2].InnerText;
                        novel.LastChapterUrl = node.ChildNodes[2].ChildNodes[0].Attributes["href"].Value;
                        //lastChapter = node.ChildNodes[2].InnerText;
                        //lastURL = node.ChildNodes[2].ChildNodes[0].Attributes["href"].Value;
                        novelList.Add(novel);
                    }
                }
            }

            return novelList;
        }

        /// <summary>
        /// 获取小说列表的网页流
        /// </summary>
        /// <param name="novelName">小说名</param>
        /// <returns></returns>
        private static Stream GetResponse(string novelName)
        {
            string postData = "searchkey=" + novelName;
            Encoding encoding = Encoding.GetEncoding("GBK");
            byte[] data = encoding.GetBytes(postData);

            // get request
            HttpWebRequest request = WebRequest.Create(novelURL) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            // sent data
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            // get reponse
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            return response.GetResponseStream();
        }

        /// <summary>
        /// 获取小说章节列表和对应链接
        /// </summary>
        /// <param name="novel">小说对象</param>
        public static void GetChapter(Novel novel)
        {

            Regex regex = new Regex("^\\d{7,10}.html$");
            HtmlWeb webClient = new HtmlWeb();
            webClient.OverrideEncoding = Encoding.GetEncoding("GBK");
            HtmlDocument doc = webClient.Load(novel.NovelUrl);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//*[@id=\"list\"]/dl/*/a");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    HtmlAttribute att = node.Attributes["href"];
                    if (regex.IsMatch(att.Value) && !novel.ChapterUrl.Contains(att.Value))
                    {
                        novel.ChapterUrl.Add(novel.NovelUrl + att.Value);
                        att = node.Attributes["title"];
                        novel.NovelChapter.Add(att.Value);
                    }
                }
            }
        }

        public static string GetText(string textUrl)
        {
            string article = null;
            HtmlWeb webClient = new HtmlWeb();
            webClient.OverrideEncoding = Encoding.GetEncoding("GBK");
            HtmlDocument doc = webClient.Load(textUrl);
            HtmlNode textNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"BookText\"]");
            //HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//*[@id=\"BookText\"]");
            if (textNode != null)
            {
                article = textNode.InnerHtml.HtmltoString("(<br\\s*/?>){1,2}", "\r\n");
                article = article.HtmltoString("&nbsp;", " ");
                return article;
            }
            else
                return null;
           // Message = "正在阅读：" + observableChapter[urlNumber];

        }

        #endregion
    }

    public static class MyString
    {
        public static string HtmltoString(this string htmlPage, string pattern, string replacement)
        {
            string newString = Regex.Replace(htmlPage, pattern, replacement);
            return newString;
        }
    }
}
