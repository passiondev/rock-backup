//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
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


namespace Rock.Client
{
    /// <summary>
    /// Base client model for ContentChannel that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class ContentChannelEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public string ChannelUrl { get; set; }

        /// <summary />
        public bool ChildItemsManuallyOrdered { get; set; }

        /// <summary />
        public int ContentChannelTypeId { get; set; }

        /// <summary />
        public Rock.Client.Enums.ContentControlType ContentControlType { get; set; }

        /// <summary />
        public string Description { get; set; }

        /// <summary />
        public bool EnableRss { get; set; }

        /// <summary />
        public Guid? ForeignGuid { get; set; }

        /// <summary />
        public string ForeignKey { get; set; }

        /// <summary />
        public string IconCssClass { get; set; }

        /// <summary />
        public bool IsIndexEnabled { get; set; }

        /// <summary />
        public bool IsTaggingEnabled { get; set; }

        /// <summary />
        public bool ItemsManuallyOrdered { get; set; }

        /// <summary />
        public int? ItemTagCategoryId { get; set; }

        /// <summary />
        public string ItemUrl { get; set; }

        /// <summary />
        public string Name { get; set; }

        /// <summary />
        public bool RequiresApproval { get; set; }

        /// <summary />
        public string RootImageDirectory { get; set; }

        /// <summary />
        public int? TimeToLive { get; set; }

        /// <summary />
        public DateTime? CreatedDateTime { get; set; }

        /// <summary />
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary />
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary />
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public int? ForeignId { get; set; }

        /// <summary>
        /// Copies the base properties from a source ContentChannel object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( ContentChannel source )
        {
            this.Id = source.Id;
            this.ChannelUrl = source.ChannelUrl;
            this.ChildItemsManuallyOrdered = source.ChildItemsManuallyOrdered;
            this.ContentChannelTypeId = source.ContentChannelTypeId;
            this.ContentControlType = source.ContentControlType;
            this.Description = source.Description;
            this.EnableRss = source.EnableRss;
            this.ForeignGuid = source.ForeignGuid;
            this.ForeignKey = source.ForeignKey;
            this.IconCssClass = source.IconCssClass;
            this.IsIndexEnabled = source.IsIndexEnabled;
            this.IsTaggingEnabled = source.IsTaggingEnabled;
            this.ItemsManuallyOrdered = source.ItemsManuallyOrdered;
            this.ItemTagCategoryId = source.ItemTagCategoryId;
            this.ItemUrl = source.ItemUrl;
            this.Name = source.Name;
            this.RequiresApproval = source.RequiresApproval;
            this.RootImageDirectory = source.RootImageDirectory;
            this.TimeToLive = source.TimeToLive;
            this.CreatedDateTime = source.CreatedDateTime;
            this.ModifiedDateTime = source.ModifiedDateTime;
            this.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            this.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }

    /// <summary>
    /// Client model for ContentChannel that includes all the fields that are available for GETs. Use this for GETs (use ContentChannelEntity for POST/PUTs)
    /// </summary>
    public partial class ContentChannel : ContentChannelEntity
    {
        /// <summary />
        public ICollection<ContentChannel> ChildContentChannels { get; set; }

        /// <summary />
        public ContentChannelType ContentChannelType { get; set; }

        /// <summary />
        public Category ItemTagCategory { get; set; }

    }
}
