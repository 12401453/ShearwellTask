namespace ShearwellTask;

public partial class GroupDetailsPage : ContentPage
{
	public GroupDetailsPage(GroupDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        //AppShell.SetNavBarIsVisible(this, false);
    }

    private async void AddButtonPressed(object sender, EventArgs e)
    {
        //AddViewModel add_vm = new AddViewModel();
        //await Navigation.PushModalAsync(new AddPage(add_vm));
        await Shell.Current.GoToAsync(nameof(AddPage));
    }
}