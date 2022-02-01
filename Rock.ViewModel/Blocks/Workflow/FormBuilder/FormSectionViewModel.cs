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
    public class FormSectionViewModel
    {
        public Guid Guid { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool ShowHeadingSeparator { get; set; }

        public Guid? Type { get; set; }

        public List<FormFieldViewModel> Fields { get; set; }
    }
}
