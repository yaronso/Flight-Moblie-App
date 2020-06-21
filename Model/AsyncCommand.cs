using FlightMoblie.Model;
using System.Threading.Tasks;

namespace FlightMobile.Model
{
    public enum Result { Ok, NotOk }

    public class AsyncCommand
    {
        public Command Command { get; private set; }
        public Task<Result> Task { get => Completion.Task; }
        public TaskCompletionSource<Result> Completion { get; private set; }
        public AsyncCommand(Command input)
        {
            Command = input;
            Completion = new TaskCompletionSource<Result>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        }
    }

}
