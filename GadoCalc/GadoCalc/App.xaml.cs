using Xamarin.Forms;
using GadoCalc.Views;

namespace GadoCalc
{
    public partial class App : Application
    {
        static ParametersDatabase database;

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        public static ParametersDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ParametersDatabase();
                }
                return database;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
