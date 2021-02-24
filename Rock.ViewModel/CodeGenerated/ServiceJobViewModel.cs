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
    /// ServiceJob View Model
    /// </summary>
    [ViewModelOf( typeof( Rock.Model.ServiceJob ) )]
    public partial class ServiceJobViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the Assembly.
        /// </summary>
        /// <value>
        /// The Assembly.
        /// </value>
        public string Assembly { get; set; }

        /// <summary>
        /// Gets or sets the Class.
        /// </summary>
        /// <value>
        /// The Class.
        /// </value>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the CronExpression.
        /// </summary>
        /// <value>
        /// The CronExpression.
        /// </value>
        public string CronExpression { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// The Description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the EnableHistory.
        /// </summary>
        /// <value>
        /// The EnableHistory.
        /// </value>
        public bool EnableHistory { get; set; }

        /// <summary>
        /// Gets or sets the HistoryCount.
        /// </summary>
        /// <value>
        /// The HistoryCount.
        /// </value>
        public int HistoryCount { get; set; }

        /// <summary>
        /// Gets or sets the IsActive.
        /// </summary>
        /// <value>
        /// The IsActive.
        /// </value>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the IsSystem.
        /// </summary>
        /// <value>
        /// The IsSystem.
        /// </value>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the LastRunDateTime.
        /// </summary>
        /// <value>
        /// The LastRunDateTime.
        /// </value>
        public DateTime? LastRunDateTime { get; set; }

        /// <summary>
        /// Gets or sets the LastRunDurationSeconds.
        /// </summary>
        /// <value>
        /// The LastRunDurationSeconds.
        /// </value>
        public int? LastRunDurationSeconds { get; set; }

        /// <summary>
        /// Gets or sets the LastRunSchedulerName.
        /// </summary>
        /// <value>
        /// The LastRunSchedulerName.
        /// </value>
        public string LastRunSchedulerName { get; set; }

        /// <summary>
        /// Gets or sets the LastStatus.
        /// </summary>
        /// <value>
        /// The LastStatus.
        /// </value>
        public string LastStatus { get; set; }

        /// <summary>
        /// Gets or sets the LastStatusMessage.
        /// </summary>
        /// <value>
        /// The LastStatusMessage.
        /// </value>
        public string LastStatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the LastSuccessfulRunDateTime.
        /// </summary>
        /// <value>
        /// The LastSuccessfulRunDateTime.
        /// </value>
        public DateTime? LastSuccessfulRunDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the NotificationEmails.
        /// </summary>
        /// <value>
        /// The NotificationEmails.
        /// </value>
        public string NotificationEmails { get; set; }

        /// <summary>
        /// Gets or sets the NotificationStatus.
        /// </summary>
        /// <value>
        /// The NotificationStatus.
        /// </value>
        public int NotificationStatus { get; set; }

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
        public virtual void SetPropertiesFrom( Rock.Model.ServiceJob model, Person currentPerson = null, bool loadAttributes = true )
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
            Assembly = model.Assembly;
            Class = model.Class;
            CronExpression = model.CronExpression;
            Description = model.Description;
            EnableHistory = model.EnableHistory;
            HistoryCount = model.HistoryCount;
            IsActive = model.IsActive;
            IsSystem = model.IsSystem;
            LastRunDateTime = model.LastRunDateTime;
            LastRunDurationSeconds = model.LastRunDurationSeconds;
            LastRunSchedulerName = model.LastRunSchedulerName;
            LastStatus = model.LastStatus;
            LastStatusMessage = model.LastStatusMessage;
            LastSuccessfulRunDateTime = model.LastSuccessfulRunDateTime;
            Name = model.Name;
            NotificationEmails = model.NotificationEmails;
            NotificationStatus = ( int ) model.NotificationStatus;
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
        public static ServiceJobViewModel From( Rock.Model.ServiceJob model, Person currentPerson = null, bool loadAttributes = true )
        {
            var viewModel = new ServiceJobViewModel();
            viewModel.SetPropertiesFrom( model, currentPerson, loadAttributes );
            return viewModel;
        }
    }
}
