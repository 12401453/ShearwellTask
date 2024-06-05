

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ShearwellTask
{
    [QueryProperty(nameof(Groups), "groups")]
    public class GroupSummaryViewModel : INotifyPropertyChanged
    {
        public GroupSummaryViewModel()
        {
        }

        //private List<Group> _groups = new List<Group> { new Group("Cows", GroupFlags.Cow), new Group("Sheep", GroupFlags.Sheep), new Group("Chickens", GroupFlags.Chicken), new Group("Meat animals", GroupFlags.Meat), new Group("Pets", GroupFlags.Pet), new Group("Pigs", GroupFlags.Pig) };
        private List<Group> _groups;
        public List<Group> Groups { 
            get => _groups;
            set
            {
                _groups = value;
                OnPropertyChanged(nameof(Groups));
                
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ICommand GroupTappedCommand => new Command<GroupFlags>(async (GroupFlags group_flag) => {

            List<Animal> selected_group_animals = await App.AnimalsDB.GetAnimalsByGroupAsync(group_flag);
            /*ObservableCollection<Animal> observable_animals = new();
            foreach(var annie in selected_group_animals)
            {
                observable_animals.Add(annie);
            }*/

            string selected_group_name = await App.AnimalsDB.GetGroupNameByGroupFlagAsync(group_flag);
            var selected_group_parameter = new ShellNavigationQueryParameters { { "selected_group_animals", selected_group_animals }, 
                {"selected_group_name", selected_group_name },
                {"selected_group_flag", group_flag } };
            //var selected_group_parameter = new ShellNavigationQueryParameters { { "selected_group_animals", observable_animals }, { "selected_group_name", selected_group_name } };

            await Shell.Current.GoToAsync(nameof(GroupDetailsPage), selected_group_parameter);
        });

        public ICommand GroupSummaryBackCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync("//MainPage");
        });

    }
}
