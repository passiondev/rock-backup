//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using System.Linq;

using Rock.Attribute;
using Rock.Data;
using Rock.ViewModel;
using Rock.Web.Cache;

namespace Rock.Model
{
    /// <summary>
    /// Workflow Service class
    /// </summary>
    public partial class WorkflowService : Service<Workflow>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public WorkflowService(RockContext context) : base(context)
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
        public bool CanDelete( Workflow item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new Service<ConnectionRequestWorkflow>( Context ).Queryable().Any( a => a.WorkflowId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Workflow.FriendlyTypeName, ConnectionRequestWorkflow.FriendlyTypeName );
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Workflow View Model Helper
    /// </summary>
    [DefaultViewModelHelper( typeof( Workflow ) )]
    public partial class WorkflowViewModelHelper : ViewModelHelper<Workflow, Rock.ViewModel.WorkflowViewModel>
    {
        /// <summary>
        /// Converts the model to a view model.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <param name="currentPerson">The current person.</param>
        /// <param name="loadAttributes">if set to <c>true</c> [load attributes].</param>
        /// <returns></returns>
        public override Rock.ViewModel.WorkflowViewModel CreateViewModel( Workflow model, Person currentPerson = null, bool loadAttributes = true )
        {
            if ( model == null )
            {
                return default;
            }

            var viewModel = new Rock.ViewModel.WorkflowViewModel
            {
                Id = model.Id,
                Guid = model.Guid,
                ActivatedDateTime = model.ActivatedDateTime,
                CompletedDateTime = model.CompletedDateTime,
                Description = model.Description,
                EntityId = model.EntityId,
                EntityTypeId = model.EntityTypeId,
                InitiatorPersonAliasId = model.InitiatorPersonAliasId,
                IsProcessing = model.IsProcessing,
                LastProcessedDateTime = model.LastProcessedDateTime,
                Name = model.Name,
                Status = model.Status,
                WorkflowIdNumber = model.WorkflowIdNumber,
                WorkflowTypeId = model.WorkflowTypeId,
                CreatedDateTime = model.CreatedDateTime,
                ModifiedDateTime = model.ModifiedDateTime,
                CreatedByPersonAliasId = model.CreatedByPersonAliasId,
                ModifiedByPersonAliasId = model.ModifiedByPersonAliasId,
            };

            AddAttributesToViewModel( model, viewModel, currentPerson, loadAttributes );
            ApplyAdditionalPropertiesAndSecurityToViewModel( model, viewModel, currentPerson, loadAttributes );
            return viewModel;
        }
    }


    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class WorkflowExtensionMethods
    {
        /// <summary>
        /// Clones this Workflow object to a new Workflow object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static Workflow Clone( this Workflow source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as Workflow;
            }
            else
            {
                var target = new Workflow();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Clones this Workflow object to a new Workflow object with default values for the properties in the Entity and Model base classes.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Workflow CloneWithoutIdentity( this Workflow source )
        {
            var target = new Workflow();
            target.CopyPropertiesFrom( source );

            target.Id = 0;
            target.Guid = Guid.NewGuid();
            target.ForeignKey = null;
            target.ForeignId = null;
            target.ForeignGuid = null;
            target.CreatedByPersonAliasId = null;
            target.CreatedDateTime = RockDateTime.Now;
            target.ModifiedByPersonAliasId = null;
            target.ModifiedDateTime = RockDateTime.Now;

            return target;
        }

        /// <summary>
        /// Copies the properties from another Workflow object to this Workflow object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this Workflow target, Workflow source )
        {
            target.Id = source.Id;
            target.ActivatedDateTime = source.ActivatedDateTime;
            target.CompletedDateTime = source.CompletedDateTime;
            target.Description = source.Description;
            target.EntityId = source.EntityId;
            target.EntityTypeId = source.EntityTypeId;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.InitiatorPersonAliasId = source.InitiatorPersonAliasId;
            target.IsProcessing = source.IsProcessing;
            target.LastProcessedDateTime = source.LastProcessedDateTime;
            target.Name = source.Name;
            target.Status = source.Status;
            target.WorkflowIdNumber = source.WorkflowIdNumber;
            target.WorkflowTypeId = source.WorkflowTypeId;
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }

        /// <summary>
        /// Creates a view model from this entity
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <param name="currentPerson" >The currentPerson.</param>
        /// <param name="loadAttributes" >Load attributes?</param>
        public static Rock.ViewModel.WorkflowViewModel ToViewModel( this Workflow model, Person currentPerson = null, bool loadAttributes = false )
        {
            var helper = new WorkflowViewModelHelper();
            var viewModel = helper.CreateViewModel( model, currentPerson, loadAttributes );
            return viewModel;
        }

    }

}
