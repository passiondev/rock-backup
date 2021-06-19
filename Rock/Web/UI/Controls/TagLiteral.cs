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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// A <see cref="T:System.Web.UI.WebControls.Literal"/> control with an associated label.
    /// </summary>
    [ToolboxData( "<{0}:TagLiteral runat=server></{0}:TagLiteral>" )]
    public class TagLiteral : Literal
    {
        #region Properties

        /// <summary>
        /// Gets or sets the label text.
        /// </summary>
        /// <value>
        /// The label text.
        /// </value>
        public string Tag
        {
            get { return ViewState["Tag"] as string ?? string.Empty; }
            set { ViewState["Tag"] = value; }
        }

        /// <summary>
        /// Gets or sets the CSS class.
        /// </summary>
        /// <value>
        /// The CSS class.
        /// </value>
        public string CssClass
        {
            get { return ViewState["CssClass"] as string ?? string.Empty; }
            set { ViewState["CssClass"] = value; }
        }

        #endregion

        /// <summary>
        /// Renders a label and <see cref="T:System.Web.UI.WebControls.TextBox"/> control to the specified <see cref="T:System.Web.UI.HtmlTextWriter"/> object.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> that receives the rendered output.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( Visible && Text.IsNotNullOrWhiteSpace() )
            {
                if ( Tag.IsNotNullOrWhiteSpace() )
                {
                    if ( CssClass.IsNotNullOrWhiteSpace() )
                    {
                        writer.AddAttribute( HtmlTextWriterAttribute.Class, CssClass );
                    }
                    writer.RenderBeginTag( Tag );
                }

                base.RenderControl( writer );

                if ( Tag.IsNotNullOrWhiteSpace() )
                {
                    writer.RenderEndTag();
                }
            }
        }
    }
}