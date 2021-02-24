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
    /// Page View Model
    /// </summary>
    [ViewModelOf( typeof( Rock.Model.Page ) )]
    public partial class PageViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the AdditionalSettings.
        /// </summary>
        /// <value>
        /// The AdditionalSettings.
        /// </value>
        public string AdditionalSettings { get; set; }

        /// <summary>
        /// Gets or sets the AllowIndexing.
        /// </summary>
        /// <value>
        /// The AllowIndexing.
        /// </value>
        public bool AllowIndexing { get; set; }

        /// <summary>
        /// Gets or sets the BodyCssClass.
        /// </summary>
        /// <value>
        /// The BodyCssClass.
        /// </value>
        public string BodyCssClass { get; set; }

        /// <summary>
        /// Gets or sets the BreadCrumbDisplayIcon.
        /// </summary>
        /// <value>
        /// The BreadCrumbDisplayIcon.
        /// </value>
        public bool BreadCrumbDisplayIcon { get; set; }

        /// <summary>
        /// Gets or sets the BreadCrumbDisplayName.
        /// </summary>
        /// <value>
        /// The BreadCrumbDisplayName.
        /// </value>
        public bool BreadCrumbDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the BrowserTitle.
        /// </summary>
        /// <value>
        /// The BrowserTitle.
        /// </value>
        public string BrowserTitle { get; set; }

        /// <summary>
        /// Gets or sets the CacheControlHeaderSettings.
        /// </summary>
        /// <value>
        /// The CacheControlHeaderSettings.
        /// </value>
        public string CacheControlHeaderSettings { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// The Description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the DisplayInNavWhen.
        /// </summary>
        /// <value>
        /// The DisplayInNavWhen.
        /// </value>
        public int DisplayInNavWhen { get; set; }

        /// <summary>
        /// Gets or sets the EnableViewState.
        /// </summary>
        /// <value>
        /// The EnableViewState.
        /// </value>
        public bool EnableViewState { get; set; }

        /// <summary>
        /// Gets or sets the HeaderContent.
        /// </summary>
        /// <value>
        /// The HeaderContent.
        /// </value>
        public string HeaderContent { get; set; }

        /// <summary>
        /// Gets or sets the IconBinaryFileId.
        /// </summary>
        /// <value>
        /// The IconBinaryFileId.
        /// </value>
        public int? IconBinaryFileId { get; set; }

        /// <summary>
        /// Gets or sets the IconCssClass.
        /// </summary>
        /// <value>
        /// The IconCssClass.
        /// </value>
        public string IconCssClass { get; set; }

        /// <summary>
        /// Gets or sets the IncludeAdminFooter.
        /// </summary>
        /// <value>
        /// The IncludeAdminFooter.
        /// </value>
        public bool IncludeAdminFooter { get; set; }

        /// <summary>
        /// Gets or sets the InternalName.
        /// </summary>
        /// <value>
        /// The InternalName.
        /// </value>
        public string InternalName { get; set; }

        /// <summary>
        /// Gets or sets the IsSystem.
        /// </summary>
        /// <value>
        /// The IsSystem.
        /// </value>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the KeyWords.
        /// </summary>
        /// <value>
        /// The KeyWords.
        /// </value>
        public string KeyWords { get; set; }

        /// <summary>
        /// Gets or sets the LayoutId.
        /// </summary>
        /// <value>
        /// The LayoutId.
        /// </value>
        public int LayoutId { get; set; }

        /// <summary>
        /// Gets or sets the MedianPageLoadTimeDurationSeconds.
        /// </summary>
        /// <value>
        /// The MedianPageLoadTimeDurationSeconds.
        /// </value>
        public double? MedianPageLoadTimeDurationSeconds { get; set; }

        /// <summary>
        /// Gets or sets the MenuDisplayChildPages.
        /// </summary>
        /// <value>
        /// The MenuDisplayChildPages.
        /// </value>
        public bool MenuDisplayChildPages { get; set; }

        /// <summary>
        /// Gets or sets the MenuDisplayDescription.
        /// </summary>
        /// <value>
        /// The MenuDisplayDescription.
        /// </value>
        public bool MenuDisplayDescription { get; set; }

        /// <summary>
        /// Gets or sets the MenuDisplayIcon.
        /// </summary>
        /// <value>
        /// The MenuDisplayIcon.
        /// </value>
        public bool MenuDisplayIcon { get; set; }

        /// <summary>
        /// Gets or sets the Order.
        /// </summary>
        /// <value>
        /// The Order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the PageDisplayBreadCrumb.
        /// </summary>
        /// <value>
        /// The PageDisplayBreadCrumb.
        /// </value>
        public bool PageDisplayBreadCrumb { get; set; }

        /// <summary>
        /// Gets or sets the PageDisplayDescription.
        /// </summary>
        /// <value>
        /// The PageDisplayDescription.
        /// </value>
        public bool PageDisplayDescription { get; set; }

        /// <summary>
        /// Gets or sets the PageDisplayIcon.
        /// </summary>
        /// <value>
        /// The PageDisplayIcon.
        /// </value>
        public bool PageDisplayIcon { get; set; }

        /// <summary>
        /// Gets or sets the PageDisplayTitle.
        /// </summary>
        /// <value>
        /// The PageDisplayTitle.
        /// </value>
        public bool PageDisplayTitle { get; set; }

        /// <summary>
        /// Gets or sets the PageTitle.
        /// </summary>
        /// <value>
        /// The PageTitle.
        /// </value>
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the ParentPageId.
        /// </summary>
        /// <value>
        /// The ParentPageId.
        /// </value>
        public int? ParentPageId { get; set; }

        /// <summary>
        /// Gets or sets the RequiresEncryption.
        /// </summary>
        /// <value>
        /// The RequiresEncryption.
        /// </value>
        public bool RequiresEncryption { get; set; }

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
        public virtual void SetPropertiesFrom( Rock.Model.Page model, Person currentPerson = null, bool loadAttributes = true )
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
            AdditionalSettings = model.AdditionalSettings;
            AllowIndexing = model.AllowIndexing;
            BodyCssClass = model.BodyCssClass;
            BreadCrumbDisplayIcon = model.BreadCrumbDisplayIcon;
            BreadCrumbDisplayName = model.BreadCrumbDisplayName;
            BrowserTitle = model.BrowserTitle;
            CacheControlHeaderSettings = model.CacheControlHeaderSettings;
            Description = model.Description;
            DisplayInNavWhen = ( int ) model.DisplayInNavWhen;
            EnableViewState = model.EnableViewState;
            HeaderContent = model.HeaderContent;
            IconBinaryFileId = model.IconBinaryFileId;
            IconCssClass = model.IconCssClass;
            IncludeAdminFooter = model.IncludeAdminFooter;
            InternalName = model.InternalName;
            IsSystem = model.IsSystem;
            KeyWords = model.KeyWords;
            LayoutId = model.LayoutId;
            MedianPageLoadTimeDurationSeconds = model.MedianPageLoadTimeDurationSeconds;
            MenuDisplayChildPages = model.MenuDisplayChildPages;
            MenuDisplayDescription = model.MenuDisplayDescription;
            MenuDisplayIcon = model.MenuDisplayIcon;
            Order = model.Order;
            PageDisplayBreadCrumb = model.PageDisplayBreadCrumb;
            PageDisplayDescription = model.PageDisplayDescription;
            PageDisplayIcon = model.PageDisplayIcon;
            PageDisplayTitle = model.PageDisplayTitle;
            PageTitle = model.PageTitle;
            ParentPageId = model.ParentPageId;
            RequiresEncryption = model.RequiresEncryption;
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
        public static PageViewModel From( Rock.Model.Page model, Person currentPerson = null, bool loadAttributes = true )
        {
            var viewModel = new PageViewModel();
            viewModel.SetPropertiesFrom( model, currentPerson, loadAttributes );
            return viewModel;
        }
    }
}
