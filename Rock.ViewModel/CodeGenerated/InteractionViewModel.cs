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
    /// Interaction View Model
    /// </summary>
    [ViewModelOf( typeof( Rock.Model.Interaction ) )]
    public partial class InteractionViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the Campaign.
        /// </summary>
        /// <value>
        /// The Campaign.
        /// </value>
        public string Campaign { get; set; }

        /// <summary>
        /// Gets or sets the ChannelCustom1.
        /// </summary>
        /// <value>
        /// The ChannelCustom1.
        /// </value>
        public string ChannelCustom1 { get; set; }

        /// <summary>
        /// Gets or sets the ChannelCustom2.
        /// </summary>
        /// <value>
        /// The ChannelCustom2.
        /// </value>
        public string ChannelCustom2 { get; set; }

        /// <summary>
        /// Gets or sets the ChannelCustomIndexed1.
        /// </summary>
        /// <value>
        /// The ChannelCustomIndexed1.
        /// </value>
        public string ChannelCustomIndexed1 { get; set; }

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        /// <value>
        /// The Content.
        /// </value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the EntityId.
        /// </summary>
        /// <value>
        /// The EntityId.
        /// </value>
        public int? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the InteractionComponentId.
        /// </summary>
        /// <value>
        /// The InteractionComponentId.
        /// </value>
        public int InteractionComponentId { get; set; }

        /// <summary>
        /// Gets or sets the InteractionData.
        /// </summary>
        /// <value>
        /// The InteractionData.
        /// </value>
        public string InteractionData { get; set; }

        /// <summary>
        /// Gets or sets the InteractionDateTime.
        /// </summary>
        /// <value>
        /// The InteractionDateTime.
        /// </value>
        public DateTime InteractionDateTime { get; set; }

        /// <summary>
        /// Gets or sets the InteractionEndDateTime.
        /// </summary>
        /// <value>
        /// The InteractionEndDateTime.
        /// </value>
        public DateTime? InteractionEndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the InteractionLength.
        /// </summary>
        /// <value>
        /// The InteractionLength.
        /// </value>
        public double? InteractionLength { get; set; }

        /// <summary>
        /// Gets or sets the InteractionSessionId.
        /// </summary>
        /// <value>
        /// The InteractionSessionId.
        /// </value>
        public int? InteractionSessionId { get; set; }

        /// <summary>
        /// Gets or sets the InteractionSummary.
        /// </summary>
        /// <value>
        /// The InteractionSummary.
        /// </value>
        public string InteractionSummary { get; set; }

        /// <summary>
        /// Gets or sets the InteractionTimeToServe.
        /// </summary>
        /// <value>
        /// The InteractionTimeToServe.
        /// </value>
        public double? InteractionTimeToServe { get; set; }

        /// <summary>
        /// Gets or sets the Medium.
        /// </summary>
        /// <value>
        /// The Medium.
        /// </value>
        public string Medium { get; set; }

        /// <summary>
        /// Gets or sets the Operation.
        /// </summary>
        /// <value>
        /// The Operation.
        /// </value>
        public string Operation { get; set; }

        /// <summary>
        /// Gets or sets the PersonalDeviceId.
        /// </summary>
        /// <value>
        /// The PersonalDeviceId.
        /// </value>
        public int? PersonalDeviceId { get; set; }

        /// <summary>
        /// Gets or sets the PersonAliasId.
        /// </summary>
        /// <value>
        /// The PersonAliasId.
        /// </value>
        public int? PersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the RelatedEntityId.
        /// </summary>
        /// <value>
        /// The RelatedEntityId.
        /// </value>
        public int? RelatedEntityId { get; set; }

        /// <summary>
        /// Gets or sets the RelatedEntityTypeId.
        /// </summary>
        /// <value>
        /// The RelatedEntityTypeId.
        /// </value>
        public int? RelatedEntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Source.
        /// </summary>
        /// <value>
        /// The Source.
        /// </value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the Term.
        /// </summary>
        /// <value>
        /// The Term.
        /// </value>
        public string Term { get; set; }

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
        public virtual void SetPropertiesFrom( Rock.Model.Interaction model, Person currentPerson = null, bool loadAttributes = true )
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
            Campaign = model.Campaign;
            ChannelCustom1 = model.ChannelCustom1;
            ChannelCustom2 = model.ChannelCustom2;
            ChannelCustomIndexed1 = model.ChannelCustomIndexed1;
            Content = model.Content;
            EntityId = model.EntityId;
            InteractionComponentId = model.InteractionComponentId;
            InteractionData = model.InteractionData;
            InteractionDateTime = model.InteractionDateTime;
            InteractionEndDateTime = model.InteractionEndDateTime;
            InteractionLength = model.InteractionLength;
            InteractionSessionId = model.InteractionSessionId;
            InteractionSummary = model.InteractionSummary;
            InteractionTimeToServe = model.InteractionTimeToServe;
            Medium = model.Medium;
            Operation = model.Operation;
            PersonalDeviceId = model.PersonalDeviceId;
            PersonAliasId = model.PersonAliasId;
            RelatedEntityId = model.RelatedEntityId;
            RelatedEntityTypeId = model.RelatedEntityTypeId;
            Source = model.Source;
            Term = model.Term;
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
        public static InteractionViewModel From( Rock.Model.Interaction model, Person currentPerson = null, bool loadAttributes = true )
        {
            var viewModel = new InteractionViewModel();
            viewModel.SetPropertiesFrom( model, currentPerson, loadAttributes );
            return viewModel;
        }
    }
}
