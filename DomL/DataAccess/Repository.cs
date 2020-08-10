using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

public class Repository<TEntity> where TEntity : class
{
    protected readonly DbContext Context;
    private DbSet<TEntity> _entities;

    public Repository(DbContext context)
    {
        Context = context;
        _entities = Context.Set<TEntity>();
    }

    public TEntity Get(int id)
    {
        // Here we are working with a DbContext, not PlutoContext. So we don't have DbSets 
        //such as Courses or Authors, and we need to use the generic Set() method to access them
        return _entities.Find(id);
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _entities.ToList();
    }

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return _entities.Where(predicate);
    }

    public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return _entities.SingleOrDefault(predicate);
    }

    public void Add(TEntity entity)
    {
        _entities.Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _entities.AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        _entities.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _entities.RemoveRange(entities);
    }
}
