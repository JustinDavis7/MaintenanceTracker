using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProjectMaintenance.DAL.Abstract
{
    /// <summary>
    /// Interface for common and CRUD operations on entities
    /// </summary>
    /// <typeparam name="TEntity">This is the entity for which we're making a repository</typeparam>
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Find entity by PK. This only works for entities with integer PKs. 
        /// </summary>
        /// <param name="id">The PK of the entity to find</param>
        /// <returns>The entity or null if not found</returns>
        TEntity FindById(int id);

        /// <summary>
        /// Check if the entity with this integer PK exists in the table.
        /// </summary>
        /// <param name="id">The PK of the entity to check</param>
        /// <returns>True if the entity exists, False otherwise</returns>
        bool Exists(int id);

        /// <summary>
        /// Get all entities in this table. Note, when eager loading is used, this
        /// method will NOT populate navigation properties associated with foreign keys.
        /// </summary>
        /// <returns>All the entities</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Get all entities in this table that satisfy the given predicate.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        /// <returns>Filtered entities</returns>
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Version of GetAll that will perform includes to load navigation properties, 
        /// but only first-level ones. It will NOT do ThenInclude.
        /// </summary>
        /// <param name="includes">Lambda functions that represent includes of properties</param>
        /// <returns>All entities with all the includes</returns>
        IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Update an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>The updated entity, suitably synced with the DB</returns>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Remove this entity from the DB. If the entity is not in the DB or has not been
        /// previously added, it "should" do nothing.
        /// </summary>
        /// <param name="entity">The entity to remove</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Remove the entity having this PK from the DB.
        /// </summary>
        /// <param name="id">The integer PK of the entity to remove</param>
        /// <exception cref="System.Exception">Thrown if no entity with this PK id exists</exception>
        void DeleteById(int id);
    }
}
