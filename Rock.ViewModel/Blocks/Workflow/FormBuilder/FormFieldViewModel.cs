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

namespace Rock.ViewModel.Blocks.Workflow.FormBuilder
{
    public class FormFieldViewModel
    {
        public Guid Guid { get; set; }

        public Guid FieldTypeGuid { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Key { get; set; }

        public int Size { get; set; }

        public bool IsRequired { get; set; }

        public bool IsHideLabel { get; set; }

        public bool IsShowOnGrid { get; set; }

        public Dictionary<string, string> ConfigurationValues { get; set; }

        public string DefaultValue { get; set; }
    }
}
