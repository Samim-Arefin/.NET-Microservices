namespace Ordering.Application.Common.Service
{
    public interface IDapperService
    {
        Task<List<T>> ExecuteQuery<T>(string procedure);
        Task<List<T>> ExecuteQueryWithParam<T, U>(string procedure, U parameter);
        Task<T> ExecuteSingleQuery<T, U>(string procedure, U parameter);
        Task<T> ExecuteNonQuery<T, U>(string procedure, U parameter);
        Task ExecuteNonQuery<U>(string procedure, U parameter);
    }
}
