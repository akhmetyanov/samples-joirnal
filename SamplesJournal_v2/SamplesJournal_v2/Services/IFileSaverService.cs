using SamplesJournal_v2.Models.Editor;
using SamplesJournal_v2.Models.Template;

namespace SamplesJournal_v2.Services
{
    public interface IFileSaverService
    {
        string SaveFile(EditorFile file);
        string SaveFileToScv(EditorFile file);
        string SaveTemplate(Template template);
    }
}
