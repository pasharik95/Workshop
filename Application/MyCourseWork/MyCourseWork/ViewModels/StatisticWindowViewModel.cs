namespace MyCourseWork.ViewModels
{
    using Catel.Data;
    using Catel.MVVM;
    using MyCourseWork.Models;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Linq;
    using System;

    public class StatisticWindowViewModel : ViewModelBase
    {
        public StatisticWindowViewModel(worker w, DateTime ft, DateTime st)
        {
            NameWorker = w.lastName + " " + w.firstName + " " + w.middleName;
            Period = "       з\t" + "\t       по\n" + ft.Date.ToString().Split(' ')[0] + "\t" + st.Date.ToString().Split(' ')[0];
            DateFirst = ft;
            DateSecond = st.AddDays(1);
            SelectedWorker = w;
        }
        public worker SelectedWorker
        {
            get
            {
                return GetValue<worker>(SelectedWorkerProperty);
            }
            set
            {
                SetValue(SelectedWorkerProperty, value);
                listProcessForPieChart = getlistProcessForPieChart(SelectedWorker.idWorker, DateFirst, DateSecond);
                listKindOfProcessForPieChart = getlistKindOfProcessForPieChart(SelectedWorker.idWorker, DateFirst, DateSecond);
                listHoursForLineChart = getlistHoursForLineChart(SelectedWorker.idWorker, DateFirst, DateSecond);
                listMarkForPieChart = getlistMarkForPieChart(SelectedWorker.idWorker, DateFirst, DateSecond);
            }
        }
        public static readonly PropertyData SelectedWorkerProperty = RegisterProperty("SelectedWorker", typeof(worker));

        public string NameWorker
        {
            get; set;
        }
        public string Period
        {
            get; set;
        }
        public List<KeyValuePair<string, int>> listProcessForPieChart
        {
            get;
            private set;
        }
        public List<KeyValuePair<string, int>> listMarkForPieChart
        {
            get;
            private set;
        }
        public List<KeyValuePair<string, int>> listKindOfProcessForPieChart
        {
            get;
            private set;
        }
        public List<KeyValuePair<string, double>> listHoursForLineChart
        {
            get;
            private set;
        }
        private List<KeyValuePair<string, int>> getlistProcessForPieChart(int idWorker, DateTime first, DateTime second)
        {
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
            using (var db = new workshopEntities())
            {
                int own = db.process.Where(p => p.Worker_idWorker == idWorker && p.Worker_idWorker_Fact == idWorker && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second).ToList().Count;
                int notOwn = db.process.Where(p => p.Worker_idWorker != idWorker && p.Worker_idWorker_Fact == idWorker && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second).ToList().Count;
                list.Add(new KeyValuePair<string, int>("Свої", own));
                list.Add(new KeyValuePair<string, int>("Чужі", notOwn));
            }
            return list;
        }
        private List<KeyValuePair<string, int>> getlistMarkForPieChart(int idWorker, DateTime first, DateTime second)
        {
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
            using (var db = new workshopEntities())
            {
                int good = db.process.Where(p => p.Worker_idWorker_Fact == idWorker && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second && p.dateTimeStartFact <= p.dateTimeStart && p.dateTimeFinishFact <= p.dateTimeFinish).ToList().Count;
                int noStart = db.process.Where(p => p.Worker_idWorker_Fact == idWorker && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second && p.dateTimeStartFact > p.dateTimeStart && p.dateTimeFinishFact <= p.dateTimeFinish).ToList().Count;
                int noFinish = db.process.Where(p => p.Worker_idWorker_Fact == idWorker && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second && p.dateTimeStartFact <= p.dateTimeStart && p.dateTimeFinishFact > p.dateTimeFinish).ToList().Count;
                int bad = db.process.Where(p => p.Worker_idWorker_Fact == idWorker && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second && p.dateTimeStartFact > p.dateTimeStart && p.dateTimeFinishFact > p.dateTimeFinish).ToList().Count;

                list.Add(new KeyValuePair<string, int>("Старт - по плану,\nКінець - не по плану", noFinish));
                list.Add(new KeyValuePair<string, int>("Всі не по плану", bad));
                list.Add(new KeyValuePair<string, int>("Всі по плану", good));
                list.Add(new KeyValuePair<string, int>("Старт - не по плану,\nКінець - по плану", noStart));
            }
            return list;
        }

        private List<KeyValuePair<string, double>> getlistHoursForLineChart(int idWorker, DateTime first, DateTime second)
        {
            List<KeyValuePair<string, double>> list = new List<KeyValuePair<string, double>>();
            using (var db = new workshopEntities())
            {
                for (DateTime i = first; i <= second; i = i.AddDays(1))
                {
                    DateTime j = i.AddDays(1);
                    List<process> processList = db.process.Where(p => p.Worker_idWorker_Fact == idWorker && p.dateTimeStartFact.Value >= i && p.dateTimeFinishFact.Value < j).ToList();
                    double sum = 0;
                    foreach (process pr in processList)
                        sum += (pr.dateTimeFinishFact.Value - pr.dateTimeStartFact.Value).TotalMinutes;

                    list.Add(new KeyValuePair<string, double>(i.ToString().Split(' ')[0], sum / 60));
                }
            }
            return list;
        }
        private List<KeyValuePair<string, int>> getlistKindOfProcessForPieChart(int idWorker, DateTime first, DateTime second)
        {
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
            using (var db = new workshopEntities())
            {
                int demontazh = db.process.Where(p => p.Worker_idWorker_Fact == idWorker && p.Operation_idOperation == 1 && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second).ToList().Count;
                int raspakovka = db.process.Where(p => p.Worker_idWorker_Fact == idWorker && p.Operation_idOperation == 2 && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second).ToList().Count;
                int svarka = db.process.Where(p => p.Worker_idWorker_Fact == idWorker && p.Operation_idOperation == 3 && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second).ToList().Count;
                int vigotovka = db.process.Where(p => p.Worker_idWorker_Fact == idWorker && p.Operation_idOperation == 4 && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second).ToList().Count;
                int pokraska = db.process.Where(p => p.Worker_idWorker_Fact == idWorker && p.Operation_idOperation == 5 && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second).ToList().Count;
                int sushka = db.process.Where(p => p.Worker_idWorker_Fact == idWorker && p.Operation_idOperation == 6 && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second).ToList().Count;
                int polirovka = db.process.Where(p => p.Worker_idWorker_Fact == idWorker && p.Operation_idOperation == 7 && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second).ToList().Count;
                int montazh = db.process.Where(p => p.Worker_idWorker_Fact == idWorker && p.Operation_idOperation == 8 && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second).ToList().Count;
                int sdacha = db.process.Where(p => p.Worker_idWorker_Fact == idWorker && p.Operation_idOperation == 9 && p.dateTimeStartFact >= first && p.dateTimeFinishFact <= second).ToList().Count;

                list.Add(new KeyValuePair<string, int>("Демонтаж", demontazh));
                list.Add(new KeyValuePair<string, int>("Розпаковка", raspakovka));
                list.Add(new KeyValuePair<string, int>("Зварка", svarka));
                list.Add(new KeyValuePair<string, int>("Виготівка", vigotovka));
                list.Add(new KeyValuePair<string, int>("Фарбування", pokraska));
                list.Add(new KeyValuePair<string, int>("Сушка", sushka));
                list.Add(new KeyValuePair<string, int>("Поліровка", polirovka));
                list.Add(new KeyValuePair<string, int>("Монтаж", montazh));
                list.Add(new KeyValuePair<string, int>("Підготовка до здачі", sdacha));
            }
            return list;
        }
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

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
