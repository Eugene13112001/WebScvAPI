using MongoDB.Driver;
using WebScvAPI.Settings;
using WebScvAPI.Models;
using MongoDB.Bson;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace WebScvAPI.Containers
{
    public interface ICSVServiceData
    {
        public Task<List<CsvModel>> Get();

        public Task<CsvModel?> GetByName(string Name);


        public Task<CsvModel?> GetById(string id);

        public Task<CsvModel> Create(CsvModel model);


        public Task<CsvModel> Update(string id, CsvModel model);


        public Task<bool> Remove(string id);
    }
                
    public class CSVServiceData: ICSVServiceData
    {
        public  IMongoCollection<CsvModel> files;

        public CSVServiceData(ICSVDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);



            var database = client.GetDatabase(settings.DatabaseName);

            files = database.GetCollection<CsvModel>(settings.CollectionName);
        }

        public async  Task<List<CsvModel>> Get()
        {
            return await  files.Find(book => true).ToListAsync();
        }
        public async Task<CsvModel?> GetByName(string Name)
        {
            var a = await files.FindAsync<CsvModel>(model => model.Name == Name);
            if (a == null) return null;
            return a.FirstOrDefault();
        }
           
        public async Task<CsvModel?> GetById(string id) 
        {
            var a = await files.FindAsync<CsvModel>(model => model.Id == id);
            if (a == null) return null;
            return a.FirstOrDefault();
        }

    public async Task<CsvModel> Create(CsvModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = Guid.NewGuid().ToString();
            }
            await files.InsertOneAsync(model);
            return model;
        }

        public async Task<CsvModel> Update(string id, CsvModel model)
        {
            await files.ReplaceOneAsync(book => book.Id == id, model);
            return model;
        }

        public async Task<bool> Remove(string id)
        {
            await files.DeleteOneAsync(book => book.Id == id);
            return true;
        }

        
    }
}
