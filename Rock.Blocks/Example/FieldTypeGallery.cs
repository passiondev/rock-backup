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
using Rock.Field.Types;
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
        [BlockAction]
        public BlockActionResult GetClientDesignValues( AttributeDesignerViewModel designer )
        {
            var response = DesignHelper.GetDesignerClientViewModel( designer, RequestContext.CurrentPerson, out var errorMessage );

            if ( response == null )
            {
                return ActionBadRequest( errorMessage );
            }

            return ActionOk( response );
        }
    }

    public static class DesignHelper
    {
        public static bool IsAuthorized( this AttributeDesignerViewModel designer, Person currentPerson )
        {
            return true;
        }

        public static AttributeDesignerViewModel GetDesignerClientViewModel( AttributeDesignerViewModel designer, Person currentPerson, out string errorMessage )
        {
            if ( designer.FieldTypeGuid != Rock.SystemGuid.FieldType.DEFINED_VALUE.AsGuid() )
            {
                errorMessage = "Field type is not supported.";
                return null;
            }

            var fieldType = new DefinedValueFieldType();

            var configurationValues = fieldType.GetConfigurationValuesFromClientDesignValues( designer.DesignValues );
            var uglyConfigurationValues = configurationValues.ToDictionary( i => i.Key, i => new Field.ConfigurationValue( i.Value ) );

            if ( !fieldType.IsDesignAuthorized( configurationValues, currentPerson ) )
            {
                errorMessage = "Not authorized to make this change.";
                return null;
            }

            var value = fieldType.GetValueFromClient( designer.DefaultValue, uglyConfigurationValues );
            var designConfigurationValues = fieldType.GetClientDesignConfigurationValues( configurationValues );
            var clientDesignValues = fieldType.GetClientDesignValues( configurationValues );
            var clientValue = fieldType.GetClientEditValue( value, uglyConfigurationValues );

            var clientEditableValue = new ClientEditableAttributeValueViewModel
            {
                FieldTypeGuid = designer.FieldTypeGuid,
                AttributeGuid = Guid.Empty,
                Name = "Default Value",
                Categories = new List<ClientAttributeValueCategoryViewModel>(),
                Order = 0,
                TextValue = fieldType.GetTextValue( value, uglyConfigurationValues ),
                Value = fieldType.GetClientEditValue( value, uglyConfigurationValues ),
                Key = "DefaultValue",
                IsRequired = false,
                Description = string.Empty,
                ConfigurationValues = fieldType.GetClientConfigurationValues( uglyConfigurationValues )
            };

            errorMessage = null;

            return new AttributeDesignerViewModel
            {
                DesignConfigurationValues = designConfigurationValues,
                DesignValues = clientDesignValues,
                EditableValue = clientEditableValue
            };
        }
    }

    public class AttributeDesignerViewModel
    {
        public Guid FieldTypeGuid { get; set; }

        public Dictionary<string, string> DesignValues { get; set; }

        public string DefaultValue { get; set; }

        public Dictionary<string, string> DesignConfigurationValues { get; set; }

        public ClientEditableAttributeValueViewModel EditableValue { get; set; }
    }

}
