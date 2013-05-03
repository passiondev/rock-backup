//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// ServiceLog Service class
    /// </summary>
    public partial class ServiceLogService : Service<ServiceLog>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogService"/> class
        /// </summary>
        public ServiceLogService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogService"/> class
        /// </summary>
        public ServiceLogService(IRepository<ServiceLog> repository) : base(repository)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( ServiceLog item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static class ServiceLogExtensionMethods
    {
        /// <summary>
        /// Clones this ServiceLog object to a new ServiceLog object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static ServiceLog Clone( this ServiceLog source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as ServiceLog;
            }
            else
            {
                var target = new ServiceLog();
                target.LogDateTime = source.LogDateTime;
                target.Input = source.Input;
                target.Type = source.Type;
                target.Name = source.Name;
                target.Result = source.Result;
                target.Success = source.Success;
                target.Id = source.Id;
                target.Guid = source.Guid;

            
                return target;
            }
        }
    }
}
