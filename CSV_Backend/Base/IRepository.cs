namespace CSV_Backend.Base;

public interface IRepository<TEntity>
{
    public Task<List<TEntity>> GetAll();
    public Task<TEntity> Add(TEntity entity);
    public Task<List<TEntity>> AddRange(List<TEntity> entities);
    public Task<TEntity> Update(TEntity entity);
    public Task Delete(int id);
}