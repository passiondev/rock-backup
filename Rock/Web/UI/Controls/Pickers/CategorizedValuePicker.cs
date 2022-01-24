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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock.Model;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// A picker control that allows a top-down, stepped selection of nodes from a tree structure.
    /// Values are selected for each level of the tree one at a time from the root, and each subsequent
    /// selection shows only the children of the previously selected node or nodes that do not have a parent.
    /// </summary>
    public class CategorizedValuePicker : CompositeControl, IRockControl, IRockChangeHandlerControl
    {
        private const string _emptyItemKey = "";
        private Repeater _categorySelectorRepeater = null;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueList"/> class.
        /// </summary>
        public CategorizedValuePicker() : base()
        {
            this.HelpBlock = new HelpBlock();
            this.WarningBlock = new WarningBlock();
            this.RequiredFieldValidator = new HiddenFieldValidator();
        }

        #endregion

        #region IRockControl implementation

        /// <summary>
        /// Gets or sets the label text.
        /// </summary>
        /// <value>
        /// The label text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The text for the label." )
        ]
        public string Label
        {
            get { return ViewState["Label"] as string ?? string.Empty; }
            set { ViewState["Label"] = value; }
        }

        /// <summary>
        /// Gets or sets the form group class.
        /// </summary>
        /// <value>
        /// The form group class.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        Description( "The CSS class to add to the form-group div." )
        ]
        public string FormGroupCssClass
        {
            get { return ViewState["FormGroupCssClass"] as string ?? string.Empty; }
            set { ViewState["FormGroupCssClass"] = value; }
        }

        /// <summary>
        /// Gets or sets the help text.
        /// </summary>
        /// <value>
        /// The help text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The help block." )
        ]
        public string Help
        {
            get
            {
                return HelpBlock != null ? HelpBlock.Text : string.Empty;
            }
            set
            {
                if ( HelpBlock != null )
                {
                    HelpBlock.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the warning text.
        /// </summary>
        /// <value>
        /// The warning text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The warning block." )
        ]
        public string Warning
        {
            get
            {
                return WarningBlock != null ? WarningBlock.Text : string.Empty;
            }
            set
            {
                if ( WarningBlock != null )
                {
                    WarningBlock.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RockTextBox"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        [
        Bindable( true ),
        Category( "Behavior" ),
        DefaultValue( "false" ),
        Description( "Is the value required?" )
        ]
        public bool Required
        {
            get { return ViewState["Required"] as bool? ?? false; }
            set { ViewState["Required"] = value; }
        }

        /// <summary>
        /// Gets or sets the required error message.  If blank, the LabelName name will be used
        /// </summary>
        /// <value>
        /// The required error message.
        /// </value>
        public string RequiredErrorMessage
        {
            get
            {
                return RequiredFieldValidator != null ? RequiredFieldValidator.ErrorMessage : string.Empty;
            }
            set
            {
                if ( RequiredFieldValidator != null )
                {
                    RequiredFieldValidator.ErrorMessage = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsValid
        {
            get
            {
                return !Required || RequiredFieldValidator == null || RequiredFieldValidator.IsValid;
            }
        }

        /// <summary>
        /// Gets or sets the help block.
        /// </summary>
        /// <value>
        /// The help block.
        /// </value>
        public HelpBlock HelpBlock { get; set; }

        /// <summary>
        /// Gets or sets the warning block.
        /// </summary>
        /// <value>
        /// The warning block.
        /// </value>
        public WarningBlock WarningBlock { get; set; }

        /// <summary>
        /// Gets or sets the required field validator.
        /// </summary>
        /// <value>
        /// The required field validator.
        /// </value>
        public RequiredFieldValidator RequiredFieldValidator { get; set; }

        /// <summary>
        /// Gets or sets an optional validation group to use.
        /// </summary>
        /// <value>
        /// The validation group.
        /// </value>
        public string ValidationGroup
        {
            get
            {
                return this.RequiredFieldValidator.ValidationGroup;
            }
            set
            {
                this.RequiredFieldValidator.ValidationGroup = value;
            }
        }

        #endregion

        #region IRockChangeHandlerControl

        /// <inheritdoc/>
        public event EventHandler ValueChanged;

        #endregion

        #region Controls

        /// <summary>
        /// Stores the current value of the control.
        /// </summary>
        private HiddenField _hfValue;

        /// <summary>
        /// Stores the current selection of the control.
        /// </summary>
        private HiddenField _hfSelection;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value prompt.
        /// </summary>
        /// <value>
        /// The value prompt.
        /// </value>
        public string ValuePrompt
        {
            get { return ViewState["ValuePrompt"] as string ?? "Value"; }
            set { ViewState["ValuePrompt"] = value; }
        }

        /// <summary>
        /// Gets or sets the hierarchy of values for the control.
        /// </summary>
        /// <value>
        /// The custom values.
        /// </value>
        public TreeNode<CategorizedValuePickerItem> ValueTree
        {
            get { return ViewState["ValueTree"] as TreeNode<CategorizedValuePickerItem>; }
            set { ViewState["ValueTree"] = value; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value
        {
            get
            {
                EnsureChildControls();
                return _hfValue.Value;
            }
            set
            {
                EnsureChildControls();

                SetSelection( value, out bool isChanged );
                CreateSelectionControls( value );

                if ( isChanged )
                {
                    ValueChanged?.Invoke( this, new EventArgs() );
                }
            }
        }

        /// <summary>
        /// Set the currently selected item for the control.
        /// </summary>
        /// <param name="itemKey"></param>
        /// <param name="isValueChanged"></param>
        private void SetSelection( string itemKey, out bool isValueChanged )
        {
            var selectedNode = ValueTree?.Find( x => x.Value.Key == itemKey ).FirstOrDefault();
            var isValue = ( selectedNode?.Value.ItemType == CategorizedValuePickerItemTypeSpecifier.Value );
            var isChanged = _hfSelection.Value != itemKey;

            if ( isChanged )
            {
                // Set the selected item and value properties.
                // The Value property is only set if the selected item is a Value type.
                _hfSelection.Value = itemKey;
                _hfValue.Value = isValue ? itemKey : string.Empty;

                isValueChanged = isValue;
            }
            else
            {
                isValueChanged = false;
            }
        }

        #endregion

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Clear();
            RockControlHelper.CreateChildControls( this, Controls );

            _hfValue = new HiddenField();
            _hfValue.ID = this.ID + "_hfValue";
            Controls.Add( _hfValue );
            _hfSelection = new HiddenField();
            _hfSelection.ID = this.ID + "_hfSelection";
            Controls.Add( _hfSelection );

            // Link the RequiredFieldValidator to the hidden field that stores the selected value.
            this.RequiredFieldValidator.ControlToValidate = _hfValue.ID;

            var template = new CategorizedValueSelectorTemplate();

            _categorySelectorRepeater = new Repeater { ID = "valueSelectorRepeater" };
            _categorySelectorRepeater.ItemTemplate = template;
            _categorySelectorRepeater.ItemDataBound += categorySelectorRepeater_ItemDataBound;

            Controls.Add( _categorySelectorRepeater );
        }

        /// <inheritdoc/>
        protected override void OnLoad( EventArgs e )
        {

            base.OnLoad( e );

            if ( !this.Page.IsPostBack )
            {
                CreateSelectionControls( this.Value );
            }

        }
        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( this.Visible )
            {
                // Set the required field message here because the control label may have been modified during initialization.
                this.RequiredFieldValidator.ErrorMessage = string.Format( "{0} must have a value.", this.Label );

                RockControlHelper.RenderControl( this, writer );
            }
        }

        private class SelectionControlInfo
        {
            public string NodeKey;
            public string Label;
            public List<CategorizedValuePickerItem> Items;
            public string SelectedItemKey;

            public override string ToString()
            {
                return $"[{NodeKey}] {Label} ({Items.Count},Selected={SelectedItemKey})";
            }
        }

        /// <summary>
        /// Handles the ItemDataBound event of the valueSelectorRepeater control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RepeaterItemEventArgs"/> instance containing the event data.</param>
        private void categorySelectorRepeater_ItemDataBound( object sender, RepeaterItemEventArgs e )
        {
            var repeaterItem = e.Item;
            var controlInfo = e.Item.DataItem as SelectionControlInfo;
            var ddlSelector = repeaterItem.FindControl( "ddlSelector" ) as RockDropDownList;
            var lLabel = repeaterItem.FindControl( "lLabel" ) as Label;

            lLabel.Text = controlInfo.Label;
            if ( this.Required )
            {
                lLabel.AddCssClass( "required-indicator" );
            }

            ddlSelector.Items.Clear();
            if ( controlInfo.Items != null )
            {
                foreach ( var item in controlInfo.Items )
                {
                    ddlSelector.Items.Add( new ListItem( item.Text, item.Key ) );
                }
            }
            ddlSelector.Required = this.Required;
            ddlSelector.SelectedValue = controlInfo.SelectedItemKey ?? _emptyItemKey;
        }

        private void CreateSelectionControls( string selectedNodeKey )
        {
            var selectionControls = new List<SelectionControlInfo>();

            if ( ValueTree != null )
            {
                // Get the currently selected node.
                var selectedNode = ValueTree.Find( x => x.Value.Key == selectedNodeKey ).FirstOrDefault();

                // If no selected node, find the first child node that allows a decision.
                var candidateNode = this.ValueTree;
                TreeNode<CategorizedValuePickerItem> selectorNode = selectedNode;

                while ( candidateNode != null && selectorNode == null )
                {
                    if ( candidateNode.Children.Count > 1 )
                    {
                        // If this node has multiple child nodes, make it the final selector.
                        selectorNode = candidateNode;
                    }
                    else if ( candidateNode.Children.Count == 1 )
                    {
                        if ( candidateNode.Children[0].Value.ItemType == CategorizedValuePickerItemTypeSpecifier.Value )
                        {
                            // If this node has a single child value, make it the final selector.
                            // This is a decision node because an empty selection is also possible.
                            selectorNode = candidateNode;
                        }
                        else
                        {
                            // If this node has a single child category, drill down further.
                            candidateNode = candidateNode[0];
                        }
                    }
                    else
                    {
                        // If this node has no child nodes, no selection is possible.
                        selectorNode = this.ValueTree;
                    }
                }

                selectorNode = selectorNode ?? this.ValueTree;

                // Create a selection control for each decision node, working back from the selected node to the root node.
                var selectorNodeKey = selectedNodeKey;
                while ( selectorNode != null )
                {
                    // Add a selector for this node if it represents a category.
                    if ( selectorNode.Value.ItemType == CategorizedValuePickerItemTypeSpecifier.Category )
                    {
                        var info = GetSelectorInfo( selectorNode, selectorNodeKey );

                        // If the node only contains a single category, skip the selector and drill-down to the next level.
                        var selectionNodes = info.Items.Where( x => x.ItemType != CategorizedValuePickerItemTypeSpecifier.Empty ).ToList();
                        var containsSingleCategory = selectionNodes.Count == 1
                            && selectionNodes[0].ItemType == CategorizedValuePickerItemTypeSpecifier.Category;

                        if ( !containsSingleCategory )
                        {
                            selectionControls.Add( info );
                        }
                    }

                    // Set the value of the parent selector control to select the current node.
                    selectorNodeKey = selectorNode.Value.Key;
                    // Move to the parent node
                    selectorNode = selectorNode.Parent;
                }

                // Reorder the controls with the top-level selection first.
                selectionControls.Reverse();

                // Add a value selector for the next unselected level in the hierarchy.
                if ( selectedNode != null
                     && selectedNode.Value.ItemType == CategorizedValuePickerItemTypeSpecifier.Category )
                {
                    var finalSelectionNode = selectedNode.Children.FirstOrDefault( x => x.Value.Key == selectedNodeKey );
                    if ( finalSelectionNode != null )
                    {
                        var info = GetSelectorInfo( finalSelectionNode, null );
                        selectionControls.Add( info );
                    }
                }
            }

            // If no ValueTree is defined, return a single empty selector.
            if ( !selectionControls.Any() )
            {
                selectionControls.Add( new SelectionControlInfo { NodeKey = _emptyItemKey, Label = string.Empty, Items = null } );
            }

            // Render the selection controls top-down.
            _categorySelectorRepeater.DataSource = selectionControls;
            _categorySelectorRepeater.DataBind();

            // Set the selected value.
            SetSelection( selectedNodeKey, out bool _ );
        }

        private SelectionControlInfo GetSelectorInfo( TreeNode<CategorizedValuePickerItem> selectorNode, string selectedNodeKey )
        {
            var availableNodes = GetSelectableNodesFromParentNode( selectorNode );

            availableNodes.Add( new CategorizedValuePickerItem { ItemType = CategorizedValuePickerItemTypeSpecifier.Empty, Text = "", Key = _emptyItemKey } );
            var info = new SelectionControlInfo
            {
                NodeKey = selectorNode.Value.Key,
                Label = selectorNode.Value.CategoryName,
                Items = availableNodes
            };

            if ( availableNodes.Any( x => x.Key == selectedNodeKey ) )
            {
                info.SelectedItemKey = selectedNodeKey;
            }

            return info;
        }

        /// <summary>
        /// Renders the base control.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public virtual void RenderBaseControl( HtmlTextWriter writer )
        {
            writer.AddAttribute( HtmlTextWriterAttribute.Class, "value-list" );
            writer.AddAttribute( HtmlTextWriterAttribute.Id, this.ClientID );
            writer.RenderBeginTag( HtmlTextWriterTag.Span );
            writer.WriteLine();

            _hfValue.RenderControl( writer );
            _hfSelection.RenderControl( writer );

            writer.WriteLine();

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "value-list-rows" );
            writer.RenderBeginTag( HtmlTextWriterTag.Span );
            writer.WriteLine();

            // Render the selection controls top-down.
            _categorySelectorRepeater.RenderControl( writer );

            writer.RenderEndTag();
            writer.WriteLine();

            writer.RenderEndTag();
            writer.WriteLine();
        }

        /// <summary>
        /// Get the list of nodes available for selection for the specified parent node.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        private List<CategorizedValuePickerItem> GetSelectableNodesFromParentNode( TreeNode<CategorizedValuePickerItem> parentNode )
        {
            var selectableNodes = new List<CategorizedValuePickerItem>();

            if ( parentNode == null )
            {
                return selectableNodes;
            }

            // If the node is a Value, there are no selections available.
            if ( parentNode.Value.ItemType == CategorizedValuePickerItemTypeSpecifier.Value
                 || parentNode.Value.ItemType == CategorizedValuePickerItemTypeSpecifier.Empty )
            {
                return selectableNodes;
            }

            // Selectable Nodes include:
            // 1. Category nodes that are immediate children of the parent selection.
            // 2. Value nodes that are children of the parent node any parent Category.
            // 3. Value nodes that do not have a parent Category.

            // Add Category nodes that are immediate children of the parent selection.
            var childCategoryNodes = parentNode.Children
                .Where( x => x.Value.ItemType == CategorizedValuePickerItemTypeSpecifier.Category )
                .Select( x => x.Value );
            selectableNodes.AddRange( childCategoryNodes );

            // Add Value nodes that are children of any parent Category.
            var categoryNode = parentNode;

            while ( categoryNode != null )
            {
                var valueNodes = categoryNode.Children
                    .Where( x => x.Value.ItemType == CategorizedValuePickerItemTypeSpecifier.Value || x.Value.ItemType == CategorizedValuePickerItemTypeSpecifier.Empty )
                    .Select( x => x.Value );
                selectableNodes.AddRange( valueNodes );

                categoryNode = categoryNode.Parent;
            }

            return selectableNodes;
        }

        #region Picker Template

        private class CategorizedValueSelectorTemplate : ITemplate
        {
            private DropDownList _ddlSelector { get; set; }

            /// <summary>
            /// When implemented by a class, defines the <see cref="T:System.Web.UI.Control" /> object that child controls and templates belong to. These child controls are in turn defined within an inline template.
            /// </summary>
            /// <param name="container">The <see cref="T:System.Web.UI.Control" /> object to contain the instances of controls from the inline template.</param>
            public void InstantiateIn( Control container )
            {
                var label = new Label() // HtmlGenericControl("span")
                {
                    ID = "lLabel"
                };

                _ddlSelector = new RockDropDownList
                {
                    DataTextField = "Value",
                    DataValueField = "Key",
                    ID = "ddlSelector",
                    AutoPostBack = true,
                };

                _ddlSelector.AddCssClass( "form-control input-width-lg js-value-list-input" );
                _ddlSelector.SelectedIndexChanged += ddlSelector_SelectedIndexChanged;

                container.Controls.Add( label );
                container.Controls.Add( _ddlSelector );
            }

            private void ddlSelector_SelectedIndexChanged( object sender, EventArgs e )
            {
                var ddlSelector = sender as DropDownList;

                var picker = ddlSelector?.FirstParentControlOfType<CategorizedValuePicker>();

                if ( picker == null )
                {
                    return;
                }

                var newValue = ddlSelector.SelectedValue;
                if ( newValue == _emptyItemKey )
                {
                    var previousValue = picker.Value;
                    var previousNode = picker.ValueTree.Find( x => x.Value.Key == previousValue ).FirstOrDefault();

                    if ( previousNode != null )
                    {
                        newValue = previousNode?.Parent.Value.Key ?? _emptyItemKey;
                    }
                }

                picker.CreateSelectionControls( newValue );
            }
        }

        #endregion
    }

    #region Support classes

    /// <summary>
    /// Specifies the type of an item added to a CategorizedValuePicker
    /// </summary>
    public enum CategorizedValuePickerItemTypeSpecifier
    {
        /// <summary>
        /// An item representing an empty selection.
        /// </summary>
        Empty = 0,
        /// <summary>
        /// A category that can contain child categories or values.
        /// Categories represent an interim selection that is available while navigating the list of values.
        /// </summary>
        Category = 1,
        /// <summary>
        /// A selectable value.
        /// </summary>
        Value = 2
    }

    /// <summary>
    /// An entry in a categorized value picker.
    /// </summary>
    public class CategorizedValuePickerItem
    {
        /// <summary>
        /// The unique key of the item.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The key of the parent item, or null if the item has no parent.
        /// </summary>
        public string ParentKey { get; set; }

        /// <summary>
        /// The text to display for this item.
        /// </summary>
        public string Text;

        /// <summary>
        /// The name of the category this item represents, or empty if the item represents a value.
        /// </summary>
        public string CategoryName = null;

        /// <summary>
        /// A flag indicating the type of list item.
        /// </summary>
        public CategorizedValuePickerItemTypeSpecifier ItemType;

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[{Key}] {Text} [{CategoryName}]";
        }
    }

    #endregion

    #region Tree Data Structure

    /// <summary>
    /// A tree data structure where each node maintains child and parent pointers, to allow for simple traversal of the tree.
    /// </summary>
    internal class TreeNode
    {
        /// <summary>
        /// Builds a tree data structure from a list of objects related by identifiable key and parent key properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="keySelector"></param>
        /// <param name="parentKeySelector"></param>
        /// <param name="rootNodeParentKey"></param>
        /// <returns></returns>
        public static List<TreeNode<T>> BuildTree<T>( IEnumerable<T> objects, Func<T, string> keySelector, Func<T, string> parentKeySelector, string rootNodeParentKey = null )
        {
            var rootNodes = new List<TreeNode<T>>();

            // Create a set of new tree nodes for the data objects, then map the nodes by key value.
            var allNodes = objects.Select( x => new TreeNode<T>( x ) ).ToList();
            var nodeKeyMap = allNodes.ToDictionary( x => keySelector( x.Value ) );

            // Add the node relationships.
            foreach ( var currentNode in allNodes )
            {
                var parentKey = parentKeySelector( currentNode.Value );
                if ( parentKey == rootNodeParentKey )
                {
                    rootNodes.Add( currentNode );
                }
                else if ( !nodeKeyMap.ContainsKey( parentKey ) )
                {
                    // If the parent key reference is invalid, add this as a root node.
                    rootNodes.Add( currentNode );
                }
                else
                {
                    nodeKeyMap[parentKey].AddChild( currentNode );
                }
            }

            return rootNodes;
        }
    }

    /// <summary>
    /// A simple tree data structure implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNode<T>
    {
        private T _value;
        private List<TreeNode<T>> _children = new List<TreeNode<T>>();

        #region Constructors

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="value"></param>
        public TreeNode( T value )
        {
            _value = value;
        }

        #endregion

        /// <summary>
        /// Gets a child node by index.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public TreeNode<T> this[int i]
        {
            get { return _children[i]; }
        }

        /// <summary>
        /// Gets the parent of this node.
        /// </summary>
        public TreeNode<T> Parent { get; private set; }

        /// <summary>
        /// The value object stored in this tree node.
        /// </summary>
        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets the collection of child nodes for this node.
        /// </summary>
        public ReadOnlyCollection<TreeNode<T>> Children
        {
            get { return _children.AsReadOnly(); }
        }

        /// <summary>
        /// Add a child node to this node.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TreeNode<T> AddChild( T value )
        {
            // Create a new node and add it to the collection of child nodes.
            var node = new TreeNode<T>( value ) { Parent = this };
            _children.Add( node );
            return node;
        }

        /// <summary>
        /// Add a child node to this node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TreeNode<T> AddChild( TreeNode<T> node )
        {
            // Re-parent the node and add it to the collection of child nodes.
            node.Parent = this;
            _children.Add( node );
            return node;
        }

        /// <summary>
        /// Add a collection of child nodes to this node.
        /// </summary>
        /// <param name="nodes"></param>
        public void AddChildren( IEnumerable<TreeNode<T>> nodes )
        {
            foreach ( var node in nodes )
            {
                AddChild( node );
            }
        }

        /// <summary>
        /// Add a collection of child nodes to this node.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public List<TreeNode<T>> AddChildren( IEnumerable<T> items )
        {
            var nodes = items.Select( x => new TreeNode<T>( x ) ).ToList();
            AddChildren( nodes );
            return nodes;
        }

        /// <summary>
        /// Remove a child node from this node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool RemoveChild( TreeNode<T> node )
        {
            return _children.Remove( node );
        }

        /// <summary>
        /// Gets all descendant nodes that match the specified predicate, including this node.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<TreeNode<T>> Find( Func<TreeNode<T>, bool> predicate )
        {
            var nodes = Flatten().Where( predicate ).ToList();
            return nodes;
        }

        /// <summary>
        /// Get a collection of descendant nodes, including this node.
        /// </summary>
        /// <returns></returns>
        public List<TreeNode<T>> Flatten()
        {
            var nodes = new List<TreeNode<T>>();

            nodes.Add( this );
            nodes.AddRange( _children.SelectMany( x => x.Flatten() ) );

            return nodes;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return _value == null ? base.ToString() : _value.ToString();
        }
    }

    #endregion
}