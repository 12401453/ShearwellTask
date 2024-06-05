

using Microsoft.Maui.Controls;
using SQLite;

namespace ShearwellTask
{
    //The structure of this class is copied to a great extent from the learn.microsoft.com website
    public class AnimalsDatabase
    {
        string _dbPath;
        SQLiteAsyncConnection? db_conn;

        public AnimalsDatabase(string dbPath) { _dbPath = dbPath; }
        public string StatusMessage { get; set; } = "";

      
        async Task InitAsync()
        {
            if (db_conn != null) return;

            db_conn = new SQLiteAsyncConnection(_dbPath);

            await db_conn.CreateTableAsync<Animal>();//includes IF EXISTS so should be OK to use even after the DB and table already exist 

            //this was test initialisation data
            /* await db_conn.DeleteAllAsync<Animal>();

             var test_db_animals = new List<Animal> { new Animal("GB0000000 00001", new DateTime(2021, 3, 14), GroupFlags.Pet), new Animal("GB0000000 00002", new DateTime(2021, 4, 11), GroupFlags.Pig), new Animal("GB0000000 00003", new DateTime(2011, 2, 14), GroupFlags.Pig), new Animal("GB0000000 00004", new DateTime(2021, 4, 11), GroupFlags.Cow), new Animal("GB0000000 00005", new DateTime(2021, 3, 14), GroupFlags.Pet | GroupFlags.Cow), new Animal("GB0000000 00006", new DateTime(2021, 4, 11), GroupFlags.Pig), new Animal("GB0000000 00007", new DateTime(2021, 3, 14), GroupFlags.Chicken | GroupFlags.Meat), new Animal("GB0000000 00008", new DateTime(2021, 4, 11), GroupFlags.Pig | GroupFlags.Meat), new Animal("GB0000000 00009", new DateTime(2021, 3, 14), GroupFlags.Pet), new Animal("GB0000000 00010", new DateTime(2021, 4, 11), GroupFlags.Pig), new Animal("GB0000000 00011", new DateTime(2021, 3, 14), GroupFlags.Pet), new Animal("GB0000000 00012", new DateTime(2021, 4, 11), GroupFlags.Pig), new Animal("GB0000000 00013", new DateTime(2021, 3, 14), GroupFlags.Pet | GroupFlags.Sheep), new Animal("GB0000000 00014", new DateTime(2021, 4, 11), GroupFlags.Pig | GroupFlags.Pet), new Animal("GB0000000 00015", new DateTime(2021, 3, 14), GroupFlags.Sheep), new Animal("GB0000000 00016", new DateTime(2021, 4, 11), GroupFlags.Chicken) };

             await db_conn.InsertAllAsync(test_db_animals); */



            //I'm interpreting the task as allowing a static list of groups so having them in the database isn't really necessary, and adding them to the database at runtime definitely shouldn't be necessary as I could just add them to the DB file manually beforehand, but I don't really know where the database file is or what the various platforms will do to it so I've got this code just to be on the safe side

            await db_conn.QueryAsync<Group>("DROP TABLE IF EXISTS groups"); //this wouldn't be necessary if the ORM's Insert wrappers had an option for INSERT OR IGNORE 
             await db_conn.CreateTableAsync<Group>();
             await db_conn.InsertAllAsync(new List<Group> { new Group("Cows", GroupFlags.Cow), new Group("Sheep", GroupFlags.Sheep), new Group("Chickens", GroupFlags.Chicken), new Group("Pigs", GroupFlags.Pig), new Group("Pets", GroupFlags.Pet), new Group("Meat Animals", GroupFlags.Meat) });
        }

        public async Task<List<int>> GetGroupsMemberCount()
        {
            if (db_conn == null) await InitAsync();
            try
            {
                List<int> groupMembersCount = new List<int>();
                
                //this is an absolutely ridiculous way of doing what should be a simple SELECT COUNT(*) query and I honestly can't believe that sqlite-net forces you to do this instead of providing a simpler to just count rows
                var cows = await db_conn.QueryAsync<Animal>("SELECT * FROM animals WHERE (GroupMembership & ?) > 0", GroupFlags.Cow);
                
                groupMembersCount.Add(cows.Count);
                var sheep = await db_conn.QueryAsync<Animal>("SELECT * FROM animals WHERE (GroupMembership & ?) > 0", GroupFlags.Sheep);
                groupMembersCount.Add(sheep.Count);
                var chickens = await db_conn.QueryAsync<Animal>("SELECT * FROM animals WHERE (GroupMembership & ?) > 0", GroupFlags.Chicken);
                groupMembersCount.Add(chickens.Count);
                var pigs = await db_conn.QueryAsync<Animal>("SELECT * FROM animals WHERE (GroupMembership & ?) > 0", GroupFlags.Pig);
                groupMembersCount.Add(pigs.Count);
                var pets = await db_conn.QueryAsync<Animal>("SELECT * FROM animals WHERE (GroupMembership & ?) > 0", GroupFlags.Pet);
                groupMembersCount.Add(pets.Count);
                var meat_animals = await db_conn.QueryAsync<Animal>("SELECT * FROM animals WHERE (GroupMembership & ?) > 0", GroupFlags.Meat);
                groupMembersCount.Add(meat_animals.Count);
                
                return groupMembersCount;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve animals in GetGroupsMemberCount() call with error-message: {0}", ex.Message);

                return new List<int>();
            }
        }

        public async Task<List<Animal>> GetAnimalsByGroupAsync(GroupFlags group)
        {
            if (db_conn == null) await InitAsync();
            try
            {
                return await db_conn.QueryAsync<Animal>("SELECT * FROM animals WHERE (GroupMembership & ?) > 0", group);
            }
            catch(Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve animals in GetAnimalsByGroup() call with error-message: {0}", ex.Message);

                return new List<Animal>();
            }
        }

        public async Task<string> GetGroupNameByGroupFlagAsync(GroupFlags group_flag)
        {
            if (db_conn == null) await InitAsync();
            try
            {
                List<Group> groups = await db_conn.QueryAsync<Group>("SELECT * FROM groups WHERE GroupFlag = ?", group_flag);
                return groups[0].Name;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve group in GetGroupNameByGroupFlag() call with error-message: {0}", ex.Message);

                return string.Empty;
            }
        }

        public async Task<List<Group>> GetGroupsAsync()
        {
            if (db_conn == null) await InitAsync();
            try
            {
                return await db_conn.Table<Group>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve groups in GetGroupsAsync() call with error-message: {0}", ex.Message);

                return new List<Group>();
            }
        }

        public async Task<string> InsertNewAnimalAsync(Animal new_animal)
        {
            int result_code = 0;
            if (db_conn == null) await InitAsync();
            try
            {
                result_code = await db_conn.InsertAsync(new_animal);
                StatusMessage = string.Format("Attempt to insert animal with tag {0} in InsertNewAnimal() returned the SQLite result-code {1}", new_animal.Tag, result_code);
                return string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to insert animal with tag {0} in InsertNewAnimal() call with error-message: {1}", new_animal.Tag,  ex.Message);
                return ex.Message;
            }
        }

        public async Task DeleteAnimalAsync(Guid animal_guid)
        {
            int result_code = 0;
            if (db_conn == null) await InitAsync();
            try
            {
                result_code = await db_conn.DeleteAsync<Animal>(animal_guid);
                StatusMessage = string.Format("Attempt to delete animal with guid {0} in DeleteAnimal() returned the SQLite result-code {1}", animal_guid, result_code);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete animal with guid {0} in DeleteAnimal() call with error-message: {1}", animal_guid, ex.Message);
            }
        }

        public async Task UpdateAnimalAsync(Animal updated_animal)
        {
            int result_code = 0;
            if (db_conn == null) await InitAsync();
            try
            {
                result_code = await db_conn.UpdateAsync(updated_animal);
                StatusMessage = string.Format("Attempt to update animal with guid {0} in UpdateAnimal() returned the SQLite result-code {1}", updated_animal.Id, result_code);
            }
            catch(Exception ex)
            {
                StatusMessage = string.Format("Failed to update animal with guid {0} in UpdateAnimal() call with error-message: {1}", updated_animal.Id, ex.Message);
            }
        }
    }
}
