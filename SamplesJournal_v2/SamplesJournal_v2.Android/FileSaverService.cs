using SamplesJournal_v2.Models.Editor;
using SamplesJournal_v2.Models.Template;
using SamplesJournal_v2.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(SamplesJournal_v2.Droid.FileSaverService))]

namespace SamplesJournal_v2.Droid
{
    public class FileSaverService : IFileSaverService
    {
        public string SaveFile(EditorFile file)
        {
            string json = JsonSerializer.Serialize(file);

            try
            {
                return save("Files", file.Name, json, "json");
            }
            catch
            {
                return "";
            }

        }

        public string SaveFileToScv(EditorFile file)
        {

            var csv = new StringBuilder();

            var headersName = file.Template.Headers.Select(h => h.Name).ToList();
            headersName.Add("EditedDate");

            csv.AppendLine(string.Join(';', headersName));

            var rows = collectAllRows(file.Node);

            foreach (var row in rows)
            {
                var stringRow = row.Values.Select(v => v.Value.ToString()).ToList();
                
                if (row.Edited)
                {
                    stringRow.Add(row.EditedDate.ToString("dd/MM/yyyy HH:mm"));
                }
                else
                {
                    stringRow.Add("-");
                }
                

                csv.AppendLine(string.Join(';', stringRow));
            }

            var filenameSplitted = file.Name.Split('.');

            string filename = "";

            if (filenameSplitted.Length > 1)
            {
                if (filenameSplitted[filenameSplitted.Length - 1] == "csv")
                {
                    for (int i = 0; i < filenameSplitted.Length - 1; i++)
                    {
                        filename += filenameSplitted[i];
                    }
                }
            }
            else
            {
                filename = file.Name;
            }

            try
            {
                return save("Files", filename, csv.ToString(), "csv");
            }
            catch
            {
                return "";
            }

        }

        List<EditorFileRow> collectAllRows(EditorFileRowsNode Node)
        {
            var retArr = new List<EditorFileRow>();

            if (Node.Childs.Count != 0)
            {
                foreach (var node in Node.Childs)
                {
                    retArr.AddRange(collectAllRows(node));
                }
            } 
            else
            {
                retArr = Node.Rows;
            }

            return retArr;
        }

        public string SaveTemplate(Template template)
        {            
            string json = JsonSerializer.Serialize(template);
            
            try
            {
                return save("Templates", template.Name, json, "json");
            }
            catch
            {
                return "";
            }
        }

        string save(string folderName, string filename, string text, string extension)
        {
            var templatesFolderPath = Path.Combine(GetRoot(), folderName);

            if (!Directory.Exists(templatesFolderPath))
            {
                Directory.CreateDirectory(templatesFolderPath);
            }

            var filePath = Path.Combine(templatesFolderPath, filename + "." + extension);

            if (Directory.Exists(filePath))
            {
                Directory.Delete(filePath);
            }

            System.IO.File.WriteAllText(filePath, text);

            return filePath;
        }

        string GetRoot()
        {
            var root =  Android.OS.Environment
                .GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
            var samplesJournalFolderPath = Path.Combine(root, "SamplesJournal");

            if (!Directory.Exists(samplesJournalFolderPath))
            {
                Directory.CreateDirectory(samplesJournalFolderPath);
            }

            return samplesJournalFolderPath;
        }
    }
}