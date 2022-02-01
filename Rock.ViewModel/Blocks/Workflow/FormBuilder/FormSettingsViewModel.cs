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

using System.Collections.Generic;

namespace Rock.ViewModel.Blocks.Workflow.FormBuilder
{
    public class FormSettingsViewModel
    {
        public string HeaderContent { get; set; }

        public string FooterContent { get; set; }

        public List<FormSectionViewModel> Sections { get; set; }

        public FormGeneralViewModel General { get; set; }

        public FormConfirmationEmailViewModel ConfirmationEmail { get; set; }

        public FormNotificationEmailViewModel NotificationEmail { get; set; }

        public FormCompletionActionViewModel Completion { get; set; }

        public bool AllowPersonEntry { get; set; }

        public FormPersonEntryViewModel PersonEntry { get; set; }
    }
}
