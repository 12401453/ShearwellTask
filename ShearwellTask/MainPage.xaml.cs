namespace ShearwellTask;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
        AppShell.SetNavBarIsVisible(this, false);
    }

    //These buttons only exist so I can get database-stored data into the groups-summary and animals-summary pages, which I can't easily do on the first page of the app due to needing the database functions to all be async, so I am implementing them as simple event-handlers
    private async void GroupsButtonPressed(object sender, EventArgs e)
    {
        var groups = await App.AnimalsDB.GetGroupsAsync();
        List<int> group_membership_counts = await App.AnimalsDB.GetGroupsMemberCount();

        for(int i = 0; i < groups.Count; i++)
        {
            groups[i].AnimalsCount = group_membership_counts[i];
        }

        var groups_parameter = new ShellNavigationQueryParameters { { "groups", groups } };
        await Shell.Current.GoToAsync(nameof(GroupSummaryPage), groups_parameter);
    }

    private async void AnimalsButtonPressed(object sender, EventArgs e)
    {

    }
}