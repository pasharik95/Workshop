using Catel.Data;
using Catel.MVVM;
using MyCourseWork.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace MyCourseWork.ViewModels
{
    public class ChangeWorkerAndDatesViewModel : ViewModelBase
    {
        public ChangeWorkerAndDatesViewModel()
        {
            using (var db = new workshopEntities())
            {
                WorkersCollection = new ObservableCollection<worker>();
                foreach (worker i in db.worker)
                {
                    WorkersCollection.Add(i);
                }
                DateFirst = DateTime.Now.Date.AddDays(-10);
                DateSecond = DateTime.Now.Date;
                SelectedWorker = WorkersCollection[0];
            }
        }
        public ObservableCollection<worker> WorkersCollection
        {
            get { return GetValue<ObservableCollection<worker>>(WorkersCollectionProperty); }
            set
            {
                SetValue(WorkersCollectionProperty, value);
            }
        }
        public static readonly PropertyData WorkersCollectionProperty = RegisterProperty("WorkersCollection", typeof(ObservableCollection<worker>));
        public worker SelectedWorker
        {
            get
            {
                return GetValue<worker>(SelectedWorkerProperty);
            }
            set
            {
                SetValue(SelectedWorkerProperty, value);
            }
        }
        public static readonly PropertyData SelectedWorkerProperty = RegisterProperty("SelectedWorker", typeof(worker));

        //DatePickers
        public DateTime DateFirst
        {
            get { return GetValue<DateTime>(DateFirstProperty); }
            set
            {
                SetValue(DateFirstProperty, value);
            }
        }
        public static readonly PropertyData DateFirstProperty = RegisterProperty("DateFirst", typeof(DateTime));
        public DateTime DateSecond
        {
            get { return GetValue<DateTime>(DateSecondProperty); }
            set { SetValue(DateSecondProperty, value); }
        }
        public static readonly PropertyData DateSecondProperty = RegisterProperty("DateSecond", typeof(DateTime));

        public override string Title { get { return "Статистика"; } }

    }

}
