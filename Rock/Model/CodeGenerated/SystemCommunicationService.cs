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
    /// SystemCommunication Service class
    /// </summary>
    public partial class SystemCommunicationService : Service<SystemCommunication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemCommunicationService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public SystemCommunicationService(RockContext context) : base(context)
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
        public bool CanDelete( SystemCommunication item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new Service<FinancialTransactionAlertType>( Context ).Queryable().Any( a => a.SystemCommunicationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", SystemCommunication.FriendlyTypeName, FinancialTransactionAlertType.FriendlyTypeName );
                return false;
            }

            if ( new Service<GroupSync>( Context ).Queryable().Any( a => a.ExitSystemCommunicationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", SystemCommunication.FriendlyTypeName, GroupSync.FriendlyTypeName );
                return false;
            }

            if ( new Service<GroupSync>( Context ).Queryable().Any( a => a.WelcomeSystemCommunicationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", SystemCommunication.FriendlyTypeName, GroupSync.FriendlyTypeName );
                return false;
            }

            if ( new Service<GroupType>( Context ).Queryable().Any( a => a.ScheduleConfirmationSystemCommunicationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", SystemCommunication.FriendlyTypeName, GroupType.FriendlyTypeName );
                return false;
            }

            if ( new Service<GroupType>( Context ).Queryable().Any( a => a.ScheduleReminderSystemCommunicationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", SystemCommunication.FriendlyTypeName, GroupType.FriendlyTypeName );
                return false;
            }

            if ( new Service<SignatureDocumentTemplate>( Context ).Queryable().Any( a => a.CompletionSystemCommunicationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", SystemCommunication.FriendlyTypeName, SignatureDocumentTemplate.FriendlyTypeName );
                return false;
            }

            if ( new Service<SignatureDocumentTemplate>( Context ).Queryable().Any( a => a.InviteSystemCommunicationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", SystemCommunication.FriendlyTypeName, SignatureDocumentTemplate.FriendlyTypeName );
                return false;
            }

            if ( new Service<WorkflowActionForm>( Context ).Queryable().Any( a => a.NotificationSystemCommunicationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", SystemCommunication.FriendlyTypeName, WorkflowActionForm.FriendlyTypeName );
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// SystemCommunication View Model Helper
    /// </summary>
    [DefaultViewModelHelper( typeof( SystemCommunication ) )]
    public partial class SystemCommunicationViewModelHelper : ViewModelHelper<SystemCommunication, Rock.ViewModel.SystemCommunicationViewModel>
    {
        /// <summary>
        /// Converts the model to a view model.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <param name="currentPerson">The current person.</param>
        /// <param name="loadAttributes">if set to <c>true</c> [load attributes].</param>
        /// <returns></returns>
        public override Rock.ViewModel.SystemCommunicationViewModel CreateViewModel( SystemCommunication model, Person currentPerson = null, bool loadAttributes = true )
        {
            if ( model == null )
            {
                return default;
            }

            var viewModel = new Rock.ViewModel.SystemCommunicationViewModel
            {
                Id = model.Id,
                Guid = model.Guid,
                Bcc = model.Bcc,
                Body = model.Body,
                CategoryId = model.CategoryId,
                Cc = model.Cc,
                CssInliningEnabled = model.CssInliningEnabled,
                From = model.From,
                FromName = model.FromName,
                IsActive = model.IsActive,
                IsSystem = model.IsSystem,
                LavaFieldsJson = model.LavaFieldsJson,
                PushData = model.PushData,
                PushImageBinaryFileId = model.PushImageBinaryFileId,
                PushMessage = model.PushMessage,
                PushOpenAction = ( int? ) model.PushOpenAction,
                PushOpenMessage = model.PushOpenMessage,
                PushSound = model.PushSound,
                PushTitle = model.PushTitle,
                SMSFromDefinedValueId = model.SMSFromDefinedValueId,
                SMSMessage = model.SMSMessage,
                Subject = model.Subject,
                Title = model.Title,
                To = model.To,
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
    public static partial class SystemCommunicationExtensionMethods
    {
        /// <summary>
        /// Clones this SystemCommunication object to a new SystemCommunication object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static SystemCommunication Clone( this SystemCommunication source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as SystemCommunication;
            }
            else
            {
                var target = new SystemCommunication();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Clones this SystemCommunication object to a new SystemCommunication object with default values for the properties in the Entity and Model base classes.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static SystemCommunication CloneWithoutIdentity( this SystemCommunication source )
        {
            var target = new SystemCommunication();
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
        /// Copies the properties from another SystemCommunication object to this SystemCommunication object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this SystemCommunication target, SystemCommunication source )
        {
            target.Id = source.Id;
            target.Bcc = source.Bcc;
            target.Body = source.Body;
            target.CategoryId = source.CategoryId;
            target.Cc = source.Cc;
            target.CssInliningEnabled = source.CssInliningEnabled;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.From = source.From;
            target.FromName = source.FromName;
            target.IsActive = source.IsActive;
            target.IsSystem = source.IsSystem;
            target.LavaFieldsJson = source.LavaFieldsJson;
            target.PushData = source.PushData;
            target.PushImageBinaryFileId = source.PushImageBinaryFileId;
            target.PushMessage = source.PushMessage;
            target.PushOpenAction = source.PushOpenAction;
            target.PushOpenMessage = source.PushOpenMessage;
            target.PushSound = source.PushSound;
            target.PushTitle = source.PushTitle;
            target.SMSFromDefinedValueId = source.SMSFromDefinedValueId;
            target.SMSMessage = source.SMSMessage;
            target.Subject = source.Subject;
            target.Title = source.Title;
            target.To = source.To;
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
        public static Rock.ViewModel.SystemCommunicationViewModel ToViewModel( this SystemCommunication model, Person currentPerson = null, bool loadAttributes = false )
        {
            var helper = new SystemCommunicationViewModelHelper();
            var viewModel = helper.CreateViewModel( model, currentPerson, loadAttributes );
            return viewModel;
        }

    }

}
