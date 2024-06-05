
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ShearwellTask
{
    [QueryProperty(nameof(SelectedGroupAnimals), "selected_group_animals")]
    [QueryProperty(nameof(SelectedGroupName), "selected_group_name")]
    [QueryProperty(nameof(SelectedGroupFlag), "selected_group_flag")]
    public partial class GroupDetailsViewModel : INotifyPropertyChanged
    {
        public GroupDetailsViewModel() { }

        private Guid selected_group_guid;
        
        /*
        public Guid SelectedGroupGuid { 
            get => selected_group_guid; 
            set 
            {
                selected_group_guid = value;
                OnPropertyChanged(nameof(SelectedGroupGuid));
            } 
        } */

        public GroupFlags SelectedGroupFlag { get; set; }

        private string selected_group_name;
        public string SelectedGroupName { get => selected_group_name; 
            set
            {
                selected_group_name = value;
                OnPropertyChanged(nameof(SelectedGroupName));
            } 
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private string guid_string;
        public string GuidString { get => guid_string;
            set
            {
                guid_string = value;
                OnPropertyChanged(nameof(GuidString));
            }
        }


        public ICommand RemoveAnimalFromGroup => new Command<Animal>(async (Animal removed_animal) =>
        {
            //this is totally stupid but I can't get ObservableCollections to work and this is the only way I can get the lists to update dynamically
            SelectedGroupAnimals.Remove(removed_animal);
            var new_selected_group = SelectedGroupAnimals;
            SelectedGroupAnimals = new List<Animal>();
            SelectedGroupAnimals = new_selected_group;

            removed_animal.RemoveGroup(SelectedGroupFlag);
            await App.AnimalsDB.UpdateAnimalAsync(removed_animal);
        });

        public ICommand DeleteAnimalEntirely => new Command<Animal>(async (Animal deleted_animal) =>
        {
            //this is totally stupid but I can't get ObservableCollections to work and this is the only way I can get the lists to update dynamically
            SelectedGroupAnimals.Remove(deleted_animal);
            var new_selected_group = SelectedGroupAnimals;
            SelectedGroupAnimals = new List<Animal>();
            SelectedGroupAnimals = new_selected_group;

            await App.AnimalsDB.DeleteAnimalAsync(deleted_animal.Id);
        });

        public ICommand GroupDetailsBackCommand => new Command(async () =>
        {
            //I need to remake the GroupSummaryPage with the updated data when you use the back-button, but the GoToAsync() navigation here plays a forwards-animation which isn't good
            var groups = await App.AnimalsDB.GetGroupsAsync();
            List<int> group_membership_counts = await App.AnimalsDB.GetGroupsMemberCount();

            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].AnimalsCount = group_membership_counts[i];
            }

            var groups_parameter = new ShellNavigationQueryParameters { { "groups", groups } };
            await Shell.Current.GoToAsync(nameof(GroupSummaryPage), groups_parameter);
        });


        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        


        private List<Animal> selected_group_animals;
        public List<Animal> SelectedGroupAnimals
        {
            get => selected_group_animals;
            set
            {
                selected_group_animals = value;
                OnPropertyChanged(nameof(SelectedGroupAnimals));
            }
        }
        
    }
}
