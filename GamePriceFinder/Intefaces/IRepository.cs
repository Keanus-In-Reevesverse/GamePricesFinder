namespace GamePriceFinder.Intefaces
{
    /// <summary>
    /// Repository interface, implemented by GameRepository, GamePricesRepository, GenreRepository, HistoryRepository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        public T FindOne(string name);
        public void AddOne(T entity);
        public void AddMany(List<T> entities);
        public void EditOne(T entity);
    }
}
