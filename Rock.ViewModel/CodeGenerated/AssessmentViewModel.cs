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
    /// Assessment View Model
    /// </summary>
    [ViewModelOf( typeof( Rock.Model.Assessment ) )]
    public partial class AssessmentViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the AssessmentResultData.
        /// </summary>
        /// <value>
        /// The AssessmentResultData.
        /// </value>
        public string AssessmentResultData { get; set; }

        /// <summary>
        /// Gets or sets the AssessmentTypeId.
        /// </summary>
        /// <value>
        /// The AssessmentTypeId.
        /// </value>
        public int AssessmentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the CompletedDateTime.
        /// </summary>
        /// <value>
        /// The CompletedDateTime.
        /// </value>
        public DateTime? CompletedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the LastReminderDate.
        /// </summary>
        /// <value>
        /// The LastReminderDate.
        /// </value>
        public DateTime? LastReminderDate { get; set; }

        /// <summary>
        /// Gets or sets the PersonAliasId.
        /// </summary>
        /// <value>
        /// The PersonAliasId.
        /// </value>
        public int PersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the RequestedDateTime.
        /// </summary>
        /// <value>
        /// The RequestedDateTime.
        /// </value>
        public DateTime? RequestedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the RequestedDueDate.
        /// </summary>
        /// <value>
        /// The RequestedDueDate.
        /// </value>
        public DateTime? RequestedDueDate { get; set; }

        /// <summary>
        /// Gets or sets the RequesterPersonAliasId.
        /// </summary>
        /// <value>
        /// The RequesterPersonAliasId.
        /// </value>
        public int? RequesterPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        /// <value>
        /// The Status.
        /// </value>
        public int Status { get; set; }

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
        public virtual void SetPropertiesFrom( Rock.Model.Assessment model, Person currentPerson = null, bool loadAttributes = true )
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
            AssessmentResultData = model.AssessmentResultData;
            AssessmentTypeId = model.AssessmentTypeId;
            CompletedDateTime = model.CompletedDateTime;
            LastReminderDate = model.LastReminderDate;
            PersonAliasId = model.PersonAliasId;
            RequestedDateTime = model.RequestedDateTime;
            RequestedDueDate = model.RequestedDueDate;
            RequesterPersonAliasId = model.RequesterPersonAliasId;
            Status = ( int ) model.Status;
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
        public static AssessmentViewModel From( Rock.Model.Assessment model, Person currentPerson = null, bool loadAttributes = true )
        {
            var viewModel = new AssessmentViewModel();
            viewModel.SetPropertiesFrom( model, currentPerson, loadAttributes );
            return viewModel;
        }
    }
}
