using FriendOrganizer.DataAccess.Migrations;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class ProgrammingLanguageDetailViewModel : DetailViewModelBase
    {
        private IProgrammingLanguageRepository _programmingLanguageRepository;
        private ProgrammingLanguageWrapper _selectedProgrammingLanguage;

        public ObservableCollection<ProgrammingLanguageWrapper> ProgrammingLanguages { get; private set; }
        public DelegateCommand AddedCommand { get; private set; }
        public DelegateCommand RemoveCommand { get; private set; }

        private ProgrammingLanguageWrapper selectedProgrammingLanguage;

        public ProgrammingLanguageWrapper SelectedProgrammingLanguage
        {
            get { return selectedProgrammingLanguage; }
            set { selectedProgrammingLanguage = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveCommand).RaiseCanExecuteChanged();
            }
        }

       

        public ProgrammingLanguageDetailViewModel(IEventAggregator EventAggregator, IMessageDialogService messageDialogService,IProgrammingLanguageRepository programmingLanguageRepository) : base(EventAggregator, messageDialogService)
        {

            Title = "Programming Languages";
            _programmingLanguageRepository = programmingLanguageRepository;
            ProgrammingLanguages = new ObservableCollection<ProgrammingLanguageWrapper>();
            AddedCommand = new DelegateCommand(OnAddExecute);
            RemoveCommand = new DelegateCommand(OnRemoveExecute, OnRemoveCanExecute);
        }

        private bool OnRemoveCanExecute()
        {
            return SelectedProgrammingLanguage != null;
        }

        private async void OnRemoveExecute()
        {
            var isReferenced = await _programmingLanguageRepository.IsReferencedByFriendAsync(SelectedProgrammingLanguage.Id);
            if(isReferenced) 
            {

                MessageDialogService.ShowInfoDialog($"The language cant be removed " + SelectedProgrammingLanguage.Name);
                return;
            
            }


            SelectedProgrammingLanguage.PropertyChanged -= Wrapper_PropertyChanged;
            _programmingLanguageRepository.Remove(SelectedProgrammingLanguage.Model);
            ProgrammingLanguages.Remove(SelectedProgrammingLanguage);
            SelectedProgrammingLanguage = null;
            HasChanges = _programmingLanguageRepository.HasCHanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();  
        }

        private void OnAddExecute()
        {
            var wrapper = new ProgrammingLanguageWrapper(new Model.ProgrammingLanguage());
            wrapper.PropertyChanged += Wrapper_PropertyChanged;
            _programmingLanguageRepository.Add(wrapper.Model);
            ProgrammingLanguages.Add(wrapper);
            wrapper.Name = "";
        }

        public override async Task LoadAsync(int id)
        {
           
            Id = id;
            foreach (var w in ProgrammingLanguages)
            {
                w.PropertyChanged -= Wrapper_PropertyChanged;
            }

            ProgrammingLanguages.Clear();
            var languages = await _programmingLanguageRepository.GetAllAsync();
            foreach (var model in languages)
            {
                var wrapper = new ProgrammingLanguageWrapper(model);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
                ProgrammingLanguages.Add(wrapper);
            }

        }

        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(!HasChanges)
            {
                HasChanges = _programmingLanguageRepository.HasCHanges();
                if(e.PropertyName==nameof(ProgrammingLanguageWrapper.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        protected override void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCanExecute()
        {
            return HasChanges && ProgrammingLanguages.All(p => !p.HasErrors);
        }

        protected override async void OnSaveExecute()
        {
            try
            {
                await _programmingLanguageRepository.SaveAsync();
                HasChanges = _programmingLanguageRepository.HasCHanges();
                RaiseCollectionSavedEvent();
            }
            catch (Exception ex)
            {

                while(ex.InnerException!=null)
                {
                    ex = ex.InnerException;
                }

                MessageDialogService.ShowInfoDialog("Error while saving the entities " + ex.Message);
                await LoadAsync(Id);

            }
           
        }
    }
}
