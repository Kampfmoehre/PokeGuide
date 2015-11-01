using PokeGuide.Model;

namespace PokeGuide.Service.Interface
{
    public interface ISettingsService
    {
        INotifyTaskCompletionCollection<Language> Languages { get; set; }
    }
}
