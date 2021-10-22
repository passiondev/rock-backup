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
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// The Related Entity to allow linking entities (of the same or different types) to each other.
    /// </summary>
    [RockDomain( "Core" )]
    [Table( "RelatedEntity" )]
    [DataContract]
    public partial class RelatedEntity : Model<RelatedEntity>, IOrdered
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the EntityTypeId for the <see cref="Rock.Model.EntityType"/> of source entity.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the EntityTypeId for the <see cref="Rock.Model.EntityType"/> of the  source entity.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
#if !NET5_0_OR_GREATER
        [Index( "IDX_SourceEntityTypeIdSourceEntityIdTargetEntityTypeIdTargetEntityIdPurposeKey", IsUnique = true, Order = 1 )]
#endif
        public int SourceEntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the EntityId of the <see cref="Rock.Model.EntityType" /> of the source.
        /// </summary>
        /// <value>
        /// The source entity identifier.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        [Range( 1, int.MaxValue, ErrorMessage = "SourceEntityId must be greater than zero" )]
#if !NET5_0_OR_GREATER
        [Index( "IDX_SourceEntityTypeIdSourceEntityIdTargetEntityTypeIdTargetEntityIdPurposeKey", IsUnique = true, Order = 2 )]
#endif
        public int SourceEntityId { get; set; }

        /// <summary>
        /// Gets or sets the EntityTypeId for the <see cref="Rock.Model.EntityType" /> of target entity.
        /// </summary>
        /// <value>
        /// The target entity type identifier.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
#if !NET5_0_OR_GREATER
        [Index( "IDX_SourceEntityTypeIdSourceEntityIdTargetEntityTypeIdTargetEntityIdPurposeKey", IsUnique = true, Order = 3 )]
#endif
        public int TargetEntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the EntityId of the <see cref="Rock.Model.EntityType" /> of the target.
        /// </summary>
        /// <value>
        /// The target entity identifier.
        /// </value>
        [Required]
        [Range( 1, int.MaxValue, ErrorMessage = "TargetEntityId must be greater than zero" )]
        [DataMember( IsRequired = true )]
#if !NET5_0_OR_GREATER
        [Index( "IDX_SourceEntityTypeIdSourceEntityIdTargetEntityTypeIdTargetEntityIdPurposeKey", IsUnique = true, Order = 4 )]
#endif
        public int TargetEntityId { get; set; }

        /// <summary>
        /// Gets or sets the purpose key. This indicates the purpose of the relationship. For example:
        /// <list type="bullet">
        /// <item>
        ///     <term><see cref="RelatedEntityPurposeKey.RegistrationInstanceGroupPlacement"/></term>
        ///     <description>This indicates a Placement Group that is specific to the <see cref="RegistrationInstance"/></description>
        /// </item>
        /// <item>
        ///     <term><see cref="RelatedEntityPurposeKey.RegistrationTemplateGroupPlacementTemplate"/></term>
        ///     <description>This indicates a Placement Group that is 'shared' for all RegistrationInstances of the <see cref="RegistrationTemplate"/></description>
        /// </item>
        /// </list>
        /// See details on <seealso cref="RelatedEntityPurposeKey"/>
        /// </summary>
        /// <value>
        /// The purpose key.
        /// </value>
        [DataMember]
        [MaxLength( 100 )]
#if !NET5_0_OR_GREATER
        [Index( "IDX_SourceEntityTypeIdSourceEntityIdTargetEntityTypeIdTargetEntityIdPurposeKey", IsUnique = true, Order = 5 )]
#endif
        public string PurposeKey { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if this Site was created by and is part of the Rock core system/framework. This property is required.
        /// </summary>
        /// <value>
        /// A <see cref="System.Boolean"/> that is <c>true</c> if this Site is part of the Rock core system/framework, otherwise <c>false</c>.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the qualifier value.
        /// See more details on <seealso cref="RelatedEntityPurposeKey"/>.
        /// </summary>
        /// <value>
        /// The qualifier value.
        /// </value>
        [DataMember]
        [MaxLength( 200 )]
        public string QualifierValue { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the type of the source entity.
        /// See notes on <seealso cref="RelatedEntityPurposeKey"/> for how this works.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        [DataMember]
        public virtual EntityType SourceEntityType { get; set; }

        /// <summary>
        /// Gets or sets the type of the target entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        [DataMember]
        public virtual EntityType TargetEntityType { get; set; }

        #endregion
    }

    /// <summary>
    /// see <see cref="RelatedEntity.PurposeKey" />
    /// </summary>
    public static class RelatedEntityPurposeKey
    {
        /// <summary>
        /// The group placement for a specific Registration Instance.
        /// <br />
        /// <br />
        /// For this, the <see cref="RelatedEntity"/> fields would be...
        /// <list>
        /// <item>
        ///     <term><see cref="RelatedEntity.PurposeKey" /></term>
        ///     <description>PLACEMENT</description>
        /// </item>
        /// <item>
        ///     <term><see cref="RelatedEntity.SourceEntityType"/></term>
        ///     <description><see cref="RegistrationInstance"/></description>
        /// </item>
        /// <item>
        ///     <term><see cref="RelatedEntity.TargetEntityType"/></term>
        ///     <description><see cref="Rock.Model.Group"/></description>
        /// </item>
        /// <item>
        ///     <term><see cref="RelatedEntity.QualifierValue"/></term>
        ///     <description><see cref="RegistrationTemplatePlacement"/> Id</description>
        /// </item>
        /// </list>
        /// </summary>
        public const string RegistrationInstanceGroupPlacement = "PLACEMENT";

        /// <summary>
        /// The group placement for a Registration Template ('Shared' for all of the RegistrationTemplate's Registration Instances),
        /// For this, the RelatedEntity fields would be...
        /// <list>
        /// <item>
        ///     <term><see cref="RelatedEntity.PurposeKey" /></term>
        ///     <description>PLACEMENT-TEMPLATE</description>
        /// </item>
        /// <item>
        ///     <term><see cref="RelatedEntity.SourceEntityType"/></term>
        ///     <description><see cref="RegistrationTemplatePlacement"/></description>
        /// </item>
        /// <item>
        ///     <term><see cref="RelatedEntity.TargetEntityType"/></term>
        ///     <description><see cref="Rock.Model.Group"/></description>
        /// </item>
        /// <item>
        ///     <term><see cref="RelatedEntity.QualifierValue"/></term>
        ///     <description><c>null</c></description>
        /// </item>
        /// </list>
        /// </summary>
        public const string RegistrationTemplateGroupPlacementTemplate = "PLACEMENT-TEMPLATE";
    }

    #region Entity Configuration

    /// <summary>
    /// RelatedEntity Configuration class.
    /// </summary>
    public partial class RelatedEntityConfiguration : EntityTypeConfiguration<RelatedEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySetItemConfiguration"/> class.
        /// </summary>
        public RelatedEntityConfiguration()
        {
            this.HasRequired( a => a.SourceEntityType ).WithMany().HasForeignKey( a => a.SourceEntityTypeId ).WillCascadeOnDelete( false );
            this.HasRequired( a => a.TargetEntityType ).WithMany().HasForeignKey( a => a.TargetEntityTypeId ).WillCascadeOnDelete( false );
        }
    }

    #endregion
}