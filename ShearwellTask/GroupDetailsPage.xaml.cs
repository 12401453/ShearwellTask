namespace ShearwellTask;

public partial class GroupDetailsPage : ContentPage
{
	public GroupDetailsPage(GroupDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        AppShell.SetNavBarIsVisible(this, false);
    }
}