using Nito.AsyncEx;

using PokeGuide.Model;

namespace PokeGuide.ViewModel.Interface
{
    public interface IAbilityViewModel
    {
        INotifyTaskCompletionCollection<Ability> Abilities { get; set; }
        INotifyTaskCompletion<Ability> SelectedAbility { get; set; }
    }
}
