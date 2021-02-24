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
    /// RelatedEntity View Model
    /// </summary>
    [ViewModelOf( typeof( Rock.Model.RelatedEntity ) )]
    public partial class RelatedEntityViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the IsSystem.
        /// </summary>
        /// <value>
        /// The IsSystem.
        /// </value>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the Order.
        /// </summary>
        /// <value>
        /// The Order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the PurposeKey.
        /// </summary>
        /// <value>
        /// The PurposeKey.
        /// </value>
        public string PurposeKey { get; set; }

        /// <summary>
        /// Gets or sets the QualifierValue.
        /// </summary>
        /// <value>
        /// The QualifierValue.
        /// </value>
        public string QualifierValue { get; set; }

        /// <summary>
        /// Gets or sets the SourceEntityId.
        /// </summary>
        /// <value>
        /// The SourceEntityId.
        /// </value>
        public int SourceEntityId { get; set; }

        /// <summary>
        /// Gets or sets the SourceEntityTypeId.
        /// </summary>
        /// <value>
        /// The SourceEntityTypeId.
        /// </value>
        public int SourceEntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the TargetEntityId.
        /// </summary>
        /// <value>
        /// The TargetEntityId.
        /// </value>
        public int TargetEntityId { get; set; }

        /// <summary>
        /// Gets or sets the TargetEntityTypeId.
        /// </summary>
        /// <value>
        /// The TargetEntityTypeId.
        /// </value>
        public int TargetEntityTypeId { get; set; }

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
        public virtual void SetPropertiesFrom( Rock.Model.RelatedEntity model, Person currentPerson = null, bool loadAttributes = true )
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
            IsSystem = model.IsSystem;
            Order = model.Order;
            PurposeKey = model.PurposeKey;
            QualifierValue = model.QualifierValue;
            SourceEntityId = model.SourceEntityId;
            SourceEntityTypeId = model.SourceEntityTypeId;
            TargetEntityId = model.TargetEntityId;
            TargetEntityTypeId = model.TargetEntityTypeId;
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
        public static RelatedEntityViewModel From( Rock.Model.RelatedEntity model, Person currentPerson = null, bool loadAttributes = true )
        {
            var viewModel = new RelatedEntityViewModel();
            viewModel.SetPropertiesFrom( model, currentPerson, loadAttributes );
            return viewModel;
        }
    }
}
