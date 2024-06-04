
using System.ComponentModel;

namespace ShearwellTask
{
    [QueryProperty(nameof(SelectedGroupAnimals), "selected_group_animals")]
    [QueryProperty(nameof(SelectedGroupName), "selected_group_name")]
    public partial class GroupDetailsViewModel : INotifyPropertyChanged
    {
        public GroupDetailsViewModel() { }

        private Guid selected_group_guid;
        public Guid SelectedGroupGuid { 
            get => selected_group_guid; 
            set 
            {
                selected_group_guid = value;
                OnPropertyChanged(nameof(SelectedGroupGuid));
            } 
        }

        private string selected_group_name;
        public string SelectedGroupName { get => selected_group_name; 
            set
            {
                selected_group_name = value;
                OnPropertyChanged(nameof(SelectedGroupName));
            } 
        }

       /* private GroupFlags selected_group_flag;
        public GroupFlags SelectedGroupFlag
        {
            get => selected_group_flag;
            set
            {
                selected_group_flag = value;
                switch(value)
                {
                    case GroupFlags.Pet:
                        SelectedGroupName = "Pets";
                        break;
                    case GroupFlags.Meat:
                        SelectedGroupName = "Meat Animals";
                        break;
                    case GroupFlags.Cow:
                        SelectedGroupName = "Cows";
                        break;
                    case GroupFlags.Chicken:
                        SelectedGroupName = "Chickens";
                        break;
                    case GroupFlags.Pig:
                        SelectedGroupName = "Pigs";
                        break;
                    case GroupFlags.Sheep:
                        SelectedGroupName = "Sheep";
                        break;

                }
                OnPropertyChanged(nameof(SelectedGroupFlag));
            }
        } */


        public event PropertyChangedEventHandler? PropertyChanged;

        private string guid_string;
        public string GuidString { get => guid_string;
            set
            {
                guid_string = value;
                OnPropertyChanged(nameof(GuidString));
            }
        }

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private List<Animal> _group_animals = new List<Animal> { new Animal("GB0000000 00001", new DateTime(2021, 3, 14), GroupFlags.Pet), new Animal("GB0000000 00002", new DateTime(2021, 4, 11), GroupFlags.Pig) };
         public List<Animal> GroupAnimals { get => _group_animals; }


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
