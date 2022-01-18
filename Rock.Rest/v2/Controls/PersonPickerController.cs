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

using System.Linq;
using System.Web.Http;

using Rock.Data;
using Rock.Rest.Filters;

namespace Rock.Rest.v2.Controls
{
    /// <summary>
    /// API endpoints for the Person Picker.
    /// </summary>
    [RoutePrefix( "api/v2/Controls/PersonPicker" )]
    public class PersonPickerController : ControlsControllerBase
    {
        /// <summary>
        /// Returns results to the Person Picker
        /// </summary>
        /// <param name="name">The search parameter for the person's name.</param>
        /// <param name="includeDetails">Set to <c>true</c> details will be included instead of lazy loaded.</param>
        /// <param name="includeBusinesses">Set to <c>true</c> to also search businesses.</param>
        /// <param name="includeDeceased">Set to <c>true</c> to include deceased people.</param>
        /// <param name="address">The search parameter for the person's address.</param>
        /// <param name="phone">The search parameter for the person's phone.</param>
        /// <param name="email">The search parameter for the person's name email.</param>
        /// <returns></returns>
        [Authenticate]
        [Secured]
        [HttpPost]
        [System.Web.Http.Route( "Search" )]
        public IQueryable<Rock.Rest.Controllers.PersonSearchResult> Search( [FromBody] SearchOptions searchOptions )
        {
            using ( var rockContext = new RockContext() )
            {
                return Rock.Rest.Controllers.PeopleController.SearchForPeople( rockContext, name, address, phone, email, includeDetails, includeBusinesses, includeDeceased, false );
            }
        }

        #region Options

        public class SearchOptions
        {
            public string Name { get; set; }

            public string Address { get; set; }

            public string Phone { get; set; }

            public string Email { get; set; }

            public bool IncludeDetails { get; set; }

            public bool IncludeBusinesses { get; set; }

            public bool IncludeDeceased { get; set; }
        }

        #endregion
    }
}
