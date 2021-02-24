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
    /// AssessmentType View Model
    /// </summary>
    [ViewModelOf( typeof( Rock.Model.AssessmentType ) )]
    public partial class AssessmentTypeViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the AssessmentPath.
        /// </summary>
        /// <value>
        /// The AssessmentPath.
        /// </value>
        public string AssessmentPath { get; set; }

        /// <summary>
        /// Gets or sets the AssessmentResultsPath.
        /// </summary>
        /// <value>
        /// The AssessmentResultsPath.
        /// </value>
        public string AssessmentResultsPath { get; set; }

        /// <summary>
        /// Gets or sets the BadgeColor.
        /// </summary>
        /// <value>
        /// The BadgeColor.
        /// </value>
        public string BadgeColor { get; set; }

        /// <summary>
        /// Gets or sets the BadgeSummaryLava.
        /// </summary>
        /// <value>
        /// The BadgeSummaryLava.
        /// </value>
        public string BadgeSummaryLava { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// The Description.
        /// </value>
        public string Description { get; set; }

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
        /// Gets or sets the IsSystem.
        /// </summary>
        /// <value>
        /// The IsSystem.
        /// </value>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the MinimumDaysToRetake.
        /// </summary>
        /// <value>
        /// The MinimumDaysToRetake.
        /// </value>
        public int MinimumDaysToRetake { get; set; }

        /// <summary>
        /// Gets or sets the RequiresRequest.
        /// </summary>
        /// <value>
        /// The RequiresRequest.
        /// </value>
        public bool RequiresRequest { get; set; }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        /// <value>
        /// The Title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the ValidDuration.
        /// </summary>
        /// <value>
        /// The ValidDuration.
        /// </value>
        public int ValidDuration { get; set; }

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
        public virtual void SetPropertiesFrom( Rock.Model.AssessmentType model, Person currentPerson = null, bool loadAttributes = true )
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
            AssessmentPath = model.AssessmentPath;
            AssessmentResultsPath = model.AssessmentResultsPath;
            BadgeColor = model.BadgeColor;
            BadgeSummaryLava = model.BadgeSummaryLava;
            Description = model.Description;
            IconCssClass = model.IconCssClass;
            IsActive = model.IsActive;
            IsSystem = model.IsSystem;
            MinimumDaysToRetake = model.MinimumDaysToRetake;
            RequiresRequest = model.RequiresRequest;
            Title = model.Title;
            ValidDuration = model.ValidDuration;
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
        public static AssessmentTypeViewModel From( Rock.Model.AssessmentType model, Person currentPerson = null, bool loadAttributes = true )
        {
            var viewModel = new AssessmentTypeViewModel();
            viewModel.SetPropertiesFrom( model, currentPerson, loadAttributes );
            return viewModel;
        }
    }
}
