using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace taskrunner
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            
            //TaskDataSet ds = DataContext as TaskDataSet;
            //ds.ReadXml("data.xml");

            //TaskDataSet.TaskDataTable taskTable = ds.Task;
            //EnumerableRowCollection<TaskDataSet.TaskRow> query;
        }

        //private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    // 編集対象の Task を取得
        //    ListBox lb = sender as ListBox;
        //    //            Task task = lb.SelectedItem as Task;
        //    Task task = (Task)lb.SelectedItem;
        //    if (task == null) // 選択されていない場合は新規作成
        //    {
        //        task = new SimpleTask();
        //    }

        //    // 保存先の取得
        //    FrameworkElement element = sender as FrameworkElement;
        //    TaskDataSet ds = element.DataContext as TaskDataSet;
        //    if (ds == null) return;

        //    TaskDetailWindow detailWindow = new TaskDetailWindow();
        //    detailWindow.DataContext = task; // 編集対象の Task
        //    detailWindow.Save.DataContext = ds; // 保存先の設定
        //    detailWindow.Show();
        //}

//        private void Save_Click(object sender, RoutedEventArgs e)
//        {
//            FrameworkElement element = sender as FrameworkElement;
//            TaskDataSet ds = element.DataContext as TaskDataSet;
//            ds.WriteXml("data.xml");
//            // EditTaskDetail();
            
//        }

//        // 新規作成
//        private void EditTaskDetail()
//        {
//            TaskDetailWindow detailWindow = new TaskDetailWindow();
//            detailWindow.Show();
//        }

//        // 既存更新
//        private void EditTaskDetail(Task task)
//        {
//            TaskDetailWindow detailWindow = new TaskDetailWindow();
//            detailWindow.DataContext = task;
//            detailWindow.Show();
//        }

//        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            FrameworkElement element = sender as FrameworkElement;
//            TaskDataSet ds = element.DataContext as TaskDataSet;
//            if (ds == null) return;

//            DataView view = ds.Task.DefaultView;
//            // Todo: フィルタ処理を書く
////            view.RowFilter = DateTime.Today.AddDays(1);
//        }

        private void Navigate(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item == null) return;

            string key = item.Header as string;
            NavigationService.Navigate(new Uri("View/" + key + ".xaml", UriKind.Relative));
        }

        

    }
}
