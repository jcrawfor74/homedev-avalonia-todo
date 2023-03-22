using HomeDev.App.Todo.Models;
using HomeDev.App.Todo.Services;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace HomeDev.App.Todo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _content;

        public MainWindowViewModel(Database db)
        {
            Content = List = new TodoListViewModel(db.GetItems());
        }

        public void AddItem()
        {
            var viewModel = new AddItemViewModel();

            Observable.Merge(
                viewModel.Ok,
                viewModel.Cancel.Select(_ => (TodoItem)null)
            )
            .Take(1)
            .Subscribe(o =>
            {
                if (o != null) { List.Items.Add(o); }
                Content = List;
            });

            Content = viewModel;
        }

        public ViewModelBase Content
        {
            get => _content;
            private set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        public TodoListViewModel List { get; }
    }
}