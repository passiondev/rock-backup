﻿// <copyright>
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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
#if NET5_0_OR_GREATER
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif
using System.Linq;

using Rock.Lava;
using Rock.Web.Cache;

namespace Rock.Model
{
    public partial class RegistrationRegistrant
    {
        #region Navigation Properties
        /// <summary>
        /// Gets the name of the nick.
        /// </summary>
        /// <value>
        /// The name of the nick.
        /// </value>
        [NotMapped]
        [LavaVisible]
        public virtual string NickName 
        {
            get
            {
                if ( PersonAlias != null && PersonAlias.Person != null )
                {
                    return PersonAlias.Person.NickName;
                }

                return string.Empty;
            }
        }
        
        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [NotMapped]
        [LavaVisible]
        public virtual string FirstName 
        {
            get
            {
                if ( PersonAlias != null && PersonAlias.Person != null )
                {
                    return PersonAlias.Person.FirstName;
                }

                return string.Empty;
            }
        }
        
        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [NotMapped]
        [LavaVisible]
        public virtual string LastName
        {
            get
            {
                if ( PersonAlias != null && PersonAlias.Person != null )
                {
                    return PersonAlias.Person.LastName;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [NotMapped]
        [LavaVisible]
        public virtual string Email
        {
            get
            {
                if ( PersonAlias != null && PersonAlias.Person != null )
                {
                    return PersonAlias.Person.Email;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the cost with fees.
        /// </summary>
        /// <value>
        /// The cost with fees.
        /// </value>
        [NotMapped]
        [LavaVisible]
        public virtual decimal TotalCost
        {
            get
            {
                if ( OnWaitList )
                {
                    return 0.0M;
                }

                var cost = Cost;
                if ( Fees != null )
                {
                    cost += Fees.Sum( f => f.TotalCost );
                }

                return cost;
            }
        }

        /// <summary>
        /// Gets the <see cref="Rock.Model.Person"/>.
        /// </summary>
        /// <value>
        /// The person.
        /// </value>
        [LavaVisible]
        public virtual Person Person
        {
            get
            {
                if ( PersonAlias != null )
                {
                    return PersonAlias.Person;
                }

                return null;
            }
        }

        #endregion Navigation Properties

        #region Methods
        /// <summary>
        /// Discounts the cost.
        /// </summary>
        /// <param name="discountPercent">The discount percent.</param>
        /// <param name="discountAmount">The discount amount.</param>
        /// <returns></returns>
        public virtual decimal DiscountedCost( decimal discountPercent, decimal discountAmount )
        {
            if ( OnWaitList )
            {
                return 0.0M;
            }

            var discountedCost = Cost - ( DiscountApplies ? ( Cost * discountPercent ) : 0.0M );
            if ( Fees != null )
            {
                foreach ( var fee in Fees )
                {
                    discountedCost += DiscountApplies ? fee.DiscountedCost( discountPercent ) : fee.TotalCost;
                }
            }

            discountedCost -= DiscountApplies ? discountAmount : 0.0M;

            return discountedCost > 0.0m ? discountedCost : 0.0m;
        }

        /// <summary>
        /// Get a list of all inherited Attributes that should be applied to this entity.
        /// </summary>
        /// <returns>A list of all inherited AttributeCache objects.</returns>
        public override List<AttributeCache> GetInheritedAttributes( Rock.Data.RockContext rockContext )
        {
            var entityTypeCache = EntityTypeCache.Get( TypeId );

            // Get the registration
            var registration = this.Registration;
            if ( registration == null && this.RegistrationId > 0 )
            {
                registration = new RegistrationService( rockContext )
                    .Queryable().AsNoTracking()
                    .FirstOrDefault( r => r.Id == this.RegistrationId );
            }

            if ( entityTypeCache == null || registration == null )
            {
                return null;
            }

            // Get the instance
            var registrationInstance = registration.RegistrationInstance;
            if ( registrationInstance == null && registration.RegistrationInstanceId > 0 )
            {
                registrationInstance = new RegistrationInstanceService( rockContext )
                    .Queryable().AsNoTracking()
                    .FirstOrDefault( r => r.Id == registration.RegistrationInstanceId );
            }

            if ( registrationInstance == null )
            {
                return null;
            }

            // Get all attributes there were defined for instance's template.
            var attributes = new List<AttributeCache>();
            foreach ( var entityTypeAttribute in AttributeCache.GetByEntityType( entityTypeCache.Id )
                .Where( e =>
                    e.EntityTypeQualifierColumn == "RegistrationTemplateId" &&
                    e.EntityTypeQualifierValue.AsInteger() == registrationInstance.RegistrationTemplateId ) )
            {
                attributes.Add( entityTypeAttribute );
            }

            return attributes;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if ( PersonAlias != null && PersonAlias.Person != null )
            {
                return PersonAlias.Person.ToString();
            }
            else
            {
                return "Registrant";
            }
        }

        #endregion Methods
    }
}
