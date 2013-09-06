namespace CodeCamp.Domain.Infrastructure {
    public interface IQuery<out TResult> {
        TResult Execute();
    }
}