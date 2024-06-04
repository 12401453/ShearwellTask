
//I'm not sure whether this task requires the ability to add/delete groups as well as animals so I've just assumed that groups are pre-determined and unchangeable; if I wanted to make it possible to add or remove groups I would probably get rid of this enum and use a straight int as the bitflag, then get the rowid of the newly-inserted group from the groups table and then bitshift the number 1 left by the rowid (1 << rowid) to get me a new flag, however this would mean I'd run out of groups after 30 because ints are 32 bit and signed, and I would have to come up with a way to reuse bitflags from groups that had been deleted.
namespace ShearwellTask
{
    public enum GroupFlags
    {
        Pet = 0b000001,
        Meat = 0b000010,
        Cow = 0b000100,
        Sheep = 0b001000,
        Pig = 0b010000,
        Chicken = 0b100000,
    }
}