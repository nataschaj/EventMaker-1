﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventMaker.Model;
using System.Windows.Input;
using EventMaker.Common;
using EventMaker.Handler;
using EventMaker.Persistency;
using System.ComponentModel;

namespace EventMaker.ViewModel
{
    class EventViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        /*Singelton*/
        private EventCatalogSingleton _eventSingleton;

        public EventCatalogSingleton Eventsingelton
        {
            get { return _eventSingleton; }
            set { _eventSingleton = value; }
        }

        /*Properties*/

          public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public DateTimeOffset _date { get; set; }
        public TimeSpan _time { get; set; }
        public myEventHandler eventHandler { get; set; }

        /*Selected Event*/
        private Event selectedEvent;

  

        public Event SelectedEvent
        {
            get { return selectedEvent; }
            set { selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
            }
        }


        /*Commands Properties*/
        public ICommand CreateEventCommand { get; set; }
        public ICommand DeleteEventComand { get; set; }



        /*Constructor*/
        public EventViewModel()
        {
            Eventsingelton = EventCatalogSingleton.Instance;

            DateTime dt = System.DateTime.Now;
            _date = new DateTimeOffset(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0, new TimeSpan());
            _time = new TimeSpan(dt.Hour, dt.Minute, dt.Second);

            /*RelayCommand og refrence til handler*/
            eventHandler = new myEventHandler(this);

         

            CreateEventCommand = new RelayCommand(eventHandler.CreateEvent, null);
            DeleteEventComand = new RelayCommand(eventHandler.DeleteEvent, checkOmListenErEmpty);
            PersistencyService.HentDataFraDiskAsync();

        }


        public bool checkOmListenErEmpty()
        {
            if (EventCatalogSingleton.Instance.Events.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
           
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}

