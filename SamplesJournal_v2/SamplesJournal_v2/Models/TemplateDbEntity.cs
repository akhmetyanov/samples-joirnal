using System;
using SQLite;

namespace SamplesJournal_v2.Models
{
    public class TemplateDbEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string TemplateJSON { get; set; }
    }
}
