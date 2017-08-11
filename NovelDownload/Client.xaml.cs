using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NovelDownload.ViewModel;

namespace NovelDownload
{
    /// <summary>
    /// Client.xaml 的交互逻辑
    /// </summary>
    public partial class Client : Window
    {
        public Client()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            var model = DataContext as ClientViewModel;
            //  model.DelayGetChapter();
           // model.ViewModelInit();
            model.DelaySearchNovel();
        }

        //private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var model = DataContext as ClientViewModel;
        //    model.GetChapter();
        //}

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var model = DataContext as ClientViewModel;
          //  model.DelayGetChapter();
            model.DelaySaveNovel();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            var model = DataContext as ClientViewModel;
            model.StopDownload();
        }
    }
}
