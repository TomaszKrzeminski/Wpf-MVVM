using FriendOrganizer.Model;
using FriendOrganizer.UI.Events;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        private bool _hasChanges;
        protected readonly IEventAggregator EventAggregator;

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand CloseDetailViewCommand { get; private set; }

        public DetailViewModelBase(IEventAggregator EventAggregator,IMessageDialogService messageDialogService)
        {
            this.EventAggregator = EventAggregator;
            MessageDialogService = messageDialogService;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            CloseDetailViewCommand = new DelegateCommand(OnCloseDetailViewExecute);
        }

        protected virtual void RaiseCollectionSavedEvent()
        {
            EventAggregator.GetEvent<AfterCollectionSavedEvent>().Publish(
                
                new AfterCollectionSavedEventArgs { ViewModelName=this.GetType().Name}
                
                );
        }


        protected virtual void OnCloseDetailViewExecute()
        {
            if(HasChanges)
            {
                var result = MessageDialogService.ShowOkCancelDialog("You have made changes. Close this item?","Question");
                if(result==MessageDialogResult.Cancel) 
                {
                    return;
                }
            }

            EventAggregator.GetEvent<AfterDetailClosedEvent>()
                  .Publish(new AfterDetailClosedEventArgs
                  {
                      Id = this.Id,
                      ViewModelName = this.GetType().Name
                  }
                  );
        }

        protected abstract void OnDeleteExecute();
        protected abstract bool OnSaveCanExecute();
        protected abstract void OnSaveExecute();
        private string _title;
        public string Title
        {
            get { return _title; }
            protected set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public abstract Task LoadAsync(int id);
        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if(_hasChanges != value) 
                {                
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();               
                
                }
            }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public IMessageDialogService MessageDialogService { get; }

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {
            EventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(new AfterDetailDeletedEventArgs() 
            {
                Id=modelId,
                ViewModelName=this.GetType().Name
            }
            );


        }

        protected virtual void RaiseDetailSavedEvent(int modelId,string displayMember)
        {
            EventAggregator.GetEvent<AfterDetailSaveEvent>().Publish(new AfterSaveDetailEventArgs()
            {
                Id = modelId,
                ViewModelName = this.GetType().Name,
                DisplayMember=displayMember
            }
            );


        }


        protected  async Task SaveWithOptimisticConcurrentcyAsync(Func<Task> saveFunc,Action afterSaveAction)
        {

            try
            {
                await saveFunc();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var databaseValues = ex.Entries.Single().GetDatabaseValues();
                if (databaseValues == null)
                {
                    MessageDialogService.ShowInfoDialog("The entity has been deleted by another user");
                    RaiseDetailDeletedEvent(Id); return;
                }


                var result = MessageDialogService.ShowOkCancelDialog("The entity has been changed click ok to save your changes or cancel to reload the entity from db", "Question");
                if (result == MessageDialogResult.OK)
                {//Update with database client wins
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    await saveFunc();
                }
                else
                {
                    await ex.Entries.Single().ReloadAsync();
                    await LoadAsync(Id);
                }
            }





            afterSaveAction();

        }


    }
}
