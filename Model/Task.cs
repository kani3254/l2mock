using System;
using System.ComponentModel;

namespace taskrunner
{
    interface Task : INotifyPropertyChanged
    {
        int Id { get; set; }
        string Name { get; set; } // what
        string Status { get; set; } // how
        string Place { get; set; } // where
        string Owner { get; set; } // whom
        DateTime Limit { get; set; } // when
        DateTime LastModified { get; set; }
        String AttachmentName { get; set; }
        byte[] AttachmentContent { get; set; }
    }
}
