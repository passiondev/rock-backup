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
using System.Collections.Generic;


namespace Rock.Client
{
    /// <summary>
    /// Recipient Information for the Statement Generator
    /// </summary>
    public partial class FinancialStatementGeneratorRecipientEntity
    {
        /// <summary />
        public decimal? ContributionTotal { get; set; }

        /// <summary />
        public string Country { get; set; }

        /// <summary />
        public int GroupId { get; set; }

        /// <summary />
        public bool HasValidMailingAddress { get; set; }

        /// <summary />
        public bool IsComplete { get; set; }

        /// <summary />
        public bool IsInternationalAddress { get; set; }

        /// <summary />
        public string LastName { get; set; }

        /// <summary />
        public int? LocationId { get; set; }

        /// <summary />
        public string NickName { get; set; }

        /// <summary />
        public bool? OptedOut { get; set; }

        /// <summary />
        public int? PaperlessStatementsIndividualCount { get; set; }

        /// <summary />
        public bool? PaperlessStatementUploaded { get; set; }

        /// <summary />
        public int? PersonId { get; set; }

        /// <summary />
        public string PostalCode { get; set; }

        /// <summary />
        public int? RenderedPageCount { get; set; }

        /// <summary>
        /// Copies the base properties from a source FinancialStatementGeneratorRecipient object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( FinancialStatementGeneratorRecipient source )
        {
            this.ContributionTotal = source.ContributionTotal;
            this.Country = source.Country;
            this.GroupId = source.GroupId;
            this.HasValidMailingAddress = source.HasValidMailingAddress;
            this.IsComplete = source.IsComplete;
            this.IsInternationalAddress = source.IsInternationalAddress;
            this.LastName = source.LastName;
            this.LocationId = source.LocationId;
            this.NickName = source.NickName;
            this.OptedOut = source.OptedOut;
            this.PaperlessStatementsIndividualCount = source.PaperlessStatementsIndividualCount;
            this.PaperlessStatementUploaded = source.PaperlessStatementUploaded;
            this.PersonId = source.PersonId;
            this.PostalCode = source.PostalCode;
            this.RenderedPageCount = source.RenderedPageCount;

        }
    }

    /// <summary>
    /// Recipient Information for the Statement Generator
    /// </summary>
    public partial class FinancialStatementGeneratorRecipient : FinancialStatementGeneratorRecipientEntity
    {
    }
}
