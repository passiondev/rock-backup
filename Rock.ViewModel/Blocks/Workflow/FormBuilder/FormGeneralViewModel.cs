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

using Rock.ViewModel.NonEntities;

namespace Rock.ViewModel.Blocks.Workflow.FormBuilder
{
    public class FormGeneralViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? Template { get; set; }

        public ListItemViewModel Category { get; set; }

        public DateTimeOffset? EntryStarts { get; set; }

        public DateTimeOffset? EntryEnds { get; set; }

        public bool IsLoginRequired { get; set; }
    }
}
