namespace LanguageQuizBot.Repository.Interfaces
{
    public interface IRepository<T> where T : class, new()
    {
        Task<T> CreateAsync(T item);
        Task<T> GetAsync(string id);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(string id);
    }
}
