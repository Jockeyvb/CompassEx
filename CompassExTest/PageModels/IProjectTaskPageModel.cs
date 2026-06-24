using CommunityToolkit.Mvvm.Input;
using CompassExTest.Models;

namespace CompassExTest.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}