namespace MyCourseWork.Views
{
    using Catel.Windows;
    using ViewModels;

    public partial class StatisticWindow
    {
        public StatisticWindow()
            : this(null) { }

        public StatisticWindow(StatisticWindowViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        private void ListView_Selected(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
