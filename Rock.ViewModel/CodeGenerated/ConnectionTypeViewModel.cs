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
    /// ConnectionType View Model
    /// </summary>
    [ViewModelOf( typeof( Rock.Model.ConnectionType ) )]
    public partial class ConnectionTypeViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the ConnectionRequestDetailPageId.
        /// </summary>
        /// <value>
        /// The ConnectionRequestDetailPageId.
        /// </value>
        public int? ConnectionRequestDetailPageId { get; set; }

        /// <summary>
        /// Gets or sets the ConnectionRequestDetailPageRouteId.
        /// </summary>
        /// <value>
        /// The ConnectionRequestDetailPageRouteId.
        /// </value>
        public int? ConnectionRequestDetailPageRouteId { get; set; }

        /// <summary>
        /// Gets or sets the DaysUntilRequestIdle.
        /// </summary>
        /// <value>
        /// The DaysUntilRequestIdle.
        /// </value>
        public int DaysUntilRequestIdle { get; set; }

        /// <summary>
        /// Gets or sets the DefaultView.
        /// </summary>
        /// <value>
        /// The DefaultView.
        /// </value>
        public int DefaultView { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// The Description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the EnableFullActivityList.
        /// </summary>
        /// <value>
        /// The EnableFullActivityList.
        /// </value>
        public bool EnableFullActivityList { get; set; }

        /// <summary>
        /// Gets or sets the EnableFutureFollowup.
        /// </summary>
        /// <value>
        /// The EnableFutureFollowup.
        /// </value>
        public bool EnableFutureFollowup { get; set; }

        /// <summary>
        /// Gets or sets the EnableRequestSecurity.
        /// </summary>
        /// <value>
        /// The EnableRequestSecurity.
        /// </value>
        public bool EnableRequestSecurity { get; set; }

        /// <summary>
        /// Gets or sets the IconCssClass.
        /// </summary>
        /// <value>
        /// The IconCssClass.
        /// </value>
        public string IconCssClass { get; set; }

        /// <summary>
        /// Gets or sets the IsActive.
        /// </summary>
        /// <value>
        /// The IsActive.
        /// </value>
        public bool IsActive { get; set; }

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
        /// Gets or sets the OwnerPersonAliasId.
        /// </summary>
        /// <value>
        /// The OwnerPersonAliasId.
        /// </value>
        public int? OwnerPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the RequestBadgeLava.
        /// </summary>
        /// <value>
        /// The RequestBadgeLava.
        /// </value>
        public string RequestBadgeLava { get; set; }

        /// <summary>
        /// Gets or sets the RequestHeaderLava.
        /// </summary>
        /// <value>
        /// The RequestHeaderLava.
        /// </value>
        public string RequestHeaderLava { get; set; }

        /// <summary>
        /// Gets or sets the RequiresPlacementGroupToConnect.
        /// </summary>
        /// <value>
        /// The RequiresPlacementGroupToConnect.
        /// </value>
        public bool RequiresPlacementGroupToConnect { get; set; }

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
        public virtual void SetPropertiesFrom( Rock.Model.ConnectionType model, Person currentPerson = null, bool loadAttributes = true )
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
            ConnectionRequestDetailPageId = model.ConnectionRequestDetailPageId;
            ConnectionRequestDetailPageRouteId = model.ConnectionRequestDetailPageRouteId;
            DaysUntilRequestIdle = model.DaysUntilRequestIdle;
            DefaultView = ( int ) model.DefaultView;
            Description = model.Description;
            EnableFullActivityList = model.EnableFullActivityList;
            EnableFutureFollowup = model.EnableFutureFollowup;
            EnableRequestSecurity = model.EnableRequestSecurity;
            IconCssClass = model.IconCssClass;
            IsActive = model.IsActive;
            Name = model.Name;
            Order = model.Order;
            OwnerPersonAliasId = model.OwnerPersonAliasId;
            RequestBadgeLava = model.RequestBadgeLava;
            RequestHeaderLava = model.RequestHeaderLava;
            RequiresPlacementGroupToConnect = model.RequiresPlacementGroupToConnect;
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
        public static ConnectionTypeViewModel From( Rock.Model.ConnectionType model, Person currentPerson = null, bool loadAttributes = true )
        {
            var viewModel = new ConnectionTypeViewModel();
            viewModel.SetPropertiesFrom( model, currentPerson, loadAttributes );
            return viewModel;
        }
    }
}
