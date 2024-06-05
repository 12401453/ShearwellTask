namespace ShearwellTask
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(GroupDetailsPage), typeof(GroupDetailsPage));
            Routing.RegisterRoute(nameof(GroupSummaryPage), typeof(GroupSummaryPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(AddPage), typeof(AddPage));
        }
    }
}
