namespace MyCourseWork.Views
{
    using ViewModels;

    public partial class DocumentWindow
    {
        public DocumentWindow()
            : this(null) { }

        public DocumentWindow(DocumentWindowViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
