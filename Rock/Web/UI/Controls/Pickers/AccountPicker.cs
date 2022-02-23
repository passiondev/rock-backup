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
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock.Data;
using Rock.Model;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// Control that can be used to select a financial account
    /// </summary>
    public class AccountPicker : ItemPicker
    {
        #region Controls

        /// <summary>
        /// The Select All button
        /// </summary>
        private HyperLink _btnSelectAll;

        /// <summary>
        /// The checkbox to show inactive groups
        /// </summary>
        private RockCheckBox _cbShowInactiveAccounts;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountPicker"/> class.
        /// </summary>
        public AccountPicker() : base()
        {
            this.ShowSelectChildren = true;
            this.EnhanceForLongLists = true;
            this.PickerMenuCssClasses = "picker-menu-w500 dropdown-menu";
            this.DisplayChildItemCountLabel = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [display active only].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [display active only]; otherwise, <c>false</c>.
        /// </value>
        public bool DisplayActiveOnly
        {
            get
            {
                return ViewState["DisplayActiveOnly"] as bool? ?? false;
            }

            set
            {
                ViewState["DisplayActiveOnly"] = value;
                SetAccountExtraRestParams();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [display public name].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [display public name]; otherwise, <c>false</c>.
        /// </value>
        public bool DisplayPublicName
        {
            get
            {
                return ViewState["DisplayPublicName"] as bool? ?? false;
            }

            set
            {
                ViewState["DisplayPublicName"] = value;
                SetAccountExtraRestParams();
            }
        }

        /// <summary>
        /// Gets or sets the selected account id.
        /// </summary>
        /// <value>
        /// The account id.
        /// </value>
        public int? AccountId
        {
            get
            {
                int selectedId = this.SelectedValue.AsInteger();
                if ( selectedId > 0 )
                {
                    return selectedId;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                SetValue( value );
            }
        }

        /// <summary>
        /// Gets or sets the root account identifier.
        /// </summary>
        /// <value>
        /// The root account identifier.
        /// </value>
        public int? RootAccountId
        {
            get
            {
                return ViewState["RootAccountId"] as int?;
            }

            set
            {
                ViewState["RootAccountId"] = value;
                SetAccountExtraRestParams();
            }
        }

        /// <summary>
        /// Gets or sets the included group type ids.
        /// </summary>
        /// <value>
        /// The included group type ids.
        /// </value>
        public List<int> IncludedAccountTypeIds
        {
            get
            {
                return ViewState["IncludedAccountTypeIds"] as List<int> ?? new List<int>();
            }

            set
            {
                ViewState["IncludedAccountTypeIds"] = value;
                SetAccountExtraRestParams();
            }

        }
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            _cbShowInactiveAccounts = new RockCheckBox
            {
                ContainerCssClass = "pull-right",
                SelectedIconCssClass = "fa fa-check-square-o",
                UnSelectedIconCssClass = "fa fa-square-o",
                ID = this.ID + $"{nameof( _cbShowInactiveAccounts )}",
                Text = "Show Inactive",
                AutoPostBack = true
            };
            _cbShowInactiveAccounts.CheckedChanged += _cbShowInactiveAccounts_CheckedChanged;
            this.Controls.Add( _cbShowInactiveAccounts );

            _btnSelectAll = new HyperLink
            {
                ID = "_btnSelectAll",
                CssClass = "btn btn-default btn-xs js-select-all pull-right",
                Text = "Select All"
            };

            this.Controls.Add( _btnSelectAll );
        }

        /// <summary>
        /// Render any additional picker actions
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void RenderCustomPickerActions( HtmlTextWriter writer )
        {
            base.RenderCustomPickerActions( writer );

            if ( this.AllowMultiSelect )
            {
                _btnSelectAll.RenderControl( writer );
            }

            _cbShowInactiveAccounts.RenderControl( writer );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            SetAccountExtraRestParams();
            this.IconCssClass = "fa fa-building-o";
        }

        /// <summary>
        /// This is where you implement the simple aspects of rendering your control.  The rest
        /// will be handled by calling RenderControlHelper's RenderControl() method.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void RenderBaseControl( HtmlTextWriter writer )
        {
            if ( EnableFullWidth )
            {
                this.RemoveCssClass( "picker-lg" );
            }
            else
            {
                this.AddCssClass( "picker-lg" );
            }

            // NOTE: The base ItemPicker.RenderBaseControl will do additional CSS class additions.
            base.RenderBaseControl( writer );
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="account">The account.</param>

        public void SetValue( FinancialAccount account )
        {
            if ( account != null )
            {
                ItemId = account.Id.ToString();

                var parentGroupIds = GetFinancialAccountAncestorsIdList( account.ParentAccount );
                InitialItemParentIds = parentGroupIds.AsDelimited( "," );
                ItemName = account.Name;
            }
            else
            {
                ItemId = Constants.None.IdValue;
                ItemName = Constants.None.TextHtml;
            }

        }
        /// <summary>
        /// Returns a list of the ancestor FinancialAccounts of the specified FinancialAccount.
        /// If the ParentFinancialAccount property of the FinancialAccount is not populated, it is assumed to be a top-level node.
        /// </summary>
        /// <param name="financialAccount">The financial account.</param>
        /// <param name="ancestorFinancialAccountIds">The ancestor financial account ids.</param>
        /// <returns></returns>
        private List<int> GetFinancialAccountAncestorsIdList( FinancialAccount financialAccount, List<int> ancestorFinancialAccountIds = null )
        {
            if ( ancestorFinancialAccountIds == null )
            {
                ancestorFinancialAccountIds = new List<int>();
            }

            if ( financialAccount == null )
            {
                return ancestorFinancialAccountIds;
            }

            // If we have encountered this node previously in our tree walk, there is a recursive loop in the tree.
            if ( ancestorFinancialAccountIds.Contains( financialAccount.Id ) )
            {
                return ancestorFinancialAccountIds;
            }

            // Create or add this node to the history stack for this tree walk.
            ancestorFinancialAccountIds.Insert( 0, financialAccount.Id );

            ancestorFinancialAccountIds = this.GetFinancialAccountAncestorsIdList( financialAccount.ParentAccount, ancestorFinancialAccountIds );

            return ancestorFinancialAccountIds;
        }

        /// <summary>
        /// Sets the values.
        /// </summary>
        /// <param name="accounts">The accounts.</param>
        public void SetValues( IEnumerable<FinancialAccount> accounts )
        {
            var financialAccounts = accounts.ToList();

            if ( financialAccounts != null && financialAccounts.Any() )
            {
                var ids = new List<string>();
                var names = new List<string>();
                var parrentAccountIds = new List<int>();

                foreach ( var account in accounts )
                {
                    if ( account != null )
                    {
                        ids.Add( account.Id.ToString() );
                        names.Add( this.DisplayPublicName ? account.PublicName : account.Name );
                        if ( account.ParentAccount != null && !parrentAccountIds.Contains( account.ParentAccount.Id ) )
                        {
                            var parrentAccount = account.ParentAccount;
                            var accountParentIds = GetFinancialAccountAncestorsIdList( parrentAccount );
                            foreach ( var accountParentId in accountParentIds )
                            {
                                if ( !parrentAccountIds.Contains( accountParentId ) )
                                {
                                    parrentAccountIds.Add( accountParentId );
                                }
                            }
                        }
                    }
                }

                // NOTE: Order is important (parents before children) since the GroupTreeView loads on demand
                InitialItemParentIds = parrentAccountIds.AsDelimited( "," );
                ItemIds = ids;
                ItemNames = names;
            }
            else
            {
                ItemId = Constants.None.IdValue;
                ItemName = Constants.None.TextHtml;
            }
        }

        /// <summary>
        /// Sets the value on select.
        /// </summary>
        protected override void SetValueOnSelect()
        {
            var accountId = ItemId.AsIntegerOrNull();
            FinancialAccount account = null;
            if ( accountId.HasValue && accountId > 0 )
            {
                account = new FinancialAccountService( new RockContext() ).Get( accountId.Value );
            }

            SetValue( account );
        }

        /// <summary>
        /// Sets the values on select.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void SetValuesOnSelect()
        {
            var accountIds = ItemIds.Where( i => i != "0" ).AsIntegerList();
            if ( accountIds.Any() )
            {
                var accounts = new FinancialAccountService( new RockContext() )
                    .Queryable()
                    .Where( g => accountIds.Contains( g.Id ) )
                    .ToList();
                this.SetValues( accounts );
            }
            else
            {
                this.SetValues( null );
            }
        }

        /// <summary>
        /// Gets the item rest URL.
        /// </summary>
        /// <value>
        /// The item rest URL.
        /// </value>
        public override string ItemRestUrl
        {
            get { return "~/api/financialaccounts/getchildren/"; }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the _cbShowInactiveAccounts control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void _cbShowInactiveAccounts_CheckedChanged( object sender, EventArgs e )
        {
            ShowDropDown = true;
           this.SetAccountExtraRestParams( _cbShowInactiveAccounts.Checked );
        }

        /// <summary>
        /// Sets the extra rest parameters.
        /// </summary>
        private void SetAccountExtraRestParams( bool includeInactiveAccounts = false )
        {
            var displayActiveOnly = this.DisplayActiveOnly || !includeInactiveAccounts;

            var extraParams = new System.Text.StringBuilder();
            extraParams.Append( $"/{displayActiveOnly}/{this.DisplayPublicName}" );
            if ( RootAccountId.HasValue )
            {
                extraParams.Append( $"&rootAccountId={RootAccountId.Value}" );
            }

            if ( IncludedAccountTypeIds != null && IncludedAccountTypeIds.Any() )
            {
                extraParams.Append( $"&includedAccountTypeIds={IncludedAccountTypeIds.AsDelimited( "," )}" );
            }
            ItemRestUrlExtraParams = extraParams.ToString();
        }
    }
}