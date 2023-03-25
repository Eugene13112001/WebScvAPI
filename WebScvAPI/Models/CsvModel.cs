using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace WebScvAPI.Models
{
    public class CsvModel
    {
        [BsonId]
        
        public string Id { get; set; }

        public string Path { get; set; }
       
        public string Name { get; set; }
        [BsonDictionaryOptions(DictionaryRepresentation.Document)]
        public Dictionary<string, string> Values { get; set; }


    }
}
