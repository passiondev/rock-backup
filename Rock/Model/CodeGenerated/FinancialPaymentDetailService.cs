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

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// FinancialPaymentDetail Service class
    /// </summary>
    public partial class FinancialPaymentDetailService : Service<FinancialPaymentDetail>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialPaymentDetailService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public FinancialPaymentDetailService(RockContext context) : base(context)
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
        public bool CanDelete( FinancialPaymentDetail item, out string errorMessage )
        {
            errorMessage = string.Empty;
 
            if ( new Service<FinancialPersonSavedAccount>( Context ).Queryable().Any( a => a.FinancialPaymentDetailId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", FinancialPaymentDetail.FriendlyTypeName, FinancialPersonSavedAccount.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<FinancialScheduledTransaction>( Context ).Queryable().Any( a => a.FinancialPaymentDetailId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", FinancialPaymentDetail.FriendlyTypeName, FinancialScheduledTransaction.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<FinancialTransaction>( Context ).Queryable().Any( a => a.FinancialPaymentDetailId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", FinancialPaymentDetail.FriendlyTypeName, FinancialTransaction.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class FinancialPaymentDetailExtensionMethods
    {
        /// <summary>
        /// Clones this FinancialPaymentDetail object to a new FinancialPaymentDetail object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static FinancialPaymentDetail Clone( this FinancialPaymentDetail source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as FinancialPaymentDetail;
            }
            else
            {
                var target = new FinancialPaymentDetail();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Clones this FinancialPaymentDetail object to a new FinancialPaymentDetail object with default values for the properties in the Entity and Model base classes.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static FinancialPaymentDetail CloneWithoutIdentity( this FinancialPaymentDetail source )
        {
            var target = new FinancialPaymentDetail();
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
        /// Copies the properties from another FinancialPaymentDetail object to this FinancialPaymentDetail object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this FinancialPaymentDetail target, FinancialPaymentDetail source )
        {
            target.Id = source.Id;
            target.AccountNumberMasked = source.AccountNumberMasked;
            target.BillingLocationId = source.BillingLocationId;
            target.CreditCardTypeValueId = source.CreditCardTypeValueId;
            target.CurrencyTypeValueId = source.CurrencyTypeValueId;
            target.ExpirationMonth = source.ExpirationMonth;
            #pragma warning disable 612, 618
            target.ExpirationMonthEncrypted = source.ExpirationMonthEncrypted;
            #pragma warning restore 612, 618
            target.ExpirationYear = source.ExpirationYear;
            #pragma warning disable 612, 618
            target.ExpirationYearEncrypted = source.ExpirationYearEncrypted;
            #pragma warning restore 612, 618
            target.FinancialPersonSavedAccountId = source.FinancialPersonSavedAccountId;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.GatewayPersonIdentifier = source.GatewayPersonIdentifier;
            target.NameOnCard = source.NameOnCard;
            #pragma warning disable 612, 618
            target.NameOnCardEncrypted = source.NameOnCardEncrypted;
            #pragma warning restore 612, 618
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }
    }
}
