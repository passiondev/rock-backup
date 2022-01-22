﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// PDF Viewer
    /// </summary>
    public class PDFViewer : CompositeControl, INamingContainer
    {
        #region Controls

        private Literal _lPDFContainer;

        #endregion Controls

        /// <summary>
        /// Gets or sets the URL of the PDF File
        /// </summary>
        /// <value>The source URL.</value>
        public string SourceUrl { get; set; }

        /// <summary>
        /// The Height of the PDF Viewer
        /// </summary>
        /// <value>The height of the view.</value>
        public string ViewerHeight { get; set; } = "400px";

        /// <summary>
        /// The initial Fit View.
        /// <br />
        /// <p><c>FitH</c> = fit to the width of the PDF</p>
        /// <br />
        /// <p><c>FitV</c> = fit to the height of the PDF</p>
        /// </summary>
        /// <value>The fit view.</value>
        public string FitView { get; set; } = "FitH";

        /// <summary>
        /// Gets or sets Pdf PageMode option: 'bookmarks, thumbs, none'
        /// </summary>
        /// <value>The page mode.</value>
        public string PageMode { get; set; } = "none";

        /// <summary>
        /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag" /> value that corresponds to this Web server control. This property is used primarily by control developers.
        /// </summary>
        /// <value>The tag key.</value>
        protected override HtmlTextWriterTag TagKey => HtmlTextWriterTag.Div;

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            if ( !ScriptManager.GetCurrent( this.Page ).IsInAsyncPostBack )
            {
                RockPage.AddScriptLink( Page, "~/Scripts/PDFObject/pdfobject.min.js" );
            }

            base.OnInit( e );
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            _lPDFContainer = new Literal
            {
                ID = "_lPDFContainer",
            };

            Controls.Add( _lPDFContainer );
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            _lPDFContainer.Text = $"<div id='{_lPDFContainer.ClientID}'></div>";
            base.RenderControl( writer );
            RegisterJavaScript();
        }

        /// <summary>
        /// Registers the java script.
        /// </summary>
        protected virtual void RegisterJavaScript()
        {
            if ( ScriptManager.GetCurrent( this.Page ).IsInAsyncPostBack )
            {
                ScriptManager.RegisterClientScriptInclude( this.Page, this.Page.GetType(), "pdf_object-include", ResolveUrl( "~/Scripts/PDFObject/pdfobject.min.js" ) );
            }

            var pdfObjectScript = $@"
(function($) {{
    var options = {{
        height: '{ViewerHeight}',
        page: '1',
        pdfOpenParams: {{
            view: '{FitView}',
            pagemode: '{PageMode}',
            search: ''
        }}
    }};

    PDFObject.embed('{SourceUrl}', '#{_lPDFContainer.ClientID}', options);
}})();
";

            ScriptManager.RegisterStartupScript( this, this.GetType(), "pdf_viewer_script" + this.ClientID, pdfObjectScript, true );
        }
    }
}
