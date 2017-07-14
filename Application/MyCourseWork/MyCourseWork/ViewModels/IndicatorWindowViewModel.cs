namespace MyCourseWork.ViewModels
{
    using Catel.MVVM;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using System.Linq;
    using Catel.Data;
    using System;
    using Catel.Services;
    using MyCourseWork.Views;
    using System.Windows.Input;

    public class IndicatorWindowViewModel : ViewModelBase
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        private readonly IUIVisualizerService _visualizerService;
        private readonly IMessageService _messageService;
        public IndicatorWindowViewModel(IUIVisualizerService visualizerService, IMessageService messageService)
        {
            _messageService = messageService;
            _visualizerService = visualizerService;
            OrderList = getOrderList();
            ShowAddCopmlatedProccessWindow = new Command(arg => AddCopmlatedProccessWindowShow());
            ShowDocumentWindow = new Command(arg => documentWindowShow());
            ShowStatisticWindow = new Command(arg => changeWorkerAndDatesShow());
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            OrderList.Clear();
            OrderList = getOrderList();
        }


        public List<OrderItem> OrderList
        {
            get { return GetValue<List<OrderItem>>(OrderListProperty); }
            set { SetValue(OrderListProperty, value); }
        }
        public static readonly PropertyData OrderListProperty = RegisterProperty("OrderList", typeof(List<OrderItem>));
        public OrderItem SelectedOrder
        {
            get { return GetValue<OrderItem>(SelectedOrderProperty); }
            set
            {
                SetValue(SelectedOrderProperty, value);
                DetailList = getDetailList(SelectedOrder.idOrder);

            }
        }
        public static readonly PropertyData SelectedOrderProperty = RegisterProperty("SelectedOrder", typeof(OrderItem));

        public List<DetailItem> DetailList
        {
            get { return GetValue<List<DetailItem>>(DetailListProperty); }
            set { SetValue(DetailListProperty, value); }
        }
        public static readonly PropertyData DetailListProperty = RegisterProperty("DetailList", typeof(List<DetailItem>));
        private List<OrderItem> getOrderList()
        {
            List<OrderItem> oiList = new List<OrderItem>();
            using (var db = new workshopEntities())
            {
                var list = db.order.Where(o => o.status == false);
                foreach (order o in list.ToList())
                {
                    OrderItem oi = new OrderItem();
                    oi.idOrder = o.idOrder;
                    oi.Car = o.car.modelofcar.markofcar.nameMarkOfCar + " " + o.car.modelofcar.nameModelOfCar + " " + o.car.registrNumber;
                    oi.DemontazhCondition = getStateAndColorForOrder(o.idOrder, 1).Split('|')[0];
                    oi.DemontazhConditionColor = getStateAndColorForOrder(o.idOrder, 1).Split('|')[1];
                    oi.RaspacovkaCondition = getStateAndColorForOrder(o.idOrder, 2).Split('|')[0];
                    oi.RaspacovkaConditionColor = getStateAndColorForOrder(o.idOrder, 2).Split('|')[1];
                    oi.SvarkaConditionColor = getStateAndColorForOrder(o.idOrder, 3).Split('|')[1];
                    oi.SvarkaCondition = getStateAndColorForOrder(o.idOrder, 3).Split('|')[0];
                    oi.VigotovkaCondition = getStateAndColorForOrder(o.idOrder, 4).Split('|')[0];
                    oi.VigotovkaConditionColor = getStateAndColorForOrder(o.idOrder, 4).Split('|')[1];
                    oi.PokraskaCondition = getStateAndColorForOrder(o.idOrder, 5).Split('|')[0];
                    oi.PokraskaConditionColor = getStateAndColorForOrder(o.idOrder, 5).Split('|')[1];
                    oi.PolirovkaCondition = getStateAndColorForOrder(o.idOrder, 7).Split('|')[0];
                    oi.PolirovkaConditionColor = getStateAndColorForOrder(o.idOrder, 7).Split('|')[1];
                    oi.MontazhCondition = getStateAndColorForOrder(o.idOrder, 8).Split('|')[0];
                    oi.MontazhConditionColor = getStateAndColorForOrder(o.idOrder, 8).Split('|')[1];
                    oi.SdachaCondition = getStateAndColorForOrder(o.idOrder, 9).Split('|')[0];
                    oi.SdachaConditionColor = getStateAndColorForOrder(o.idOrder, 9).Split('|')[1];
                    oi.SushkaCondition = getStateAndColorForOrder(o.idOrder, 6).Split('|')[0];
                    oi.SushkaConditionColor = getStateAndColorForOrder(o.idOrder, 6).Split('|')[1];
                    oiList.Add(oi);


                }
            }
            return oiList;
        }
        private List<DetailItem> getDetailList(int idOrder)
        {
            List<DetailItem> diList = new List<DetailItem>();
            using (var db = new workshopEntities())
            {
                int idCar = db.order.Find(idOrder).Car_idCar;
                var _details = db.detail.Where(d => d.Car_idCar == idCar);
                foreach (detail d in _details.ToList())
                {
                    DetailItem di = new DetailItem();
                    di.NameDetail = d.standartdetail.nameStandartDetail;
                    di.idDetail = d.idDetail;
                    di.DemontazhCondition = getStateAndColorForDetail(d.idDetail, idOrder, 1).Split('|')[0];
                    di.DemontazhConditionColor = getStateAndColorForDetail(d.idDetail, idOrder, 1).Split('|')[1];
                    di.RaspacovkaCondition = getStateAndColorForDetail(d.idDetail, idOrder, 2).Split('|')[0];
                    di.RaspacovkaConditionColor = getStateAndColorForDetail(d.idDetail, idOrder, 2).Split('|')[1];
                    di.SvarkaConditionColor = getStateAndColorForDetail(d.idDetail, idOrder, 3).Split('|')[1];
                    di.SvarkaCondition = getStateAndColorForDetail(d.idDetail, idOrder, 3).Split('|')[0];
                    di.VigotovkaCondition = getStateAndColorForDetail(d.idDetail, idOrder, 4).Split('|')[0];
                    di.VigotovkaConditionColor = getStateAndColorForDetail(d.idDetail, idOrder, 4).Split('|')[1];
                    di.PokraskaCondition = getStateAndColorForDetail(d.idDetail, idOrder, 5).Split('|')[0];
                    di.PokraskaConditionColor = getStateAndColorForDetail(d.idDetail, idOrder, 5).Split('|')[1];
                    di.PolirovkaCondition = getStateAndColorForDetail(d.idDetail, idOrder, 7).Split('|')[0];
                    di.PolirovkaConditionColor = getStateAndColorForDetail(d.idDetail, idOrder, 7).Split('|')[1];
                    di.MontazhCondition = getStateAndColorForDetail(d.idDetail, idOrder, 8).Split('|')[0];
                    di.MontazhConditionColor = getStateAndColorForDetail(d.idDetail, idOrder, 8).Split('|')[1];
                    di.SdachaCondition = getStateAndColorForDetail(d.idDetail, idOrder, 9).Split('|')[0];
                    di.SdachaConditionColor = getStateAndColorForDetail(d.idDetail, idOrder, 9).Split('|')[1];
                    di.SushkaCondition = getStateAndColorForDetail(d.idDetail, idOrder, 6).Split('|')[0];
                    di.SushkaConditionColor = getStateAndColorForDetail(d.idDetail, idOrder, 6).Split('|')[1];
                    diList.Add(di);
                }

            }
            return diList;
        }
        private string getStateAndColorForOrder(int idOrder, int idOperation)
        {
            using (var db = new workshopEntities())
            {
                var proccesList = db.process.Where(p => p.Order_idOrder == idOrder && p.Operation_idOperation == idOperation);
                if (proccesList.ToList().Count == 0)
                {
                    return " |" + "WhiteSmoke";
                }
                else
                {
                    process p = proccesList.ToList().First();
                    if (p.dateTimeStartFact == null && p.dateTimeFinishFact == null)
                    {
                        if (p.dateTimeStart.Value > DateTime.Now)
                            return "не почалось|LightGreen";
                        else
                        {
                            if (p.dateTimeFinish.Value > DateTime.Now)
                                return "не почалось|LightPink";
                            else
                                return "не почалось|Red";
                        }
                    }
                    else if (p.dateTimeStartFact != null && p.dateTimeFinishFact == null)
                    {
                        if (p.dateTimeStartFact <= p.dateTimeStart.Value && p.dateTimeFinish.Value > DateTime.Now)
                            return "розпочато|LightGreen";
                        if (p.dateTimeStartFact > p.dateTimeStart.Value && p.dateTimeFinish.Value > DateTime.Now)
                            return "розпочато|Yellow";
                        if (p.dateTimeStartFact > p.dateTimeStart.Value && p.dateTimeFinish.Value < DateTime.Now)
                            return "розпочато|Red";
                        if (p.dateTimeStartFact <= p.dateTimeStart.Value && p.dateTimeFinish.Value < DateTime.Now)
                            return "розпочато|LightPink";
                        return "-----|Blue";
                    }
                    else
                    {
                        if (p.dateTimeStart.Value >= p.dateTimeStartFact && p.dateTimeFinish.Value >= p.dateTimeFinishFact)
                            return "ГОТОВО|LightGreen";
                        if (p.dateTimeStart.Value < p.dateTimeStartFact && p.dateTimeFinish.Value >= p.dateTimeFinishFact)
                            return "ГОТОВО|Yellow";
                        if (p.dateTimeStart.Value >= p.dateTimeStartFact && p.dateTimeFinish.Value < p.dateTimeFinishFact)
                            return "ГОТОВО|LightPink";
                        if (p.dateTimeStart.Value < p.dateTimeStartFact && p.dateTimeFinish.Value < p.dateTimeFinishFact)
                            return "ГОТОВО|Red";
                        return "-----|Blue";
                    }
                }
            }
        }
        private string getStateAndColorForDetail(int idDetail, int idOrder, int idOperation)
        {
            using (var db = new workshopEntities())
            {
                var processList = db.process_has_detail.Where(p => p.Detail_idDetail == idDetail).Select(p => p.process).Where(p => p.Operation_idOperation == idOperation && p.Order_idOrder == idOrder);

                if (processList.ToList().Count == 0)
                {
                    return " |" + "WhiteSmoke";
                }
                else
                {
                    process p = processList.ToList()[0];
                    if (p.dateTimeStartFact == null && p.dateTimeFinishFact == null)
                    {
                        if (p.dateTimeStart > DateTime.Now)
                            return "не почалось|LightGreen";
                        else
                        {
                            if (p.dateTimeFinish > DateTime.Now)
                                return "не почалось|LightPink";
                            else
                                return "не почалось|Red";
                        }
                    }
                    else if (p.dateTimeStartFact != null && p.dateTimeFinishFact == null)
                    {
                        if (p.dateTimeStartFact <= p.dateTimeStart && p.dateTimeFinish > DateTime.Now)
                            return "розпочато|LightGreen";
                        if (p.dateTimeStartFact > p.dateTimeStart && p.dateTimeFinish > DateTime.Now)
                            return "розпочато|Yellow";
                        if (p.dateTimeStartFact > p.dateTimeStart && p.dateTimeFinish < DateTime.Now)
                            return "розпочато|Red";
                        if (p.dateTimeStartFact <= p.dateTimeStart && p.dateTimeFinish < DateTime.Now)
                            return "розпочато|LightPink";
                        return "-----|Blue";
                    }
                    else
                    {
                        if (p.dateTimeStart >= p.dateTimeStartFact && p.dateTimeFinish >= p.dateTimeFinishFact)
                            return "ГОТОВО|LightGreen";
                        if (p.dateTimeStart < p.dateTimeStartFact && p.dateTimeFinish >= p.dateTimeFinishFact)
                            return "ГОТОВО|Yellow";
                        if (p.dateTimeStart >= p.dateTimeStartFact && p.dateTimeFinish < p.dateTimeFinishFact)
                            return "ГОТОВО|LightPink";
                        if (p.dateTimeStart < p.dateTimeStartFact && p.dateTimeFinish < p.dateTimeFinishFact)
                            return "ГОТОВО|Red";
                        return "-----|Blue";
                    }
                }
            }
        }
        public ICommand ShowAddCopmlatedProccessWindow
        {
            get;
            set;
        }
        public ICommand ShowDocumentWindow
        {
            get;
            set;
        }
        public ICommand ShowStatisticWindow
        {
            get;
            set;
        }
        public void AddCopmlatedProccessWindowShow()
        {
            var sw = new AddComplatedProcccessWindow
            {
                DataContext = new AddComplatedProcccessWindowViewModel()
                //(SettingWindowViewModel)this
            };
            sw.ShowDialog();

            if (sw.DialogResult.ToString() == "True")
            {
                string message = "Не вдалося зчитати інформацію:\n";
                int i = 0;
                if (sw.fullInfoFromQRCode.Text == "")
                    message += " " + (i++) + ") відсканируйте QR-код Завдання!\n";
                if (sw.fullInfoFromQRCode.Text == "Такого Завдання не знайдено!!!")
                    message += " " + (i++) + ") Такого Завдання не знайдено!\n";
                if (sw.InfoAboutWorker.Content.ToString() == "Піднесіть свій бейджик до камери!" || sw.InfoAboutWorker.Content.ToString() == "Такого працівника не знайдено!!!")
                    message += " " + (i++) + ") Працівника не знайдено!\n";

                if (i == 0)
                    addStatusProcess(Convert.ToInt32(sw.id_procces.Content), Convert.ToInt32(sw.id_worker.Content));
                else
                    _messageService.ShowAsync(message, "", MessageButton.OK, MessageImage.Error);

            }

        }
        public void documentWindowShow()
        {
            var dw = new DocumentWindow
            {
                DataContext = new DocumentWindowViewModel(_messageService)
            };
            dw.ShowDialog();

        }
        public void statisticWindowShow(worker w, DateTime first, DateTime second)
        {
            var sw = new StatisticWindow
            {
                DataContext = new StatisticWindowViewModel(w, first, second)
            };
            sw.ShowDialog();

        }
        public void changeWorkerAndDatesShow()
        {
            var cw = new ChangeWorkerAndDates
            {
                DataContext = new ChangeWorkerAndDatesViewModel()
            };
            cw.ShowDialog();

            if ((bool)cw.DialogResult)
            {
                DateTime fd = Convert.ToDateTime(cw.FirstDate.SelectedDate);
                DateTime sd = Convert.ToDateTime(cw.SecondDate.SelectedDate);
                worker sw = (worker)cw.WorkersList.SelectedItem;
                statisticWindowShow(sw, fd, sd);
            }

        }
        private void addStatusProcess(int idProcess, int idWorker)
        {
            using (var db = new workshopEntities())
            {
                process oneProcess = db.process.Find(idProcess);
                string message = "";
                if (oneProcess.dateTimeStartFact == null)
                {
                    oneProcess.dateTimeStartFact = DateTime.Now;
                    oneProcess.Worker_idWorker_Fact = idWorker;
                    message = "Старт зафіксовано!";
                    if (oneProcess.dateTimeStartFact.Value < oneProcess.dateTimeStart.Value)
                        message += "\nВи працюєте по плану";
                    else
                        message += "\nВи не встигаєте";
                    message += "\nПриступайте до роботи!!!";
                }
                else if (oneProcess.dateTimeFinishFact == null)
                {
                    oneProcess.dateTimeFinishFact = DateTime.Now;
                    message = "Завершення работи зафіксовано!";
                    if (oneProcess.dateTimeFinishFact.Value < oneProcess.dateTimeFinish.Value)
                        message += "\nВи встигли";
                    else
                        message += "\nВи не встигли";
                    message += "\nДякуємо за виконану роботу!!!";
                }
                else
                    message = "Це завдання вже завершено!!!";

                db.SaveChanges();
                var noComplatedProcesses = db.process.Where(p => (p.Order_idOrder == oneProcess.Order_idOrder && (p.dateTimeFinishFact == null || p.dateTimeStartFact == null))).ToList();
                if (noComplatedProcesses.Count == 0)
                {
                    var order = db.order.Find(oneProcess.Order_idOrder);
                    order.status = true;
                    db.SaveChanges();
                }
                _messageService.ShowAsync(message, "", MessageButton.OK, MessageImage.Information);
            }

            OrderList.Clear();
            OrderList = getOrderList();
        }
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
