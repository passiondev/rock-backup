using System.Data.Entity;

using Rock.Web.Cache;

namespace Rock.Model
{
    public partial class WorkflowActionFormSection
    {

        #region ICacheable

        /// <summary>
        /// Gets the cache object associated with this Entity
        /// </summary>
        /// <returns></returns>
        public IEntityCache GetCacheObject()
        {
            return WorkflowActionFormSectionCache.Get( this.Id );
        }

        /// <summary>
        /// Updates any Cache Objects that are associated with this entity
        /// </summary>
        /// <param name="entityState">State of the entity.</param>
        /// <param name="dbContext">The database context.</param>
        public void UpdateCache( EntityState entityState, Rock.Data.DbContext dbContext )
        {
            WorkflowActionFormSectionCache.UpdateCachedEntity( this.Id, entityState );
        }

        #endregion
    }
}
