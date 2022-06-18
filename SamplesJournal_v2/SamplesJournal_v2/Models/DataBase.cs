using System.Threading;
using System.Threading.Tasks;
using SQLite;
using System.Text.Json;
using System.Collections.Generic;
using SamplesJournal_v2.Models.Editor;
using System;
using System.IO;

namespace SamplesJournal_v2.Models
{
    public class DataBase
    {
        private SQLiteAsyncConnection _database;
        private string _dbPath;

        public DataBase(string dbPath)
        {
            _dbPath = dbPath;
            createDatabase(_dbPath);
            Set();
        }
        public bool createDatabase(string dbPath)
        {
            try
            {
                using (var connection = new SQLiteConnection(dbPath))
                {
                    connection.CreateTable<TemplateDbEntity>();
                    connection.CreateTable<FileDbEntity>();

                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                return false;
            }
        }

        void Set()
        {
            while (!File.Exists(_dbPath))
            {

            }

            _database = new SQLiteAsyncConnection(_dbPath, SQLiteOpenFlags.ReadWrite);
        }

        #region template
        public List<Template.Template> GetTemplates()
        {
            var ret = new List<Template.Template>();

            var dbs = _database.Table<TemplateDbEntity>().ToListAsync().Result;

            foreach (var dbsItem in dbs)
            {
                ret.Add(
                    JsonSerializer.Deserialize<Template.Template>(dbsItem.TemplateJSON)
                    );
            }

            return ret;
        }

        public Task<int> InsertOrReplaceTemplate(Template.Template template)
        {
            var dbEntity = new TemplateDbEntity();
            dbEntity.Id = template.Id;
            dbEntity.TemplateJSON = JsonSerializer.Serialize(template);
            return _database.InsertOrReplaceAsync(dbEntity);
        }

        public Task<int> DeleteTemplate(Template.Template template)
        {
            var dbEntity = new TemplateDbEntity();
            dbEntity.Id = template.Id;
            dbEntity.TemplateJSON = JsonSerializer.Serialize(template);

            return _database.DeleteAsync(dbEntity);
        }
        #endregion

        #region file
        public List<EditorFile> GetFiles()
        {
            var ret = new List<EditorFile>();

            var dbs = _database.Table<FileDbEntity>().ToListAsync().Result;

            foreach (var dbsItem in dbs)
            {
                ret.Add(
                    JsonSerializer.Deserialize<EditorFile>(dbsItem.FileJSON)
                    );
            }

            return ret;
        }

        public Task<int> InsertOrReplaceFile(EditorFile file)
        {
            var dbEntity = new FileDbEntity();
            dbEntity.Id = file.Id;
            dbEntity.FileJSON = JsonSerializer.Serialize(file);
            return _database.InsertOrReplaceAsync(dbEntity);
        }

        public Task<int> DeleteFile(EditorFile file)
        {
            var dbEntity = new FileDbEntity();
            dbEntity.Id = file.Id;
            dbEntity.FileJSON = JsonSerializer.Serialize(file);

            return _database.DeleteAsync(dbEntity);
        }
        #endregion
    }
}
