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
    /// EventItem View Model
    /// </summary>
    [ViewModelOf( typeof( Rock.Model.EventItem ) )]
    public partial class EventItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the ApprovedByPersonAliasId.
        /// </summary>
        /// <value>
        /// The ApprovedByPersonAliasId.
        /// </value>
        public int? ApprovedByPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the ApprovedOnDateTime.
        /// </summary>
        /// <value>
        /// The ApprovedOnDateTime.
        /// </value>
        public DateTime? ApprovedOnDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// The Description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the DetailsUrl.
        /// </summary>
        /// <value>
        /// The DetailsUrl.
        /// </value>
        public string DetailsUrl { get; set; }

        /// <summary>
        /// Gets or sets the IsActive.
        /// </summary>
        /// <value>
        /// The IsActive.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the IsApproved.
        /// </summary>
        /// <value>
        /// The IsApproved.
        /// </value>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the PhotoId.
        /// </summary>
        /// <value>
        /// The PhotoId.
        /// </value>
        public int? PhotoId { get; set; }

        /// <summary>
        /// Gets or sets the Summary.
        /// </summary>
        /// <value>
        /// The Summary.
        /// </value>
        public string Summary { get; set; }

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
        public virtual void SetPropertiesFrom( Rock.Model.EventItem model, Person currentPerson = null, bool loadAttributes = true )
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
            ApprovedByPersonAliasId = model.ApprovedByPersonAliasId;
            ApprovedOnDateTime = model.ApprovedOnDateTime;
            Description = model.Description;
            DetailsUrl = model.DetailsUrl;
            IsActive = model.IsActive;
            IsApproved = model.IsApproved;
            Name = model.Name;
            PhotoId = model.PhotoId;
            Summary = model.Summary;
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
        public static EventItemViewModel From( Rock.Model.EventItem model, Person currentPerson = null, bool loadAttributes = true )
        {
            var viewModel = new EventItemViewModel();
            viewModel.SetPropertiesFrom( model, currentPerson, loadAttributes );
            return viewModel;
        }
    }
}
