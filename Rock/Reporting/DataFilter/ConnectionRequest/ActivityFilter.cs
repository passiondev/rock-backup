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
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI.Controls;

namespace Rock.Reporting.DataFilter.ConnectionRequest
{
    /// <summary>
    ///
    /// </summary>
    [Description( "Would allow fitering by activity types." )]
    [Export( typeof( DataFilterComponent ) )]
    [ExportMetadata( "ComponentName", "Activity Filter" )]
    public class ActivityFilter : DataFilterComponent
    {
        #region Properties

        /// <summary>
        /// Gets the entity type that filter applies to.
        /// </summary>
        /// <value>
        /// The entity that filter applies to.
        /// </value>
        public override string AppliesToEntityType
        {
            get { return typeof( Rock.Model.ConnectionRequest ).FullName; }
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <value>
        /// The section.
        /// </value>
        public override string Section
        {
            get { return "Additional Filters"; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        /// <value>
        /// The title.
        /// </value>
        public override string GetTitle( Type entityType )
        {
            return "Activity";
        }

        /// <summary>
        /// Formats the selection on the client-side.  When the filter is collapsed by the user, the Filterfield control
        /// will set the description of the filter to whatever is returned by this property.  If including script, the
        /// controls parent container can be referenced through a '$content' variable that is set by the control before
        /// referencing this property.
        /// </summary>
        /// <value>
        /// The client format script.
        /// </value>
        public override string GetClientFormatSelection( Type entityType )
        {
            return "'Activity ' + " +
                "'\\'' + $('.js-activity-type', $content).find(':selected').text() + '\\' ' + " +
                "$('.js-filter-compare', $content).find(':selected').text() + ' ' + " +
                "$('.js-count', $content).val() + ' times. Date Range: ' + " +
                "$('.js-slidingdaterange-text-value', $content).val()";
        }

        /// <summary>
        /// Formats the selection.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="selection">The selection.</param>
        /// <returns></returns>
        public override string FormatSelection( Type entityType, string selection )
        {
            string s = "Activity";

            string[] options = selection.Split( '|' );
            if ( options.Length >= 4 )
            {
                var activityType = new ConnectionActivityTypeService( new RockContext() ).Get( options[0].AsGuid() );
                var activityName = GetActivityName( activityType );
                ComparisonType comparisonType = options[1].ConvertToEnum<ComparisonType>( ComparisonType.GreaterThanOrEqualTo );

                string dateRangeText;
                int? lastXWeeks = options[3].AsIntegerOrNull();
                if ( lastXWeeks.HasValue )
                {
                    dateRangeText = " in last " + ( lastXWeeks.Value * 7 ).ToString() + " days";
                }
                else
                {
                    dateRangeText = SlidingDateRangePicker.FormatDelimitedValues( options[3].Replace( ',', '|' ) );
                }

                s = string.Format(
                    "Activity '{0}' {1} {2} times. Date Range: {3}",
                    activityName,
                    comparisonType.ConvertToString(),
                    options[2],
                    dateRangeText );
            }

            return s;
        }

        /// <summary>
        /// Creates the child controls.
        /// </summary>
        /// <returns></returns>
        public override Control[] CreateChildControls( Type entityType, FilterField filterControl )
        {
            var ddlActivityType = new RockDropDownList();
            ddlActivityType.ID = filterControl.ID + "_ddlActivityType";
            ddlActivityType.AddCssClass( "js-activity-type" );
            ddlActivityType.Label = "Activity Type";
            filterControl.Controls.Add( ddlActivityType );
            var activityTypes = new ConnectionActivityTypeService( new RockContext() ).Queryable( "ConnectionType" ).AsNoTracking().Where( a => a.IsActive )
                .OrderBy( a => a.ConnectionTypeId.HasValue )
                .ThenBy( a => a.Name )
                .ToList();
            ddlActivityType.Items.Clear();
            ddlActivityType.Items.Insert( 0, new ListItem() );
            foreach ( var activityType in activityTypes )
            {
                var activityName = GetActivityName( activityType );
                ddlActivityType.Items.Add( new ListItem( activityName, activityType.Guid.ToString() ) );
            }

            var ddlIntegerCompare = ComparisonHelper.ComparisonControl( ComparisonHelper.NumericFilterComparisonTypes );
            ddlIntegerCompare.Label = "Count";
            ddlIntegerCompare.ID = filterControl.ID + "_ddlIntegerCompare";
            ddlIntegerCompare.AddCssClass( "js-filter-compare" );
            filterControl.Controls.Add( ddlIntegerCompare );

            var nbCount = new NumberBox();
            nbCount.ID = filterControl.ID + "_nbCount";
            nbCount.Label = "&nbsp;"; // give it whitespace label so it lines up nicely
            nbCount.AddCssClass( "js-count" );
            filterControl.Controls.Add( nbCount );

            var slidingDateRangePicker = new SlidingDateRangePicker();
            slidingDateRangePicker.Label = "Date Range";
            slidingDateRangePicker.ID = filterControl.ID + "_slidingDateRangePicker";
            slidingDateRangePicker.AddCssClass( "js-sliding-date-range" );
            filterControl.Controls.Add( slidingDateRangePicker );

            var controls = new Control[4] { ddlActivityType, ddlIntegerCompare, nbCount, slidingDateRangePicker };

            // convert pipe to comma delimited
            var defaultDelimitedValues = slidingDateRangePicker.DelimitedValues.Replace( "|", "," );
            var defaultCount = 1;

            // set the default values in case this is a newly added filter
            SetSelection(
                entityType,
                controls,
                string.Format( "{0}|{1}|{2}|{3}", ddlActivityType.SelectedValue, ComparisonType.GreaterThanOrEqualTo.ConvertToInt().ToString(), defaultCount, defaultDelimitedValues ) );

            return controls;
        }

        /// <summary>
        /// Renders the controls.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="filterControl">The filter control.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="controls">The controls.</param>
        public override void RenderControls( Type entityType, FilterField filterControl, HtmlTextWriter writer, Control[] controls )
        {
            var ddlActivityType = controls[0] as RockDropDownList;
            var ddlIntegerCompare = controls[1] as RockDropDownList;
            var nbCount = controls[2] as NumberBox;
            var slidingDateRangePicker = controls[3] as SlidingDateRangePicker;

            // Row 1
            writer.AddAttribute( HtmlTextWriterAttribute.Class, "row form-row" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.AddAttribute( "class", "col-md-6" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            ddlActivityType.RenderControl( writer );
            writer.RenderEndTag();

            writer.RenderEndTag();

            // Row 2
            writer.AddAttribute( HtmlTextWriterAttribute.Class, "row form-row" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.AddAttribute( "class", "col-md-4" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            ddlIntegerCompare.RenderControl( writer );
            writer.RenderEndTag();

            writer.AddAttribute( "class", "col-md-1" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            nbCount.RenderControl( writer );
            writer.RenderEndTag();

            writer.AddAttribute( "class", "col-md-7" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            slidingDateRangePicker.RenderControl( writer );
            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        /// <summary>
        /// Gets the selection.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="controls">The controls.</param>
        /// <returns></returns>
        public override string GetSelection( Type entityType, Control[] controls )
        {
            var ddlActivityType = controls[0] as RockDropDownList;
            var ddlIntegerCompare = controls[1] as RockDropDownList;
            var nbCount = controls[2] as NumberBox;
            var slidingDateRangePicker = controls[3] as SlidingDateRangePicker;

            // convert the date range from pipe-delimited to comma since we use pipe delimited for the selection values
            var dateRangeCommaDelimitedValues = slidingDateRangePicker.DelimitedValues.Replace( '|', ',' );
            return string.Format( "{0}|{1}|{2}|{3}", ddlActivityType.SelectedValue, ddlIntegerCompare.SelectedValue, nbCount.Text, dateRangeCommaDelimitedValues );
        }

        /// <summary>
        /// Sets the selection.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="controls">The controls.</param>
        /// <param name="selection">The selection.</param>
        public override void SetSelection( Type entityType, Control[] controls, string selection )
        {
            var ddlActivityType = controls[0] as RockDropDownList;
            var ddlIntegerCompare = controls[1] as RockDropDownList;
            var nbCount = controls[2] as NumberBox;
            var slidingDateRangePicker = controls[3] as SlidingDateRangePicker;

            string[] options = selection.Split( '|' );
            if ( options.Length >= 4 )
            {
                ddlActivityType.SelectedValue = options[0];
                ddlIntegerCompare.SelectedValue = options[1];
                nbCount.Text = options[2];
                int? lastXWeeks = options[3].AsIntegerOrNull();

                if ( lastXWeeks.HasValue )
                {
                    //// selection was from when it just simply a LastXWeeks instead of Sliding Date Range
                    // Last X Weeks was treated as "LastXWeeks * 7" days, so we have to convert it to a SlidingDateRange of Days to keep consistent behavior
                    slidingDateRangePicker.SlidingDateRangeMode = SlidingDateRangePicker.SlidingDateRangeType.Last;
                    slidingDateRangePicker.TimeUnit = SlidingDateRangePicker.TimeUnitType.Day;
                    slidingDateRangePicker.NumberOfTimeUnits = lastXWeeks.Value * 7;
                }
                else
                {
                    // convert from comma-delimited to pipe since we store it as comma delimited so that we can use pipe delimited for the selection values
                    var dateRangeCommaDelimitedValues = options[3];
                    string slidingDelimitedValues = dateRangeCommaDelimitedValues.Replace( ',', '|' );
                    slidingDateRangePicker.DelimitedValues = slidingDelimitedValues;
                }
            }
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="serviceInstance">The service instance.</param>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <param name="selection">The selection.</param>
        /// <returns></returns>
        public override Expression GetExpression( Type entityType, IService serviceInstance, ParameterExpression parameterExpression, string selection )
        {
            string[] options = selection.Split( '|' );
            if ( options.Length < 4 )
            {
                return null;
            }

            Guid activityTypeGuid = options[0].AsGuid();
            ComparisonType comparisonType = options[1].ConvertToEnum<ComparisonType>( ComparisonType.GreaterThanOrEqualTo );
            int? count = options[2].AsIntegerOrNull();
            string slidingDelimitedValues;

            if ( options[3].AsIntegerOrNull().HasValue )
            {
                //// selection was from when it just simply a LastXWeeks instead of Sliding Date Range
                // Last X Weeks was treated as "LastXWeeks * 7" days, so we have to convert it to a SlidingDateRange of Days to keep consistent behavior
                int lastXWeeks = options[3].AsIntegerOrNull() ?? 1;
                var fakeSlidingDateRangePicker = new SlidingDateRangePicker();
                fakeSlidingDateRangePicker.SlidingDateRangeMode = SlidingDateRangePicker.SlidingDateRangeType.Last;
                fakeSlidingDateRangePicker.TimeUnit = SlidingDateRangePicker.TimeUnitType.Day;
                fakeSlidingDateRangePicker.NumberOfTimeUnits = lastXWeeks * 7;
                slidingDelimitedValues = fakeSlidingDateRangePicker.DelimitedValues;
            }
            else
            {
                slidingDelimitedValues = options[3].Replace( ',', '|' );
            }

            var dateRange = SlidingDateRangePicker.CalculateDateRangeFromDelimitedValues( slidingDelimitedValues );
            var rockContext = serviceInstance.Context as RockContext;
            var connectionRequestActivityQry = new ConnectionRequestActivityService( rockContext ).Queryable();

            if ( dateRange.Start.HasValue )
            {
                var startDate = dateRange.Start.Value;
                connectionRequestActivityQry = connectionRequestActivityQry.Where( a => a.CreatedDateTime >= startDate );
            }

            if ( dateRange.End.HasValue )
            {
                var endDate = dateRange.End.Value;
                connectionRequestActivityQry = connectionRequestActivityQry.Where( a => a.CreatedDateTime < endDate );
            }

            connectionRequestActivityQry = connectionRequestActivityQry.Where( a => a.ConnectionActivityType.Guid == activityTypeGuid );

            var qry = new ConnectionRequestService( rockContext ).Queryable()
                  .Where( p => connectionRequestActivityQry.Where( xx => xx.ConnectionRequestId == p.Id ).Count() == count );

            BinaryExpression compareEqualExpression = FilterExpressionExtractor.Extract<Rock.Model.ConnectionRequest>( qry, parameterExpression, "p" ) as BinaryExpression;
            BinaryExpression result = FilterExpressionExtractor.AlterComparisonType( comparisonType, compareEqualExpression, null );

            return result;
        }

        private string GetActivityName( ConnectionActivityType activityType )
        {
            var activityName = string.Empty;
            if ( activityType != null )
            {
                activityName = activityType.Name;
                if ( activityType.ConnectionType != null )
                {
                    activityName += $" ( {activityType.ConnectionType.Name} )";
                }
            }

            return activityName;
        }

        #endregion
    }
}