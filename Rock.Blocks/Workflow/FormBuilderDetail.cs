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

using System.ComponentModel;

using Rock.Attribute;
using Rock.Model;

namespace Rock.Blocks.Workflow
{
    /// <summary>
    /// Edits the details of a workflow Form Builder action.
    /// </summary>
    /// <seealso cref="Rock.Blocks.RockObsidianBlockType" />

    [DisplayName( "Form Builder Detail" )]
    [Category( "Obsidian > Workflow" )]
    [Description( "Edits the details of a workflow Form Builder action." )]
    [IconCssClass( "fa fa-hammer" )]

    public class FormBuilderDetail : RockObsidianBlockType
    {
    }
}
