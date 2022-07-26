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

using Rock.ViewModels.Utility;

namespace Rock.ViewModels.Blocks.Cms.ContentLibraryDetail
{
    /// <summary>
    /// Identifies a single library content source object in the system
    /// that could be added to the library.
    /// </summary>
    public class AvailableContentSourceBag
    {
        /// <summary>
        /// Gets or sets the unique identifier of the potential source.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the potential source.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the attributes that can be selected on the potential source.
        /// </summary>
        public List<ListItemBag> Attributes { get; set; }
    }
}