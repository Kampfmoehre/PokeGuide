using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Nito.AsyncEx;

using PokeGuide.Core.Event;
using PokeGuide.Core.Model;
using PokeGuide.Core.Service.Interface;
using PokeGuide.Core.ViewModel.Interface;

namespace PokeGuide.Core.ViewModel
{
    public class MoveViewModel : SingleObjectViewModel, IMoveViewModel, INavigable
    {
        IDataService _dataService;
        INotifyTaskCompletionCollection<ModelNameBase> _moveList;
        INotifyTaskCompletion<Move> _currentMove;
        int? _cachedMoveId;

        public MoveViewModel(IDataService dataService) : base()
        {
            _dataService = dataService;
        }

        /// <summary>
        /// Sets and gets the the list of abilities
        /// </summary>
        public INotifyTaskCompletionCollection<ModelNameBase> MoveList
        {
            get { return _moveList; }
            set
            {
                if (MoveList != null)
                    MoveList.SelectedItemChanged -= SelectedMoveChanged;
                Set(() => MoveList, ref _moveList, value);
                if (MoveList != null)
                    MoveList.SelectedItemChanged += SelectedMoveChanged;
            }
        }
        /// <summary>
        /// Sets and gets the current selected ability
        /// </summary>
        public INotifyTaskCompletion<Move> CurrentMove
        {
            get { return _currentMove; }
            set { Set(() => CurrentMove, ref _currentMove, value); }
        }

        public void Activate(object param)
        {
            int moveId = 0;
            if (param != null && Int32.TryParse(param.ToString(), out moveId))
            {
                _cachedMoveId = moveId;
                MoveList.SelectItem(moveId);
            }
        }
        public void Deactivate(object param)
        { }

        /// <summary>
        /// Reacts to changes of the current version
        /// </summary>
        /// <param name="newVersion">The new version</param>
        protected override void ChangeVersion(GameVersion newVersion)
        {
            base.ChangeVersion(newVersion);
            MoveList = NotifyTaskCompletionCollection<ModelNameBase>.Create(LoadMovesAsync, _cachedMoveId);
        }
        void SelectedMoveChanged(object sender, SelectedItemChangedEventArgs<ModelNameBase> e)
        {
            CurrentMove = null;
            if (e.NewItem != null)
                CurrentMove = NotifyTaskCompletion.Create(LoadMoveByIdAsync(e.NewItem.Id));
        }

        async Task<ObservableCollection<ModelNameBase>> LoadMovesAsync()
        {
            IEnumerable<ModelNameBase> list = await _dataService.LoadMoveNamesAsync(CurrentLanguage.Id, CurrentVersion.Generation, TokenSource.Token);
            return new ObservableCollection<ModelNameBase>(list);
        }

        async Task<Move> LoadMoveByIdAsync(int id)
        {
            return await _dataService.LoadMoveByIdAsync(id, CurrentVersion, CurrentLanguage.Id, TokenSource.Token);
        }
    }
}
