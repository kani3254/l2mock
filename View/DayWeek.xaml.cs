using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace taskrunner.View
{
    /// <summary>
    /// DayWeek.xaml の相互作用ロジック
    /// </summary>
    public partial class DayWeek : Page
    {
        private DateTime todayEnd = DateTime.Today.AddDays(1).AddSeconds(-1);
        private DateTime weekEnd = DateTime.Today.AddDays(7).AddSeconds(-1);

        public DayWeek()
        {
            InitializeComponent();

            // データ読み込み
            Tasks = new ObservableCollection<Task>();

            // view 組み立て
            this.running.DataContext = CreateView(Tasks, new Predicate<object>(RunningFilter));
            this.today.DataContext = CreateView(Tasks, new Predicate<object>(TodayFilter));
            this.thisWeek.DataContext = CreateView(Tasks, new Predicate<object>(ThisWeekFilter));
            this.icebox.DataContext = CreateView(Tasks, new Predicate<object>(IceboxFilter));
            this.closed.DataContext = CreateView(Tasks, new Predicate<object>(ClosedFilter));

        }

        #region データ関係
        // タスクリスト
        private ObservableCollection<Task> Tasks { get; set; }

        // running フィルタ
        private bool RunningFilter(object de)
        {
            Task task = de as Task;
            return (task.Status == "Running");
        }

        // today フィルタ
        private bool TodayFilter(object de)
        {
            Task task = de as Task;
            return (task.Limit <= todayEnd && task.Status != "Running" && task.Status != "Closed");
        }

        // this week フィルタ
        private bool ThisWeekFilter(object de)
        {
            Task task = de as Task;
            return (task.Limit <= weekEnd && task.Status != "Running" && task.Status != "Closed");
        }

        // iceboxフィルタ
        private bool IceboxFilter(object de)
        {
            Task task = de as Task;
            return (task.Status != "Running" && task.Status != "Closed");
        }

        // closed フィルタ
        private bool ClosedFilter(object de)
        {
            Task task = de as Task;
            return (task.Status == "Closed");
        }

        private ICollectionView CreateView(ICollection<Task> collection, Predicate<object> filter)
        {
            var ViewSource = new CollectionViewSource();
            ViewSource.Source = collection;
            var View = ViewSource.View;
            View.Filter = new Predicate<object>(filter);
            return View;
        }

        private void RefreshViews()
        {
            (running.DataContext as CollectionView).Refresh();
            (today.DataContext as CollectionView).Refresh();
            (thisWeek.DataContext as CollectionView).Refresh();
            (icebox.DataContext as CollectionView).Refresh();
            (closed.DataContext as CollectionView).Refresh();
        }
        #endregion

        private void Page_Drop(object sender, DragEventArgs e)
        {
            if (FileGroupDescriptor.CanDecode(e.Data))
            {
                foreach (FileGroupDescriptor.File file in FileGroupDescriptor.Decode(e.Data))
                {
                    Task newTask = new SimpleTask();
                    newTask.Name = file.Name;
                    newTask.AttachmentName = file.Name;
                    byte[] buffer = new byte[file.Content.Length];
                    file.Content.Position = 0;
                    file.Content.Read(buffer, 0, Convert.ToInt32(file.Content.Length));
                    newTask.AttachmentContent = buffer;
                    Tasks.Add(newTask);
                }
            }
            // View をリフレッシュ
            RefreshViews();
        }

        ListBoxItem dragItem;
        Point dragStartPos;
        DragAdorner dragGhost;

        private void listBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // マウスダウンされたアイテムを記憶
            dragItem = sender as ListBoxItem;
            // マウスダウン時の座標を取得
            dragStartPos = e.GetPosition(dragItem);
        }

        private void listBoxItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var lbi = sender as ListBoxItem;
            if (e.LeftButton == MouseButtonState.Pressed && dragGhost == null && dragItem == lbi)
            {
                var nowPos = e.GetPosition(lbi);
                if (Math.Abs(nowPos.X - dragStartPos.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(nowPos.Y - dragStartPos.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    var layer = AdornerLayer.GetAdornerLayer(this);
                    dragGhost = new DragAdorner(this, lbi, 0.5, dragStartPos);
                    layer.Add(dragGhost);
                    DragDrop.DoDragDrop(lbi, lbi, DragDropEffects.Move);
                    layer.Remove(dragGhost);
                    dragGhost = null;
                    dragItem = null;
                }
            }
        }

        private void listBoxItem_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (dragGhost != null)
            {
                var p = CursorInfo.GetNowPosition(this);
                var loc = this.PointFromScreen(this.PointToScreen(new Point(0, 0)));
                dragGhost.LeftOffset = p.X - loc.X;
                dragGhost.TopOffset = p.Y - loc.Y;
            }
        }

        private void Running_Drop(object sender, DragEventArgs e)
        {
            var item = e.Data.GetData(typeof(ListBoxItem)) as ListBoxItem;
            if (item == null) return;

            // ステータスを変更
            (item.Content as Task).Status = "Running";
        }

        private void Close_Drop(object sender, DragEventArgs e)
        {
            var item = e.Data.GetData(typeof(ListBoxItem)) as ListBoxItem;
            if (item == null) return;

            // ステータスを変更
            (item.Content as Task).Status = "Closed";
        }

        private void Open_Drop(object sender, DragEventArgs e)
        {
            var item = e.Data.GetData(typeof(ListBoxItem)) as ListBoxItem;
            if (item == null) return;

            // ステータスを変更
            (item.Content as Task).Status = null;
        }

        private void Today_Drop(object sender, DragEventArgs e)
        {
            var item = e.Data.GetData(typeof(ListBoxItem)) as ListBoxItem;
            if (item == null) return;

            // 期限を変更
            (item.Content as Task).Limit = todayEnd;
        }

        private void ThisWeek_Drop(object sender, DragEventArgs e)
        {
            var item = e.Data.GetData(typeof(ListBoxItem)) as ListBoxItem;
            if (item == null) return;

            // 期限を変更
            (item.Content as Task).Limit = weekEnd;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseButton.Visibility = Visibility.Collapsed;
            RunningButton.Visibility = Visibility.Visible;

            running.Visibility = Visibility.Collapsed;
            closed.Visibility = Visibility.Visible;
        }

        private void RunningButton_Click(object sender, RoutedEventArgs e)
        {
            RunningButton.Visibility = Visibility.Collapsed;
            CloseButton.Visibility = Visibility.Visible;

            closed.Visibility = Visibility.Collapsed;
            running.Visibility = Visibility.Visible;
        }

        private void TaskList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var list = sender as ListBox;
            if (list == null) return;
            var item = list.SelectedItem;
            Task task = null;
            if (item == null) // 選択されていない場合は新規
            {
                task = new SimpleTask();
            }
            else
            {
                task = item as Task;
            }

            var detailWindow = new TaskDetailWindow();
            detailWindow.DataContext = task;
            detailWindow.Show();
        }
    }
}
