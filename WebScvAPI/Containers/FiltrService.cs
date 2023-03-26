using System.Text.RegularExpressions;
using System.Text;
using WebScvAPI.Models;

namespace WebScvAPI.Containers
{
    public interface ICSVServiceFiltr
    {
        public Task<CSVFile> Filtr(string filePath, CsvModel model, Dictionary<string, double[]> filtrnumber, Dictionary<string, string>
            filtrsting, Dictionary<string, string> sort);
    }
    public class CSVServiceFiltr : ICSVServiceFiltr
    {
        public async Task<CSVFile> Filtr(string filePath, CsvModel model, Dictionary<string, double[]> filtrnumber, Dictionary<string, string>
            filtrsting, Dictionary<string, string> sort)
        {
            int count = 0;
            Dictionary<int, double[]> numbvalues = new Dictionary<int, double[]>();
            Dictionary<int, string> strvalues = new Dictionary<int, string>();
            Dictionary<int, string> sorted = new Dictionary<int, string>();

            var lines = File.ReadLines(filePath, Encoding.Default);
            string firstline = lines.ElementAt(0);
            var keys = lines.ElementAt(0).Split(',');
            for (int i = 0; i < keys.Length; i++)
            {
                if (filtrnumber.ContainsKey(keys[i])) numbvalues[i] = filtrnumber[keys[i]];
                if (filtrsting.ContainsKey(keys[i])) strvalues[i] = filtrsting[keys[i]];
                if (sort.ContainsKey(keys[i])) sorted[i] = sort[keys[i]];

            }
            var data = lines
                       .Skip(1)
                       .Select(l => new { Fields = l.Split(','), Line = l })

                       .Select(x => x.Line);
            foreach (int i in numbvalues.Keys)
            {
                data = data.Select(l => new { Fields = l.Split(','), Line = l })
                .Where(p => ((Convert.ToDouble(p.Fields[i]) >= numbvalues[i][0]) && (Convert.ToDouble(p.Fields[i]) <= numbvalues[i][1]))).Select(x => x.Line);
            }
            foreach (int i in strvalues.Keys)
            {

                data = data.Select(l => new { Fields = l.Split(','), Line = l })
                .Where(p => Regex.IsMatch(String.Format(@"{0}", p.Fields[0]), strvalues[i], RegexOptions.IgnoreCase)).Select(x => x.Line);
            }

            var data2 = data.Select(l => new CSVLine { Fields = l.Split(','), Line = l });
            IOrderedEnumerable<CSVLine> ordered = data2.OrderBy(p => p.Line);
            foreach (int i in sorted.Keys)
            {
                if (sorted[i] == "ASC")
                    ordered = ordered.
                        ThenBy(p => p.Fields[i]);
                else
                    ordered = ordered.
                        ThenByDescending(p => p.Fields[i]);
            }
            List<string> final = ordered.Select(p => p.Line).ToList();

            StringBuilder csvExport = new StringBuilder();
            csvExport.AppendLine(firstline);
            foreach (string i in final) csvExport.AppendLine(i);
            CSVFile file = new CSVFile();
            file.File_type = "text/csv";
            file.File_name = model.Name + ".csv";
            file.Finaldata = new ASCIIEncoding().GetBytes(csvExport.ToString());
            return file;

        }
    }
}
