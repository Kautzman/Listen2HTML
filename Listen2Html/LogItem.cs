using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Listen2Html
{
    class LogItem : INotifyPropertyChanged
    {
        private string _html;
        public string Html
        {
            get { return _html; }
            set
            {
                _html = value;
                OnPropertyChanged();
            }
        }

        private DateTime _timestamp = DateTime.Now;
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set
            {
                _timestamp = value;
                OnPropertyChanged();
            }
        }

        private HTTPState _state;
        public HTTPState State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        private bool _hasChanges;
        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                _hasChanges = value;
                OnPropertyChanged();
            }
        }

        // Success
        public LogItem(string thisHtml, bool thisHasChanges)
        {
            State = HTTPState.success;
            HasChanges = thisHasChanges;
            Html = thisHtml;
        }

        // Failure
        public LogItem()
        {
            State = HTTPState.failed;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
