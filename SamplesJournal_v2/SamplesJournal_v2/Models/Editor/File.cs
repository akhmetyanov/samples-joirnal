using System;
using System.Collections.Generic;
using SamplesJournal_v2.Models.Template;

namespace SamplesJournal_v2.Models.Editor
{
    public class EditorFile
    {
        public EditorFile()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EditorFileRowsNode Node { get; set; }
        public Template.Template Template { get; set; }
    }
    public class EditorFileRowsNode
    {
        public EditorFileRowsNode()
        {
            Id= Guid.NewGuid();
            Childs = new List<EditorFileRowsNode>();
            Rows = new List<EditorFileRow>();
        }
        public Guid Id { get; set; }
        public Guid TemplateHeaderId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public List<EditorFileRowsNode> Childs { get; set; }
        public List<EditorFileRow> Rows { get; set; }
    }
    public class EditorFileRow
    {
        public EditorFileRow()
        {
            Id= Guid.NewGuid();
            Values = new List<EditorFileRowValue>();
            Edited = false;
        }
        public Guid Id { get; set; }
        public List<EditorFileRowValue> Values { get; set; }
        public bool Edited { get; set; }
        public DateTime EditedDate { get; set; }
    }

    public class EditorFileRowValue
    {

        public EditorFileRowValue()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid TemplateHeaderId { get; set; }
        public Object Value { get; set; }
    }
}
