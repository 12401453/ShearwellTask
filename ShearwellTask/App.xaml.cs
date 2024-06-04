namespace ShearwellTask
{
    public partial class App : Application
    {
        public static AnimalsDatabase AnimalsDB { get; private set; }
        public App(AnimalsDatabase db)
        {
            InitializeComponent();

            MainPage = new AppShell();
            AnimalsDB = db;
        }
    }
}
