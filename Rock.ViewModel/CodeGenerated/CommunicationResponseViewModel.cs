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
    /// CommunicationResponse View Model
    /// </summary>
    [ViewModelOf( typeof( Rock.Model.CommunicationResponse ) )]
    public partial class CommunicationResponseViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the FromPersonAliasId.
        /// </summary>
        /// <value>
        /// The FromPersonAliasId.
        /// </value>
        public int? FromPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the IsRead.
        /// </summary>
        /// <value>
        /// The IsRead.
        /// </value>
        public bool IsRead { get; set; }

        /// <summary>
        /// Gets or sets the MessageKey.
        /// </summary>
        /// <value>
        /// The MessageKey.
        /// </value>
        public string MessageKey { get; set; }

        /// <summary>
        /// Gets or sets the RelatedCommunicationId.
        /// </summary>
        /// <value>
        /// The RelatedCommunicationId.
        /// </value>
        public int? RelatedCommunicationId { get; set; }

        /// <summary>
        /// Gets or sets the RelatedMediumEntityTypeId.
        /// </summary>
        /// <value>
        /// The RelatedMediumEntityTypeId.
        /// </value>
        public int RelatedMediumEntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the RelatedSmsFromDefinedValueId.
        /// </summary>
        /// <value>
        /// The RelatedSmsFromDefinedValueId.
        /// </value>
        public int? RelatedSmsFromDefinedValueId { get; set; }

        /// <summary>
        /// Gets or sets the RelatedTransportEntityTypeId.
        /// </summary>
        /// <value>
        /// The RelatedTransportEntityTypeId.
        /// </value>
        public int RelatedTransportEntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Response.
        /// </summary>
        /// <value>
        /// The Response.
        /// </value>
        public string Response { get; set; }

        /// <summary>
        /// Gets or sets the ToPersonAliasId.
        /// </summary>
        /// <value>
        /// The ToPersonAliasId.
        /// </value>
        public int? ToPersonAliasId { get; set; }

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
        public virtual void SetPropertiesFrom( Rock.Model.CommunicationResponse model, Person currentPerson = null, bool loadAttributes = true )
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
            FromPersonAliasId = model.FromPersonAliasId;
            IsRead = model.IsRead;
            MessageKey = model.MessageKey;
            RelatedCommunicationId = model.RelatedCommunicationId;
            RelatedMediumEntityTypeId = model.RelatedMediumEntityTypeId;
            RelatedSmsFromDefinedValueId = model.RelatedSmsFromDefinedValueId;
            RelatedTransportEntityTypeId = model.RelatedTransportEntityTypeId;
            Response = model.Response;
            ToPersonAliasId = model.ToPersonAliasId;
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
        public static CommunicationResponseViewModel From( Rock.Model.CommunicationResponse model, Person currentPerson = null, bool loadAttributes = true )
        {
            var viewModel = new CommunicationResponseViewModel();
            viewModel.SetPropertiesFrom( model, currentPerson, loadAttributes );
            return viewModel;
        }
    }
}
