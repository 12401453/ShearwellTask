using SQLite;

namespace ShearwellTask
{
    [Table("animals")]
    public class Animal
    {
        public Animal(string tag, DateTime dob, GroupFlags groupMembership = 0)
        {
            this.Id = Guid.NewGuid();
            this.Tag = tag;
            this.DateOfBirth = dob;
            this.GroupMembership = groupMembership;
        }

        //the ORMs require classes to have a parameterless constructor
        public Animal() 
        { 
        }

        [PrimaryKey]
        public Guid Id { get; set; }
        [Unique]
        public string Tag { get; set; }

        public DateTime DateOfBirth { get; set; }

        public GroupFlags GroupMembership { get; set; }

        [Ignore]
        public string DateOfBirthString { get => DateOfBirth.ToString("d"); }
        [Ignore]
        public bool EvenOrOdd { get; set; }

        void AddGroup(GroupFlags group)
        {
            GroupMembership = GroupMembership | group;
        }

        void RemoveGroup(GroupFlags group)
        {                                       //this bracketed bit equals zero if the Animal does not already have the `group` flag assigned, and anything XOR'd with zero is unchanged
            GroupMembership = GroupMembership ^ (GroupMembership & group);
        }


    }
}
