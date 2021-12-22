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
using System.Web.Http;

using Rock.ClientService.Core.Category;
using Rock.ClientService.Core.Category.Options;
using Rock.Data;
using Rock.Rest.Filters;

namespace Rock.Rest.v2.Controls
{
    /// <summary>
    /// Provides API endpoints for the CategoryPicker control.
    /// </summary>
    /// <seealso cref="Rock.Rest.v2.Controls.ControlsControllerBase" />
    [RoutePrefix( "api/v2/Controls/CategoryPicker" )]
    public class CategoryPickerController : ControlsControllerBase
    {
        /// <summary>
        /// Gets the child items that match the options sent in the request body.
        /// This endpoint returns items formatted for use in a tree view control.
        /// </summary>
        /// <param name="options">The options that describe which items to load.</param>
        /// <returns>A collection of view models that represent the tree items.</returns>
        [HttpPost]
        [System.Web.Http.Route( "childTreeItems" )]
        [Authenticate]
        public IHttpActionResult PostChildTreeItems( [FromBody] ChildTreeItemOptions options )
        {
            using ( var rockContext = new RockContext() )
            {
                var clientService = new CategoryClientService( rockContext, GetPerson( rockContext ) );

                var items = clientService.GetCategorizedTreeItems( new CategoryItemTreeOptions
                {
                    ParentGuid = options.ParentGuid,
                    GetCategorizedItems = options.GetCategorizedItems,
                    EntityTypeGuid = options.EntityTypeGuid,
                    EntityTypeQualifierColumn = options.EntityTypeQualifierColumn,
                    EntityTypeQualifierValue = options.EntityTypeQualifierValue,
                    IncludeUnnamedEntityItems = options.IncludeUnnamedEntityItems,
                    IncludeCategoriesWithoutChildren = options.IncludeCategoriesWithoutChildren,
                    DefaultIconCssClass = options.DefaultIconCssClass,
                    IncludeInactiveItems = options.IncludeInactiveItems,
                    LazyLoad = options.LazyLoad
                } );

                return Ok( items );
            }
        }

        /// <summary>
        /// The options for retrieving child tree items.
        /// </summary>
        public class ChildTreeItemOptions
        {
            /// <inheritdoc cref="CategoryItemTreeOptions.ParentGuid"/>
            public Guid? ParentGuid { get; set; }

            /// <inheritdoc cref="CategoryItemTreeOptions.GetCategorizedItems"/>
            public bool GetCategorizedItems { get; set; }

            /// <inheritdoc cref="CategoryItemTreeOptions.EntityTypeGuid"/>
            public Guid? EntityTypeGuid { get; set; }

            /// <inheritdoc cref="CategoryItemTreeOptions.EntityTypeQualifierColumn"/>
            public string EntityTypeQualifierColumn { get; set; }

            /// <inheritdoc cref="CategoryItemTreeOptions.EntityTypeQualifierValue"/>
            public string EntityTypeQualifierValue { get; set; }

            /// <inheritdoc cref="CategoryItemTreeOptions.IncludeUnnamedEntityItems"/>
            public bool IncludeUnnamedEntityItems { get; set; }

            /// <inheritdoc cref="CategoryItemTreeOptions.IncludeCategoriesWithoutChildren"/>
            public bool IncludeCategoriesWithoutChildren { get; set; } = true;

            /// <inheritdoc cref="CategoryItemTreeOptions.DefaultIconCssClass"/>
            public string DefaultIconCssClass { get; set; } = "fa fa-list-ol";

            /// <inheritdoc cref="CategoryItemTreeOptions.IncludeInactiveItems"/>
            public bool IncludeInactiveItems { get; set; }

            /// <inheritdoc cref="CategoryItemTreeOptions.LazyLoad"/>
            public bool LazyLoad { get; set; } = true;
        }
    }
}
