

using System.ComponentModel;
using System.Windows.Input;

namespace ShearwellTask
{
    public class AddViewModel : INotifyPropertyChanged

    {
        public AddViewModel()
        {
            SelectedDate = DateTime.Now;
        }

        //this would be less code if I used the [ObservableProperty] source-generators from the MVVM-toolkit but that hides too much implementation for me to be comfortable with at this stage and I don't know how it will affect other things
        private string _tagFirstPart = string.Empty;
        public string TagFirstPart { get => _tagFirstPart;
            set 
            {
                _tagFirstPart = value;
                OnPropertyChanged(nameof(TagFirstPart));
            } 
        }
        private string _tagSecondPart = string.Empty;
        public string TagSecondPart
        {
            get => _tagSecondPart;
            set
            {
                _tagSecondPart = value;
                OnPropertyChanged(nameof(TagSecondPart));
            }
        }

        private bool _cowChecked;
        public bool CowChecked { 
            get => _cowChecked; 
            set
            { 
                _cowChecked = value; 
                OnPropertyChanged(nameof(CowChecked)); 
            } 
        }
        private bool _sheepChecked;
        public bool SheepChecked
        {
            get => _sheepChecked;
            set
            {
                _sheepChecked = value;
                OnPropertyChanged(nameof(SheepChecked));
            }
        }
        private bool _chickenChecked;
        public bool ChickenChecked
        {
            get => _chickenChecked;
            set
            {
                _chickenChecked = value;
                OnPropertyChanged(nameof(ChickenChecked));
            }
        }
        private bool _pigChecked;
        public bool PigChecked
        {
            get => _pigChecked;
            set
            {
                _pigChecked = value;
                OnPropertyChanged(nameof(PigChecked));
            }
        }
        private bool _meatChecked;
        public bool MeatChecked
        {
            get => _meatChecked;
            set
            {
                _meatChecked = value;
                OnPropertyChanged(nameof(MeatChecked));
            }
        }
        private bool _petChecked;
        public bool PetChecked
        {
            get => _petChecked;
            set
            {
                _petChecked = value;
                OnPropertyChanged(nameof(PetChecked));
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public ICommand AddNewAnimal => new Command(async () => {
            bool[] role_groups_checked = [PetChecked, MeatChecked];
            bool[] species_groups_checked = [CowChecked, SheepChecked, PigChecked, ChickenChecked];

            //this is making a bitmask out of the radio-buttons
            int role_flag_value = 0;
            for (int i = 0; i < role_groups_checked.Length; i++)
            {
                if (role_groups_checked[i] == true) role_flag_value = 1 << i;
            }
            int species_flag_value = 0;
            for (int i = 0; i < species_groups_checked.Length; i++)
            {
                if (species_groups_checked[i] == true) species_flag_value = 1 << i + 2;
            }
            GroupFlags role_group_flag = (GroupFlags)role_flag_value;
            GroupFlags species_group_flag = (GroupFlags)species_flag_value;

            string tag = "GB" + TagFirstPart + " " + TagSecondPart;
            Animal new_animal = new Animal(tag, SelectedDate, role_group_flag | species_group_flag);

            await App.AnimalsDB.InsertNewAnimalAsync(new_animal);


            var groups = await App.AnimalsDB.GetGroupsAsync();
            List<int> group_membership_counts = await App.AnimalsDB.GetGroupsMemberCount();

            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].AnimalsCount = group_membership_counts[i];
            }
            var groups_parameter = new ShellNavigationQueryParameters { { "groups", groups } };
            await Shell.Current.GoToAsync(nameof(GroupSummaryPage), groups_parameter);
        });
        
        
        
        
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
