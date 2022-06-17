using System;
using System.Collections.Generic;
using System.Text;

namespace SamplesJournal_v2.Models.Template
{
    public class Template
    {
        public Template()
        {
            Id = Guid.NewGuid();
            Name = "Template";
            Headers = new List<TemplateHeader>();
            Dicts = new List<TemplateDict>();
        }
        public Guid Id{ get; set; }
        public string Name { get; set; }
        public List<TemplateHeader> Headers { get; set; }
        public List<TemplateDict> Dicts { get; set; }
    }

    public class TemplateHeader
    {
        public TemplateHeader(Guid templateId)
        {
            TemplateId = templateId;
            Id = Guid.NewGuid();
            InputTool = InputTypeEnum.TextInput;
            NullAble = false;
            GrupBy = false;
            ShowInEditor = false;
            ToEdit = false;
        }

        public Guid TemplateId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameInFile { get; set; }
        public string DefaultValue { get; set; }
        public Guid DictId { get; set; }
        public InputTypeEnum InputTool { get; set; }
        public bool ToEdit { get; set; }
        public bool NullAble { get; set; }
        public bool GrupBy { get; set; }
        public bool ShowInEditor { get; set; }
    }

    public class TemplateDict
    {
        public TemplateDict(Guid templateId)
        {
            TemplateId = templateId;
            Id = Guid.NewGuid();
            Values = new List<TemplateDictValue>();
        }

        public Guid TemplateId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<TemplateDictValue> Values{ get; set; }
    }

    public class TemplateDictValue
    {
        public TemplateDictValue()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }
    }
}