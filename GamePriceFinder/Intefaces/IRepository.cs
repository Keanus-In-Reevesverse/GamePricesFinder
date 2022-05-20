namespace GamePriceFinder.Intefaces
{
    public interface IRepository<T> where T : class
    {
        public T FindOne(int id);
        public void AddOne(T entity);
        public void AddMany(List<T> entities);
        public void EditOne(T entity);
    }
}
