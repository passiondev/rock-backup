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

using Rock.ViewModels.Utility;

namespace Rock.ViewModels.Entities
{
    /// <summary>
    /// ExceptionLog View Model
    /// </summary>
    public partial class ExceptionLogBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets the Cookies.
        /// </summary>
        /// <value>
        /// The Cookies.
        /// </value>
        public string Cookies { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// The Description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ExceptionType.
        /// </summary>
        /// <value>
        /// The ExceptionType.
        /// </value>
        public string ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the Form.
        /// </summary>
        /// <value>
        /// The Form.
        /// </value>
        public string Form { get; set; }

        /// <summary>
        /// Gets or sets the HasInnerException.
        /// </summary>
        /// <value>
        /// The HasInnerException.
        /// </value>
        public bool? HasInnerException { get; set; }

        /// <summary>
        /// Gets or sets the PageId.
        /// </summary>
        /// <value>
        /// The PageId.
        /// </value>
        public int? PageId { get; set; }

        /// <summary>
        /// Gets or sets the PageUrl.
        /// </summary>
        /// <value>
        /// The PageUrl.
        /// </value>
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the ParentId.
        /// </summary>
        /// <value>
        /// The ParentId.
        /// </value>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the QueryString.
        /// </summary>
        /// <value>
        /// The QueryString.
        /// </value>
        public string QueryString { get; set; }

        /// <summary>
        /// Gets or sets the ServerVariables.
        /// </summary>
        /// <value>
        /// The ServerVariables.
        /// </value>
        public string ServerVariables { get; set; }

        /// <summary>
        /// Gets or sets the SiteId.
        /// </summary>
        /// <value>
        /// The SiteId.
        /// </value>
        public int? SiteId { get; set; }

        /// <summary>
        /// Gets or sets the Source.
        /// </summary>
        /// <value>
        /// The Source.
        /// </value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the StackTrace.
        /// </summary>
        /// <value>
        /// The StackTrace.
        /// </value>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the StatusCode.
        /// </summary>
        /// <value>
        /// The StatusCode.
        /// </value>
        public string StatusCode { get; set; }

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

    }
}