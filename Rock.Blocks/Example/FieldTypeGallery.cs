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
using System.ComponentModel;
using System.Linq;

using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.ViewModel.NonEntities;
using Rock.Web.Cache;

namespace Rock.Blocks.Example
{
    /// <summary>
    /// Allows the user to try out various field types.
    /// </summary>
    /// <seealso cref="Rock.Blocks.RockObsidianBlockType" />

    [DisplayName( "Field Type Gallery" )]
    [Category( "Obsidian > Example" )]
    [Description( "Allows the user to try out various field types." )]
    [IconCssClass( "fa fa-flask" )]

    public class FieldTypeGallery : RockObsidianBlockType
    {
        public override object GetObsidianBlockInitialization()
        {
            return new
            {
                Attribute = GetEditableAttributeViewModel( new Guid( "3F6EAFE3-E027-4E97-A649-09F75D6F3CF5" ) )
            };
        }

        [BlockAction]
        public BlockActionResult SaveAttribute( PublicEditableAttributeViewModel attribute )
        {
            using ( var rockContext = new RockContext() )
            {
                var attributeService = new AttributeService( rockContext );

                var newAttribute = new Rock.Model.Attribute();
                var attr = attributeService.Get( attribute.Guid ?? Guid.Empty );

                newAttribute.CopyPropertiesFrom( attr );

                var fieldTypeCache = FieldTypeCache.Get( attribute.FieldTypeGuid ?? Guid.Empty );

                var configurationValues = fieldTypeCache.Field.GetPrivateConfigurationOptions( attribute.ConfigurationOptions );

                newAttribute.Name = attribute.Name;
                newAttribute.AbbreviatedName = attribute.AbbreviatedName;
                newAttribute.Key = attribute.Key;
                newAttribute.Description = attribute.Description;
                newAttribute.IsActive = attribute.IsActive;
                newAttribute.IsPublic = attribute.IsPublic;
                newAttribute.IsRequired = attribute.IsRequired;
                newAttribute.ShowOnBulk = attribute.ShowOnBulk;
                newAttribute.IsGridColumn = attribute.ShowInGrid;
                newAttribute.FieldTypeId = fieldTypeCache.Id;
                newAttribute.DefaultValue = fieldTypeCache.Field.GetValueFromClient( attribute.DefaultValue, configurationValues );

                var categoryGuids = attribute.Categories?.Select( c => c.Value.AsGuid() ).ToList();
                newAttribute.Categories.Clear();
                if ( categoryGuids != null && categoryGuids.Any() )
                {
                    new CategoryService( rockContext ).Queryable()
                        .Where( c => categoryGuids.Contains( c.Guid ) )
                        .ToList()
                        .ForEach( c => newAttribute.Categories.Add( c ) );
                }

                // Since changes to Categories isn't tracked by ChangeTracker, set the ModifiedDateTime just in case Categories changed
                newAttribute.ModifiedDateTime = RockDateTime.Now;

                newAttribute.AttributeQualifiers.Clear();
                foreach ( var qualifier in configurationValues )
                {
                    AttributeQualifier attributeQualifier = new AttributeQualifier
                    {
                        IsSystem = false,
                        Key = qualifier.Key,
                        Value = qualifier.Value ?? string.Empty
                    };

                    newAttribute.AttributeQualifiers.Add( attributeQualifier );
                }

                foreach ( var qualifier in attr.AttributeQualifiers )
                {
                    var aq = newAttribute.AttributeQualifiers.FirstOrDefault( q => q.Key == qualifier.Key );

                    if ( aq == null )
                    {
                        AttributeQualifier attributeQualifier = new AttributeQualifier
                        {
                            IsSystem = false,
                            Key = qualifier.Key,
                            Value = qualifier.Value ?? string.Empty
                        };

                        newAttribute.AttributeQualifiers.Add( attributeQualifier );
                    }
                }

                var newAttr = Rock.Attribute.Helper.SaveAttributeEdits( newAttribute, attr.EntityTypeId, string.Empty, string.Empty, rockContext );

                if ( newAttr == null )
                {
                    return ActionBadRequest();
                }

                return ActionOk();
            }
        }

        private PublicEditableAttributeViewModel GetEditableAttributeViewModel( Guid guid )
        {
            using ( var rockContext = new RockContext() )
            {
                var attributeService = new AttributeService( rockContext );
                var attribute = attributeService.Get( guid );

                var fieldTypeCache = FieldTypeCache.Get( attribute.FieldTypeId );
                var configurationValues = attribute.AttributeQualifiers.ToDictionary( q => q.Key, q => q.Value );

                return new PublicEditableAttributeViewModel
                {
                    Guid = attribute.Guid,
                    Name = attribute.Name,
                    Key = attribute.Key,
                    AbbreviatedName = attribute.AbbreviatedName,
                    Description = attribute.Description,
                    IsActive = attribute.IsActive,
                    IsPublic = attribute.IsPublic,
                    IsRequired = attribute.IsRequired,
                    ShowInGrid = attribute.IsGridColumn,
                    ShowOnBulk = attribute.ShowOnBulk,
                    FieldTypeGuid = fieldTypeCache.Guid,
                    Categories = attribute.Categories
                        .Select( c => new ListItemViewModel
                        {
                            Value = c.Guid.ToString(),
                            Text = c.Name
                        })
                        .ToList(),
                    ConfigurationOptions = fieldTypeCache.Field.GetPublicConfigurationOptions( configurationValues ),
                    DefaultValue = fieldTypeCache.Field.GetClientEditValue( attribute.DefaultValue, configurationValues )
                };
            }
        }
    }
}
