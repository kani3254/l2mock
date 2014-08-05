using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace taskrunner
{
    class SimpleTask : Task
    {
        private int _id; // タスクID
        private string _name; // タスク名
        private string _status; // ステータス
        private string _place;
        private string _owner; // オーナー
        private DateTime _limit; // 期限
        private DateTime _lastModified; // 最終更新者
        private string _attachmentName; // 添付名
        private byte[] _attachmentContent; // 添付内容

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }
        public string Place
        {
            get { return _place; }
            set { SetProperty(ref _place, value); }
        }
        public string Owner
        {
            get { return _owner; }
            set { SetProperty(ref _owner, value); }
        }
        public DateTime Limit
        {
            get { return _limit; }
            set { SetProperty(ref _limit, value); }
        }
        public DateTime LastModified
        {
            get { return _lastModified; }
            set { SetProperty(ref _lastModified, value); }
        }
        public String AttachmentName
        {
            get { return _attachmentName; }
            set { SetProperty(ref _attachmentName, value); }
        }
        public byte[] AttachmentContent
        {
            get { return _attachmentContent; }
            set { SetProperty(ref _attachmentContent, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SetProperty<T>(ref T storage, T value,
                            [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return;

            storage = value;
            NotifyPropertyChanged(propertyName);
        }
    }
}
