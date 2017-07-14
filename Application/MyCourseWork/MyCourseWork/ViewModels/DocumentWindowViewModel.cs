namespace MyCourseWork.ViewModels
{
    using Catel.Data;
    using Catel.MVVM;
    using System;
    using System.Linq;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Catel.Services;
    using MyCourseWork.WorkWithDOCX;
    using System.Collections.Generic;
    using System.Diagnostics;
    using MyCourseWork.Models;

    public class DocumentWindowViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;

        public DocumentWindowViewModel(IMessageService messageService)
        {
            _messageService = messageService;

            AllOrders = true;
            OneTask = true;
            using (var db = new workshopEntities())
            {
                WorkersCollection = new ObservableCollection<worker>();
                foreach (worker i in db.worker)
                {
                    WorkersCollection.Add(i);
                }
            }
            DateFirst = DateTime.Now;
            DateSecond = DateTime.Now;
        }

        //WorkersCollection
        public ObservableCollection<worker> WorkersCollection
        {
            get { return GetValue<ObservableCollection<worker>>(WorkersCollectionProperty); }
            set { SetValue(WorkersCollectionProperty, value); }
        }
        public static readonly PropertyData WorkersCollectionProperty = RegisterProperty("WorkersCollection", typeof(ObservableCollection<worker>));
        public worker SelectedWorker
        {
            get { return GetValue<worker>(SelectedWorkerProperty); }
            set
            {
                SetValue(SelectedWorkerProperty, value);
                if (OneTask)
                {
                    if (SelectedWorker != null)
                    {
                        TasksCollection = getProcessList(SelectedWorker, DateFirst);
                        if (TasksCollection.Count == 0)
                        {
                            _messageService.ShowAsync("В цей день не знайдено\n завдань для працівника!!!", "", MessageButton.OK, MessageImage.Information);
                            EnableTasksCollection = false;
                        }
                        else
                        {
                            EnableTasksCollection = true;
                        }
                    }
                    else
                        EnableTasksCollection = false;
                }
            }
        }
        public static readonly PropertyData SelectedWorkerProperty = RegisterProperty("SelectedWorker", typeof(worker));

        //RadioButtons for Tasks
        public bool OneTask
        {
            get { return GetValue<bool>(OneTaskProperty); }
            set
            {
                SetValue(OneTaskProperty, value);
                if (OneTask == true)
                {
                    VisibleSecondDatePicker = "Hidden";
                    TextLabel = "Виберіть дату";
                    EnableTasksCollection = false;
                }
            }
        }
        public static readonly PropertyData OneTaskProperty = RegisterProperty("OneTask", typeof(bool));
        public bool ManyTasksOneDay
        {
            get { return GetValue<bool>(ManyTasksOneDayProperty); }
            set
            {
                SetValue(ManyTasksOneDayProperty, value);
                if (ManyTasksOneDay == true)
                {
                    VisibleSecondDatePicker = "Hidden";
                    TextLabel = "Виберіть дату";
                    EnableTasksCollection = false;
                }
            }
        }
        public static readonly PropertyData ManyTasksOneDayProperty = RegisterProperty("ManyTasksOneDay", typeof(bool));
        public bool ManyTasks
        {
            get { return GetValue<bool>(ManyTasksProperty); }
            set
            {
                SetValue(ManyTasksProperty, value);
                if (ManyTasks)
                {
                    VisibleSecondDatePicker = "Visible";
                    TextLabel = "Начиная с";
                    EnableTasksCollection = false;
                }
            }
        }
        public static readonly PropertyData ManyTasksProperty = RegisterProperty(" ManyTasks", typeof(bool));

        //DatePickers
        public DateTime DateFirst
        {
            get { return GetValue<DateTime>(DateFirstProperty); }
            set
            {
                SetValue(DateFirstProperty, value);
                SelectedWorker = null;
            }
        }
        public static readonly PropertyData DateFirstProperty = RegisterProperty("DateFirst", typeof(DateTime));
        public string VisibleSecondDatePicker
        {
            get { return GetValue<string>(VisibleSecondDatePickerProperty); }
            set { SetValue(VisibleSecondDatePickerProperty, value); }
        }
        public static readonly PropertyData VisibleSecondDatePickerProperty = RegisterProperty("VisibleSecondDatePicker", typeof(string));
        public DateTime DateSecond
        {
            get { return GetValue<DateTime>(DateSecondProperty); }
            set { SetValue(DateSecondProperty, value); }
        }
        public static readonly PropertyData DateSecondProperty = RegisterProperty("DateSecond", typeof(DateTime));
        public string TextLabel
        {
            get { return GetValue<string>(TextLabelProperty); }
            set { SetValue(TextLabelProperty, value); }
        }

        public static readonly PropertyData TextLabelProperty = RegisterProperty("TextLabel", typeof(string));

        //Collection( and items) for Processes and selected process(value)
        public bool EnableTasksCollection
        {
            get { return GetValue<bool>(EnableTasksCollectionProperty); }
            set
            {
                SetValue(EnableTasksCollectionProperty, value);
                if (!EnableTasksCollection)
                {
                    SelectedProcess = null;
                    try
                    {
                        TasksCollection.Clear();
                    }
                    catch { }
                }
            }
        }
        public static readonly PropertyData EnableTasksCollectionProperty = RegisterProperty("EnableTasksCollection", typeof(bool));
        private ObservableCollection<process> getProcessList(worker w, DateTime date)
        {
            ObservableCollection<process> processList = new ObservableCollection<process>();
            using (var db = new workshopEntities())
            {
                var _processList = db.process.Where(p => p.Worker_idWorker == w.idWorker);
                foreach (process i in _processList.ToList())
                {
                    if ((i.dateTimeStart.Value - date).Days == 0)
                    {
                        i.order = db.order.Find(i.Order_idOrder);
                        i.order.car = db.car.Find(i.order.Car_idCar);
                        i.order.car.modelofcar = db.modelofcar.Find(i.order.car.ModelOfCar_idModelOfCar);
                        i.order.car.modelofcar.markofcar = db.markofcar.Find(i.order.car.modelofcar.MarkOfCar_idMarkOfCar);
                        i.operation = db.operation.Find(i.Operation_idOperation);
                        processList.Add(i);

                    }
                }
            }
            return processList;
        }
        public ObservableCollection<process> TasksCollection
        {
            get { return GetValue<ObservableCollection<process>>(TasksCollectionProperty); }
            set { SetValue(TasksCollectionProperty, value); }
        }
        public static readonly PropertyData TasksCollectionProperty = RegisterProperty("TasksCollection", typeof(ObservableCollection<process>));
        public process SelectedProcess
        {

            get { return GetValue<process>(SelectedProcessProperty); }
            set
            {
                SetValue(SelectedProcessProperty, value);

            }
        }
        public static readonly PropertyData SelectedProcessProperty = RegisterProperty("SelectedProcess", typeof(process));

        public Catel.MVVM.Command CreateTaskDocument
        {
            get
            {
                return new Catel.MVVM.Command(async () =>
                {
                    try
                    {

                        if (SelectedWorker == null)
                            throw new Exception("Будь ласка Виберіть працівника");
                        else
                        {

                            if (OneTask)
                                if (SelectedProcess != null)
                                    Report.createTasksDocument(new List<process>() { SelectedProcess, });
                                else
                                    throw new Exception("Будь ласка Виберіть завдання");

                            if (ManyTasksOneDay)
                                Report.createTasksDocument(getProcessesBetweenDate(DateFirst, DateFirst, SelectedWorker));

                            if (ManyTasks)
                                Report.createTasksDocument(getProcessesBetweenDate(DateFirst, DateSecond, SelectedWorker));

                            if (await _messageService.ShowAsync("Документ сформавано!!!\nвідкрити його?", "Результат", MessageButton.YesNo, MessageImage.Question) == MessageResult.Yes)
                                Process.Start("Task.docx");
                        }
                    }
                    catch (Exception ex)
                    {
                        _messageService.ShowAsync(ex.Message, "", MessageButton.OK, MessageImage.Exclamation);

                    }

                });
            }
        }
        private List<process> getProcessesBetweenDate(DateTime first, DateTime second, worker w)
        {
            second = second.AddDays(1);
            using (var db = new workshopEntities())
            {
                return db.process.Where(p => (p.Worker_idWorker == w.idWorker && p.dateTimeStart >= first && p.dateTimeFinish < second)).ToList();
            }
        }

        //Properties for Orders 
        public List<order> OrdersCollection
        {
            get { return GetValue<List<order>>(OrdersCollectionProperty); }
            set { SetValue(OrdersCollectionProperty, value); }
        }
        public static readonly PropertyData OrdersCollectionProperty = RegisterProperty("OrdersCollection", typeof(List<order>));
        public order SelectedOrder
        {
            get { return GetValue<order>(SelectedOrderProperty); }
            set { SetValue(SelectedOrderProperty, value); }
        }
        public static readonly PropertyData SelectedOrderProperty = RegisterProperty("SelectedOrder", typeof(order));
        public bool AllOrders
        {
            get { return GetValue<bool>(AllOrdersProperty); }
            set
            {
                SetValue(AllOrdersProperty, value);
                if (AllOrders)
                {
                    ComplateOrders = true;
                    NoStartOrders = true;
                    DoingOrders = true;
                }
            }
        }
        public static readonly PropertyData AllOrdersProperty = RegisterProperty("AllOrders", typeof(bool));
        public bool ComplateOrders
        {
            get { return GetValue<bool>(ComplateOrdersProperty); }
            set
            {
                SetValue(ComplateOrdersProperty, value);
                if (!ComplateOrders)
                    AllOrders = false;
            }
        }
        public static readonly PropertyData ComplateOrdersProperty = RegisterProperty("ComplateOrders", typeof(bool));
        public bool NoStartOrders
        {
            get { return GetValue<bool>(NoStartOrdersProperty); }
            set
            {
                SetValue(NoStartOrdersProperty, value);
                if (!NoStartOrders)
                    AllOrders = false;
            }
        }
        public static readonly PropertyData NoStartOrdersProperty = RegisterProperty("NoStartOrders", typeof(bool));
        public bool DoingOrders
        {
            get { return GetValue<bool>(DoingOrdersProperty); }
            set
            {
                SetValue(DoingOrdersProperty, value);
                if (!DoingOrders)
                    AllOrders = false;
            }
        }
        public static readonly PropertyData DoingOrdersProperty = RegisterProperty("DoingOrders", typeof(bool));
        public override string Title { get { return "View model title"; } }

        //Command for Orders
        public Catel.MVVM.Command FindOrdersCommand
        {
            get
            {
                return new Catel.MVVM.Command(() =>
                {
                    using (var db = new workshopEntities())
                    {
                        OrdersCollection = getOrders(DateTime.Now, DateTime.Now, AllOrders, ComplateOrders, NoStartOrders, DoingOrders);

                    }
                });
            }
        }
        private List<order> getOrders(DateTime first, DateTime second, bool allOrders, bool complateOrders, bool noStartOrders, bool doingOrders)
        {
            List<order> ordersCollection = new List<order>();
            using (var db = new workshopEntities())
            {

                if (complateOrders)
                    foreach (order i in db.order.Where(o => (o.status == true)).ToList())
                    {
                        i.Color = "#90EE90";
                        ordersCollection.Add(i);
                    }

                if (noStartOrders)
                    foreach (order i in db.order.Where(o => (o.status == false)).ToList())
                    {
                        var processes = db.process.Where(p => (p.Order_idOrder == i.idOrder && p.dateTimeStartFact != null));
                        if (processes.ToList().Count == 0)
                        {
                            i.Color = "#FFB6C1";
                            ordersCollection.Add(i);
                        }
                    }
                if (doingOrders)
                    foreach (order i in db.order.Where(o => (o.status == false)).ToList())
                    {
                        var processes = db.process.Where(p => (p.Order_idOrder == i.idOrder && p.dateTimeStartFact != null));

                        if (processes.ToList().Count > 0)
                        {
                            i.Color = "#87CEFA";
                            ordersCollection.Add(i);
                        }
                    }
                foreach (order i in ordersCollection)
                {
                    i.car = db.car.Find(i.Car_idCar);
                    i.car.modelofcar = db.modelofcar.Find(i.car.ModelOfCar_idModelOfCar);
                    i.car.modelofcar.markofcar = db.markofcar.Find(i.car.modelofcar.MarkOfCar_idMarkOfCar);
                    i.car.colorofcar = db.colorofcar.Find(i.car.ColorOfCar_idColorOfCar);
                }
            }
            return ordersCollection;
        }
        public Catel.MVVM.Command CreateWorksWithCarDocument
        {
            get
            {
                return new Catel.MVVM.Command(async () =>
                {
                    try
                    {
                        Report.createWorksWithCarDocument(SelectedOrder);
                        if (await _messageService.ShowAsync("Документ сформавано!!!\nвідкрити його?", "Результат", MessageButton.YesNo, MessageImage.Question) == MessageResult.Yes)
                            Process.Start("WorksWithCar.docx");
                    }
                    catch { }

                });
            }
        }
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
