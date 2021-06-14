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
    /// Base client model for FinancialPaymentDetail that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class FinancialPaymentDetailEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public string AccountNumberMasked { get; set; }

        /// <summary />
        public int? BillingLocationId { get; set; }

        /// <summary />
        public int? CreditCardTypeValueId { get; set; }

        /// <summary />
        public int? CurrencyTypeValueId { get; set; }

        /// <summary />
        public int? ExpirationMonth { get; set; }

        /// <summary />
        // Made Obsolete in Rock "1.12.4"
        [Obsolete( "Use ExpirationMonth", false )]
        public string ExpirationMonthEncrypted { get; set; }

        /// <summary />
        public int? ExpirationYear { get; set; }

        /// <summary />
        // Made Obsolete in Rock "1.12.4"
        [Obsolete( "Use ExpirationYear", false )]
        public string ExpirationYearEncrypted { get; set; }

        /// <summary />
        public int? FinancialPersonSavedAccountId { get; set; }

        /// <summary />
        public Guid? ForeignGuid { get; set; }

        /// <summary />
        public string ForeignKey { get; set; }

        /// <summary />
        public string GatewayPersonIdentifier { get; set; }

        /// <summary>
        /// If the ModifiedByPersonAliasId is being set manually and should not be overwritten with current user when saved, set this value to true
        /// </summary>
        public bool ModifiedAuditValuesAlreadyUpdated { get; set; }

        /// <summary />
        public string NameOnCard { get; set; }

        /// <summary />
        // Made Obsolete in Rock "1.12.4"
        [Obsolete( "Use NameOnCard", false )]
        public string NameOnCardEncrypted { get; set; }

        /// <summary>
        /// Leave this as NULL to let Rock set this
        /// </summary>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// This does not need to be set or changed. Rock will always set this to the current date/time when saved to the database.
        /// </summary>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Leave this as NULL to let Rock set this
        /// </summary>
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary>
        /// If you need to set this manually, set ModifiedAuditValuesAlreadyUpdated=True to prevent Rock from setting it
        /// </summary>
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public int? ForeignId { get; set; }

        /// <summary>
        /// Copies the base properties from a source FinancialPaymentDetail object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( FinancialPaymentDetail source )
        {
            this.Id = source.Id;
            this.AccountNumberMasked = source.AccountNumberMasked;
            this.BillingLocationId = source.BillingLocationId;
            this.CreditCardTypeValueId = source.CreditCardTypeValueId;
            this.CurrencyTypeValueId = source.CurrencyTypeValueId;
            this.ExpirationMonth = source.ExpirationMonth;
            #pragma warning disable 612, 618
            this.ExpirationMonthEncrypted = source.ExpirationMonthEncrypted;
            #pragma warning restore 612, 618
            this.ExpirationYear = source.ExpirationYear;
            #pragma warning disable 612, 618
            this.ExpirationYearEncrypted = source.ExpirationYearEncrypted;
            #pragma warning restore 612, 618
            this.FinancialPersonSavedAccountId = source.FinancialPersonSavedAccountId;
            this.ForeignGuid = source.ForeignGuid;
            this.ForeignKey = source.ForeignKey;
            this.GatewayPersonIdentifier = source.GatewayPersonIdentifier;
            this.ModifiedAuditValuesAlreadyUpdated = source.ModifiedAuditValuesAlreadyUpdated;
            this.NameOnCard = source.NameOnCard;
            #pragma warning disable 612, 618
            this.NameOnCardEncrypted = source.NameOnCardEncrypted;
            #pragma warning restore 612, 618
            this.CreatedDateTime = source.CreatedDateTime;
            this.ModifiedDateTime = source.ModifiedDateTime;
            this.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            this.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }

    /// <summary>
    /// Client model for FinancialPaymentDetail that includes all the fields that are available for GETs. Use this for GETs (use FinancialPaymentDetailEntity for POST/PUTs)
    /// </summary>
    public partial class FinancialPaymentDetail : FinancialPaymentDetailEntity
    {
        /// <summary />
        public Location BillingLocation { get; set; }

        /// <summary />
        public DefinedValue CreditCardTypeValue { get; set; }

        /// <summary />
        public DefinedValue CurrencyTypeValue { get; set; }

        /// <summary />
        public FinancialPersonSavedAccount FinancialPersonSavedAccount { get; set; }

        /// <summary>
        /// NOTE: Attributes are only populated when ?loadAttributes is specified. Options for loadAttributes are true, false, 'simple', 'expanded' 
        /// </summary>
        public Dictionary<string, Rock.Client.Attribute> Attributes { get; set; }

        /// <summary>
        /// NOTE: AttributeValues are only populated when ?loadAttributes is specified. Options for loadAttributes are true, false, 'simple', 'expanded' 
        /// </summary>
        public Dictionary<string, Rock.Client.AttributeValue> AttributeValues { get; set; }
    }
}
