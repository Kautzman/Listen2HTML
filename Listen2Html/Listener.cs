using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Net;
using System.IO;

namespace Listen2Html
{
    class Listener : BindableBase
    {
        private SolidColorBrush ListenerGreen = new SolidColorBrush(Color.FromRgb(20, 200, 20));
        private SolidColorBrush ListenerRed = new SolidColorBrush(Color.FromRgb(200, 20, 20));
        private SolidColorBrush ListenerGray = new SolidColorBrush(Color.FromRgb(90, 90, 90));

        Timer timer = new Timer();

        private string _url;
        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        private int _updateInterval = 1800;
        public int UpdateInterval
        {
            get { return _updateInterval; }
            set { SetProperty(ref _updateInterval, value); }

        }

        private int _intervalRemaining;
        public int IntervalRemaining
        {
            get { return _intervalRemaining; }
            set { SetProperty(ref _intervalRemaining, value); }

        }

        private HTTPState _state = HTTPState.none;
        public HTTPState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }

        }

        private int _totalRequests;
        public int TotalRequests
        {
            get { return _totalRequests; }
            set { SetProperty(ref _totalRequests, value); }
        }

        private int _changes;
        public int Changes
        {
            get { return _changes; }
            set { SetProperty(ref _changes, value); }
        }

        private string _lastHTML;
        public string LastHTML
        {
            get { return _lastHTML; }
            set { SetProperty(ref _lastHTML, value); }
        }

        private bool _isActive = false;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                SetProperty(ref _isActive, value);
                RaisePropertyChanged("ActionState");
                RaisePropertyChanged("ActionColor");
            }
        }

        private ObservableCollection<LogItem> _logs = new ObservableCollection<LogItem>();
        public ObservableCollection<LogItem> Logs
        {
            get { return _logs; }
            set { SetProperty(ref _logs, value); }
        }

        private LogItem LastLog;

        public string ActionState
        {
            get
            {
                return (IsActive) ? "\uE769" : "\uE768";
            }
        }

        public Brush ActionColor
        {
            get
            {
                return (IsActive) ? ListenerRed : ListenerGreen;
            }
        }

        public Brush StateColor
        {
            get
            {
                switch(State)
                {
                    case HTTPState.none: return ListenerGray;
                    case HTTPState.success: return ListenerGreen;
                    case HTTPState.failed: return ListenerRed;
                    default: return ListenerGray;
                }
            }
        }

        public DelegateCommand PauseCommand { get; private set; }
        public DelegateCommand RestartCommand { get; private set; }
        public DelegateCommand ShowHistoryCommand { get; private set; }

        public Listener(string thisURL, int thisUpdateInterval)
        {
            Url = thisURL;
            UpdateInterval = thisUpdateInterval;
        }

        public Listener(string thisUrl)
        {
            PauseCommand = new DelegateCommand(TogglePauseState);
            RestartCommand = new DelegateCommand(RestartTimer);
            ShowHistoryCommand = new DelegateCommand(ShowHistory);

            Url = thisUrl;

            IntervalRemaining = UpdateInterval;
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = 1000;
        }

        private void TogglePauseState()
        {
            IsActive = !IsActive;

            if(IsActive)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }

        private void RestartTimer()
        {
            IntervalRemaining = UpdateInterval;
            timer.Start();
        }

        //TODO: Implement Me
        private void ShowHistory()
        {
            MessageBox.Show("Not Yet Implemented");
        }

        private void TimerTick(object sender, EventArgs thisEvent)
        {
            IntervalRemaining--;

            if (IntervalRemaining <= 0)
            {
                timer.Stop();
                GetHtml();
                RestartTimer();
            }
        }

        private void GetHtml()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                string html = readStream.ReadToEnd();

                response.Close();
                readStream.Close();

                if (LastLog != null)
                {
                    bool hasChanges = CompareHtml(html, LastLog.Html);
                    LastLog = new LogItem(html, hasChanges);
                }
                else
                {
                    LastLog = new LogItem(html, false);
                }

                Logs.Add(LastLog);
                State = HTTPState.success;
            }
            else
            {
                LastLog = new LogItem();
                State = HTTPState.failed;
            }
        }

        private bool CompareHtml(string newHtml, string prevHtml)
        {
            bool hasChanges = !newHtml.SequenceEqual(prevHtml);

            if (hasChanges)
            {
                UserInfo.SendEmail(newHtml, prevHtml, Url);
            }

            return hasChanges;
        }

        //TODO:  Alert that this is a thing that happens
        private int ClampTime(int seconds)
        {
            // Capping refresh time to 3 minutes - don't spam web servers
            return (seconds >= 180) ? seconds : 180;
        }
    }

    enum HTTPState
    {
        none,
        failed,
        success
    }
}
