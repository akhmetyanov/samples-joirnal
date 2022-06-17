using System;
using SQLite;

namespace SamplesJournal_v2.Models
{
    public class FileDbEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string FileJSON { get; set; }
    }
}
