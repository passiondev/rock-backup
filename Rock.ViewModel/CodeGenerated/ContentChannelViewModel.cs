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
using System.Linq;
using Rock.Attribute;
using Rock.Model;
using Rock.Web.Cache;

namespace Rock.ViewModel
{
    /// <summary>
    /// ContentChannel View Model
    /// </summary>
    [ViewModelOf( typeof( Rock.Model.ContentChannel ) )]
    public partial class ContentChannelViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the ChannelUrl.
        /// </summary>
        /// <value>
        /// The ChannelUrl.
        /// </value>
        public string ChannelUrl { get; set; }

        /// <summary>
        /// Gets or sets the ChildItemsManuallyOrdered.
        /// </summary>
        /// <value>
        /// The ChildItemsManuallyOrdered.
        /// </value>
        public bool ChildItemsManuallyOrdered { get; set; }

        /// <summary>
        /// Gets or sets the ContentChannelTypeId.
        /// </summary>
        /// <value>
        /// The ContentChannelTypeId.
        /// </value>
        public int ContentChannelTypeId { get; set; }

        /// <summary>
        /// Gets or sets the ContentControlType.
        /// </summary>
        /// <value>
        /// The ContentControlType.
        /// </value>
        public int ContentControlType { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// The Description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the EnableRss.
        /// </summary>
        /// <value>
        /// The EnableRss.
        /// </value>
        public bool EnableRss { get; set; }

        /// <summary>
        /// Gets or sets the IconCssClass.
        /// </summary>
        /// <value>
        /// The IconCssClass.
        /// </value>
        public string IconCssClass { get; set; }

        /// <summary>
        /// Gets or sets the IsIndexEnabled.
        /// </summary>
        /// <value>
        /// The IsIndexEnabled.
        /// </value>
        public bool IsIndexEnabled { get; set; }

        /// <summary>
        /// Gets or sets the IsStructuredContent.
        /// </summary>
        /// <value>
        /// The IsStructuredContent.
        /// </value>
        public bool IsStructuredContent { get; set; }

        /// <summary>
        /// Gets or sets the IsTaggingEnabled.
        /// </summary>
        /// <value>
        /// The IsTaggingEnabled.
        /// </value>
        public bool IsTaggingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the ItemsManuallyOrdered.
        /// </summary>
        /// <value>
        /// The ItemsManuallyOrdered.
        /// </value>
        public bool ItemsManuallyOrdered { get; set; }

        /// <summary>
        /// Gets or sets the ItemTagCategoryId.
        /// </summary>
        /// <value>
        /// The ItemTagCategoryId.
        /// </value>
        public int? ItemTagCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the ItemUrl.
        /// </summary>
        /// <value>
        /// The ItemUrl.
        /// </value>
        public string ItemUrl { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the RequiresApproval.
        /// </summary>
        /// <value>
        /// The RequiresApproval.
        /// </value>
        public bool RequiresApproval { get; set; }

        /// <summary>
        /// Gets or sets the RootImageDirectory.
        /// </summary>
        /// <value>
        /// The RootImageDirectory.
        /// </value>
        public string RootImageDirectory { get; set; }

        /// <summary>
        /// Gets or sets the StructuredContentToolValueId.
        /// </summary>
        /// <value>
        /// The StructuredContentToolValueId.
        /// </value>
        public int? StructuredContentToolValueId { get; set; }

        /// <summary>
        /// Gets or sets the TimeToLive.
        /// </summary>
        /// <value>
        /// The TimeToLive.
        /// </value>
        public int? TimeToLive { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDateTime.
        /// </summary>
        /// <value>
        /// The CreatedDateTime.
        /// </value>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedDateTime.
        /// </summary>
        /// <value>
        /// The ModifiedDateTime.
        /// </value>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the CreatedByPersonAliasId.
        /// </summary>
        /// <value>
        /// The CreatedByPersonAliasId.
        /// </value>
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedByPersonAliasId.
        /// </summary>
        /// <value>
        /// The ModifiedByPersonAliasId.
        /// </value>
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary>
        /// Sets the properties from.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="currentPerson">The current person.</param>
        /// <param name="loadAttributes">if set to <c>true</c> [load attributes].</param>
        public virtual void SetPropertiesFrom( Rock.Model.ContentChannel model, Person currentPerson = null, bool loadAttributes = true )
        {
            if ( model == null )
            {
                return;
            }

            if ( loadAttributes && model is IHasAttributes hasAttributes )
            {
                if ( hasAttributes.Attributes == null )
                {
                    hasAttributes.LoadAttributes();
                }

                Attributes = hasAttributes.AttributeValues.Where( av =>
                {
                    var attribute = AttributeCache.Get( av.Value.AttributeId );
                    return attribute?.IsAuthorized( Rock.Security.Authorization.EDIT, currentPerson ) ?? false;
                } ).ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.ToViewModel<AttributeValueViewModel>() as object );
            }

            Id = model.Id;
            Guid = model.Guid;
            ChannelUrl = model.ChannelUrl;
            ChildItemsManuallyOrdered = model.ChildItemsManuallyOrdered;
            ContentChannelTypeId = model.ContentChannelTypeId;
            ContentControlType = ( int ) model.ContentControlType;
            Description = model.Description;
            EnableRss = model.EnableRss;
            IconCssClass = model.IconCssClass;
            IsIndexEnabled = model.IsIndexEnabled;
            IsStructuredContent = model.IsStructuredContent;
            IsTaggingEnabled = model.IsTaggingEnabled;
            ItemsManuallyOrdered = model.ItemsManuallyOrdered;
            ItemTagCategoryId = model.ItemTagCategoryId;
            ItemUrl = model.ItemUrl;
            Name = model.Name;
            RequiresApproval = model.RequiresApproval;
            RootImageDirectory = model.RootImageDirectory;
            StructuredContentToolValueId = model.StructuredContentToolValueId;
            TimeToLive = model.TimeToLive;
            CreatedDateTime = model.CreatedDateTime;
            ModifiedDateTime = model.ModifiedDateTime;
            CreatedByPersonAliasId = model.CreatedByPersonAliasId;
            ModifiedByPersonAliasId = model.ModifiedByPersonAliasId;

            SetAdditionalPropertiesFrom( model, currentPerson, loadAttributes );
        }

        /// <summary>
        /// Creates a view model from the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="currentPerson" >The current person.</param>
        /// <param name="loadAttributes" >if set to <c>true</c> [load attributes].</param>
        /// <returns></returns>
        public static ContentChannelViewModel From( Rock.Model.ContentChannel model, Person currentPerson = null, bool loadAttributes = true )
        {
            var viewModel = new ContentChannelViewModel();
            viewModel.SetPropertiesFrom( model, currentPerson, loadAttributes );
            return viewModel;
        }
    }
}
