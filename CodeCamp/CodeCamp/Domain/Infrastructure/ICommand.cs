namespace CodeCamp.Domain.Infrastructure {
    public interface ICommand<out TResponse> where TResponse : CommandResponse {
        TResponse Execute();
    }
}