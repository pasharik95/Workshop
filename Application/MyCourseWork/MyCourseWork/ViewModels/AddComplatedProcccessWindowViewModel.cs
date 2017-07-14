namespace MyCourseWork.ViewModels
{
    using Catel.Data;
    using Catel.MVVM;
    using System.Threading.Tasks;

    public class AddComplatedProcccessWindowViewModel : ViewModelBase
    {
        public AddComplatedProcccessWindowViewModel()
        {
        }

        public string InfoAboutProccess
        {
            get { return GetValue<string>(InfoAboutProccessProperty); }
            set { SetValue(InfoAboutProccessProperty, value); }
        }


        public static readonly PropertyData InfoAboutProccessProperty = RegisterProperty("InfoAboutProccess", typeof(string));




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
