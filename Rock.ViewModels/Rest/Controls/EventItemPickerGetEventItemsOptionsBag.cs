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

using System;
using System.Collections.Generic;

namespace Rock.ViewModels.Rest.Controls
{
    /// <summary>
    /// The options that can be passed to the GetEventItems API action of
    /// the EventItemPicker control.
    /// </summary>
    public class EventItemPickerGetEventItemsOptionsBag
    {
        /// <summary>
        /// Whether or not to include inactive event items
        /// </summary>
        /// <value>The option to include inactive event items</value>
        public bool IncludeInactive { get; set; } = false;
    }
}
