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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Rock.Attribute;
using Rock.ClientService.Core.DefinedValue;
using Rock.Data;
using Rock.Model;
using Rock.ViewModel.Blocks.Workflow.FormBuilder;
using Rock.ViewModel.NonEntities;
using Rock.Web.Cache;

namespace Rock.Blocks.Workflow.FormBuilder
{
    /// <summary>
    /// Edits the details of a workflow Form Builder action.
    /// </summary>
    /// <seealso cref="Rock.Blocks.RockObsidianBlockType" />

    [DisplayName( "Form Builder Detail" )]
    [Category( "Obsidian > Workflow > Form Builder" )]
    [Description( "Edits the details of a workflow Form Builder action." )]
    [IconCssClass( "fa fa-hammer" )]

    #region Block Attributes

    [LinkedPage( "Submissions Page",
        Description = "The page that contains the Submissions block to view submissions existing submissions for this form.",
        IsRequired = true,
        Key = AttributeKey.SubmissionsPage,
        Order = 0 )]

    [LinkedPage( "Analytics Page",
        Description = "The page that contains the Analytics block to view statistics on existing submissions for this form.",
        IsRequired = true,
        Key = AttributeKey.AnalyticsPage,
        Order = 1 )]

    #endregion

    public class FormBuilderDetail : RockObsidianBlockType
    {
        private static class AttributeKey
        {
            public const string SubmissionsPage = "SubmissionsPage";

            public const string AnalyticsPage = "AnalyticsPage";
        }

        /// <inheritdoc/>
        public override object GetObsidianBlockInitialization()
        {
            using ( var rockContext = new RockContext() )
            {
                return new FormBuilderDetailViewModel
                {
                    SubmissionsPageUrl = LinkedPageUrl( AttributeKey.SubmissionsPage, RequestContext.GetPageParameters() ),
                    AnalyticsPageUrl = LinkedPageUrl( AttributeKey.AnalyticsPage, RequestContext.GetPageParameters() ),
                    Sources = GetOptionSources( rockContext ),
                    Form = null
                };
            }
        }

        private List<FormFieldTypeViewModel> GetAvailableFieldTypes()
        {
            return FieldTypeCache.All()
                .Where( f => f.Platform.HasFlag( Utility.RockPlatform.Obsidian )
                    && ( f.Usage.HasFlag( Field.FieldTypeUsage.Common ) || f.Usage.HasFlag( Field.FieldTypeUsage.Advanced ) ) )
                .Select( f => new FormFieldTypeViewModel
                {
                    Guid = f.Guid,
                    Text = f.Name,
                    Icon = f.IconCssClass,
                    IsCommon = f.Usage.HasFlag( Field.FieldTypeUsage.Common )
                } )
                .ToList();
        }

        private FormValueSourcesViewModel GetOptionSources( RockContext rockContext )
        {
            var definedValueClientService = new DefinedValueClientService( rockContext, RequestContext.CurrentPerson );

            return new FormValueSourcesViewModel
            {
                FieldTypes = GetAvailableFieldTypes(),
                CampusStatusOptions = definedValueClientService.GetDefinedValuesAsListItems( SystemGuid.DefinedType.CAMPUS_STATUS.AsGuid() ),
                CampusTypeOptions = definedValueClientService.GetDefinedValuesAsListItems( SystemGuid.DefinedType.CAMPUS_TYPE.AsGuid() ),
                AddressTypeOptions = definedValueClientService.GetDefinedValuesAsListItems( SystemGuid.DefinedType.GROUP_LOCATION_TYPE.AsGuid() ),
                ConnectionStatusOptions = definedValueClientService.GetDefinedValuesAsListItems( SystemGuid.DefinedType.PERSON_CONNECTION_STATUS.AsGuid() ),
                RecordStatusOptions = definedValueClientService.GetDefinedValuesAsListItems( SystemGuid.DefinedType.PERSON_RECORD_STATUS.AsGuid() ),
                EmailTemplateOptions = GetEmailTemplateOptions( rockContext ),

                CampusTopicOptions = new List<ListItemViewModel>(),
                SectionTypeOptions = new List<ListItemViewModel>()
                {
                    new ListItemViewModel
                    {
                        Value = Guid.NewGuid().ToString(),
                        Text = "Plain",
                        Category = ""
                    },
                    new ListItemViewModel
                    {
                        Value = Guid.NewGuid().ToString(),
                        Text = "Well",
                        Category = "well"
                    }
                },
            };
        }

        private List<ListItemViewModel> GetEmailTemplateOptions( RockContext rockContext )
        {
            return new SystemCommunicationService( rockContext )
                .Queryable()
                .Where( c => c.IsActive == true )
                .ToList()
                .Where( c => c.IsAuthorized( Rock.Security.Authorization.VIEW, RequestContext.CurrentPerson ) )
                .OrderBy( c => c.Title )
                .Select( c => new ListItemViewModel
                {
                    Value = c.Guid.ToString(),
                    Text = c.Title
                } )
                .ToList();
        }

        #region Move to Extension Methods

        /// <summary>
        /// Builds and returns the URL for a linked <see cref="Rock.Model.Page"/>
        /// from a <see cref="Rock.Attribute.LinkedPageAttribute"/> and any necessary
        /// query parameters.
        /// </summary>
        /// <param name="attributeKey">The attribute key that contains the linked page value.</param>
        /// <param name="queryParams">Any query string parameters that should be included in the built URL.</param>
        /// <returns>A string representing the URL to the linked <see cref="Rock.Model.Page"/>.</returns>
        private string LinkedPageUrl( string attributeKey, IDictionary<string, string> queryParams = null )
        {
            var pageReference = new Rock.Web.PageReference( GetAttributeValue( attributeKey ), new Dictionary<string, string>( queryParams ) );

            if ( pageReference.PageId > 0 )
            {
                return pageReference.BuildUrl();
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
