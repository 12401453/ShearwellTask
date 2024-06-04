using SQLite;


namespace ShearwellTask
{
    [Table("groups")]
    public class Group
    {
        public Group(string name, GroupFlags groupFlag)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.DateCreated = DateTime.Now;
            this.GroupFlag = groupFlag;
        }

        //the ORMs require classes to have a parameterless constructor
        public Group()
        {
        }

        [PrimaryKey]
        public Guid Id { get; set; }

        [Unique]
        public string Name { get; set; }

        [Unique]
        public GroupFlags GroupFlag { get; set; }


        public DateTime DateCreated { get; set; }

        [Ignore]
        public int AnimalsCount { get; set; } = 0;
        [Ignore]
        public string DateString { get => DateCreated.ToString("g"); }
        [Ignore]
        public string IdString { get => Id.ToString(); }
    }
}
