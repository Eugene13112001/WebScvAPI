using System.Collections;
using System.Globalization;
using System.IO;
using WebScvAPI.Models;
using static System.Net.Mime.MediaTypeNames;
using DnsClient.Protocol;
using System.Text.RegularExpressions;
using SharpCompress.Compressors.Xz;
using SharpCompress.Common;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Collections.Generic;

namespace WebScvAPI.Containers
{
    public interface ICSVServiceReader
    {
        public Task<CsvModel> ReadCSV (Stream file, string path, string name);
    }
    public class CSVServiceReader : ICSVServiceReader
    {
        public async Task<CsvModel>  ReadCSV (Stream file, string path, string name)
        {
            int count = 0;
            Dictionary<string, string> values = new Dictionary<string, string> ();
            string[] keys = new string[] {}; 
            using (StreamReader reader = new StreamReader(file))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {

                    string[] words = line.Split(new char[] { ',' });
                    
                    if (count == 0) keys = line.Split(new char[] { ',' });
                    if (keys.Length == 0) throw new Exception("Файл пустой");
                    if (count > 0)
                    {
                        if (words.Length != keys.Length) throw new Exception(String.Format("В файле ошибка на {0}  строке", (count+1)));
                        for (int i=0; i < keys.Length; i++)
                        {
                            
                            values[keys[i]] = "string";
                            Regex regex = new Regex(@"( ){0,}\d+(.\d+){0,1}( ){0,}");
                            MatchCollection matches = regex.Matches(words[i]);
                            if (matches.Count != 1) continue;
                            if ( words[i].Replace(  matches.First().Value , "").Length != 0) continue;
                            values[keys[i]] = "number";

                        }
                    }

                    count += 1;
                }
            }
            if (values.Count == 0) throw new Exception("Файл пустой");
            return new CsvModel { Path = String.Format("files/{0}.csv", path) , Values = values , Name = name};
        }
    }

    public interface ICSVServiceWriter
    {
        public Task<string> WriteCSV(Stream file, string path);
    }
    public class CSVServiceWriter : ICSVServiceWriter
    {
        public async Task<string> WriteCSV(Stream file, string path)
        {
            StreamReader reader = new StreamReader(file);
            string text = reader.ReadToEnd();
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync(text);
            }
            
            return path;
        }
    }
    
    
}
