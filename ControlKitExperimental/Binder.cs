using System;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ControlKitExperimental.Models;

namespace ControlKitExperimental
{
    public class Binder : INotifyPropertyChanged
    {
        static Binder instance = null;
        static readonly object padlock = new object();

        public Binder()
        {
            RandomTextItems = new ObservableCollection<TextSet>();
        }

        public static Binder Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Binder();
                    }
                    return instance;
                }
            }
        }

        private ObservableCollection<TextSet> randomTextItems;
        public ObservableCollection<TextSet> RandomTextItems
        {
            get
            {
                return randomTextItems;
            }
            set
            {
                if (randomTextItems != value)
                {
                    randomTextItems = value;
                    NotifyPropertyChanged("RandomTextItems");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { PropertyChanged(this, new PropertyChangedEventArgs(info)); });
            }
        }
    }
}
