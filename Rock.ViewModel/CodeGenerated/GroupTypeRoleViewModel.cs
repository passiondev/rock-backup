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
//

using System;
using System.Linq;
using Rock.Attribute;
using Rock.Model;
using Rock.Web.Cache;

namespace Rock.ViewModel
{
    /// <summary>
    /// GroupTypeRole View Model
    /// </summary>
    [ViewModelOf( typeof( Rock.Model.GroupTypeRole ) )]
    public partial class GroupTypeRoleViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the CanEdit.
        /// </summary>
        /// <value>
        /// The CanEdit.
        /// </value>
        public bool CanEdit { get; set; }

        /// <summary>
        /// Gets or sets the CanManageMembers.
        /// </summary>
        /// <value>
        /// The CanManageMembers.
        /// </value>
        public bool CanManageMembers { get; set; }

        /// <summary>
        /// Gets or sets the CanView.
        /// </summary>
        /// <value>
        /// The CanView.
        /// </value>
        public bool CanView { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// The Description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the GroupTypeId.
        /// </summary>
        /// <value>
        /// The GroupTypeId.
        /// </value>
        public int? GroupTypeId { get; set; }

        /// <summary>
        /// Gets or sets the IsLeader.
        /// </summary>
        /// <value>
        /// The IsLeader.
        /// </value>
        public bool IsLeader { get; set; }

        /// <summary>
        /// Gets or sets the IsSystem.
        /// </summary>
        /// <value>
        /// The IsSystem.
        /// </value>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the MaxCount.
        /// </summary>
        /// <value>
        /// The MaxCount.
        /// </value>
        public int? MaxCount { get; set; }

        /// <summary>
        /// Gets or sets the MinCount.
        /// </summary>
        /// <value>
        /// The MinCount.
        /// </value>
        public int? MinCount { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Order.
        /// </summary>
        /// <value>
        /// The Order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the ReceiveRequirementsNotifications.
        /// </summary>
        /// <value>
        /// The ReceiveRequirementsNotifications.
        /// </value>
        public bool ReceiveRequirementsNotifications { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDateTime.
        /// </summary>
        /// <value>
        /// The CreatedDateTime.
        /// </value>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedDateTime.
        /// </summary>
        /// <value>
        /// The ModifiedDateTime.
        /// </value>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the CreatedByPersonAliasId.
        /// </summary>
        /// <value>
        /// The CreatedByPersonAliasId.
        /// </value>
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedByPersonAliasId.
        /// </summary>
        /// <value>
        /// The ModifiedByPersonAliasId.
        /// </value>
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary>
        /// Sets the properties from.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="currentPerson">The current person.</param>
        /// <param name="loadAttributes">if set to <c>true</c> [load attributes].</param>
        public virtual void SetPropertiesFrom( Rock.Model.GroupTypeRole model, Person currentPerson = null, bool loadAttributes = true )
        {
            if ( model == null )
            {
                return;
            }

            if ( loadAttributes && model is IHasAttributes hasAttributes )
            {
                if ( hasAttributes.Attributes == null )
                {
                    hasAttributes.LoadAttributes();
                }

                Attributes = hasAttributes.AttributeValues.Where( av =>
                {
                    var attribute = AttributeCache.Get( av.Value.AttributeId );
                    return attribute?.IsAuthorized( Rock.Security.Authorization.EDIT, currentPerson ) ?? false;
                } ).ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.ToViewModel<AttributeValueViewModel>() as object );
            }

            Id = model.Id;
            Guid = model.Guid;
            CanEdit = model.CanEdit;
            CanManageMembers = model.CanManageMembers;
            CanView = model.CanView;
            Description = model.Description;
            GroupTypeId = model.GroupTypeId;
            IsLeader = model.IsLeader;
            IsSystem = model.IsSystem;
            MaxCount = model.MaxCount;
            MinCount = model.MinCount;
            Name = model.Name;
            Order = model.Order;
            ReceiveRequirementsNotifications = model.ReceiveRequirementsNotifications;
            CreatedDateTime = model.CreatedDateTime;
            ModifiedDateTime = model.ModifiedDateTime;
            CreatedByPersonAliasId = model.CreatedByPersonAliasId;
            ModifiedByPersonAliasId = model.ModifiedByPersonAliasId;

            SetAdditionalPropertiesFrom( model, currentPerson, loadAttributes );
        }

        /// <summary>
        /// Creates a view model from the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="currentPerson" >The current person.</param>
        /// <param name="loadAttributes" >if set to <c>true</c> [load attributes].</param>
        /// <returns></returns>
        public static GroupTypeRoleViewModel From( Rock.Model.GroupTypeRole model, Person currentPerson = null, bool loadAttributes = true )
        {
            var viewModel = new GroupTypeRoleViewModel();
            viewModel.SetPropertiesFrom( model, currentPerson, loadAttributes );
            return viewModel;
        }
    }
}
