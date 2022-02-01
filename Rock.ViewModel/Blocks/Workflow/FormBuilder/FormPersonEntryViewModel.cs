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
    public class FormPersonEntryViewModel
    {
        public bool AutofillCurrentPerson { get; set; }

        public bool HideIfCurrentPersonKnown { get; set; }

        public Guid? RecordStatus { get; set; }

        public Guid? ConnectionStatus { get; set; }

        public bool ShowCampus { get; set; }

        public Guid? CampusType { get; set; }

        public Guid? CampusStatus { get; set; }

        public FieldVisibilityRule Gender { get; set; }

        public FieldVisibilityRule Email { get; set; }

        public FieldVisibilityRule MobilePhone { get; set; }

        public FieldVisibilityRule Birthdate { get; set; }

        public FieldVisibilityRule Address { get; set; }

        public Guid? AddressType { get; set; }

        public FieldVisibilityRule MaritalStatus { get; set; }

        public FieldVisibilityRule SpouseEntry { get; set; }

        public string SpouseLabel { get; set; }
    }
}
