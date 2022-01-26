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
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock.Model;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// Control for Drawing or Typing an Electronic Signature
    /// </summary>
    public class ElectronicSignatureControl : CompositeControl, INamingContainer
    {
        #region ViewState Keys

        private static class ViewStateKey
        {
            public const string DocumentTerm = "DocumentTerm";
            public const string SignatureType = "SignatureType";
            public const string DrawnSignatureImageMimeType = "DrawnSignatureImageMimeType";
        }

        #endregion ViewState Keys
        #region Controls

        private HiddenFieldWithClass _hfSignatureImageDataUrl;
        private Panel _pnlSignatureEntryDrawn;
        private Literal _lSignaturePadCanvas;
        private Panel _pnlSignatureEntryTyped;
        private RockTextBox _tbSignatureTyped;
        private Literal _lSignatureSignDisclaimer;
        private BootstrapButton _btnSignSignature;
        private Literal _clearSignatureLink;
        private CustomValidator _customValidator;
        private ValidationSummary _customValidationSummary;

        #endregion Controls

        /// <inheritdoc cref="SignatureDocumentTemplate.SignatureType"/>
        public SignatureType SignatureType
        {
            get
            {
                return this.ViewState[ViewStateKey.SignatureType] as SignatureType? ?? SignatureType.Typed;
            }

            set
            {
                EnsureChildControls();

                this.ViewState[ViewStateKey.SignatureType] = value;
                _pnlSignatureEntryDrawn.Visible = value == SignatureType.Drawn;
                _pnlSignatureEntryTyped.Visible = value == SignatureType.Typed;
            }
        }

        /// <inheritdoc cref="SignatureDocumentTemplate.DocumentTerm"/>
        public string DocumentTerm
        {
            get => this.ViewState[ViewStateKey.DocumentTerm] as string;

            set
            {
                EnsureChildControls();
                this.ViewState[ViewStateKey.DocumentTerm] = value;

                SetSignatureSignDisclaimerText( _lSignatureSignDisclaimer, value );
            }
        }

        /// <summary>
        /// Gets or sets the image mime type (<c>image/png, image/jpeg, image/svg+xml</c>) to do used capturing the signature image data. Defaults to <c>image/png</c>.
        /// See also <see cref="DrawnSignatureImageDataUrl"/>
        /// </summary>
        public string DrawnSignatureImageMimeType
        {
            get
            {
                return this.ViewState[ViewStateKey.DrawnSignatureImageMimeType] as string ?? "image/png";
            }

            set
            {
                this.ViewState[ViewStateKey.DrawnSignatureImageMimeType] = value;
            }
        }

        /// <summary>
        /// Gets the signature image data URL when in <see cref="SignatureType.Drawn"/> mode. After drawing a signature,
        /// this would be an img URL in Base64 format
        /// </summary>
        /// <value>The signature image data URL.</value>
        public string DrawnSignatureImageDataUrl
        {
            get
            {
                EnsureChildControls();
                return _hfSignatureImageDataUrl.Value;
            }
        }

        /// <summary>
        /// Gets the typed signature text when in <see cref="SignatureType.Typed" /> mode.
        /// </summary>
        /// <value>The typed signature text.</value>
        public string TypedSignatureText
        {
            get
            {
                EnsureChildControls();
                return _tbSignatureTyped.Text;
            }
        }

        /// <summary>
        /// Sets the signature sign disclaimer text.
        /// </summary>
        /// <param name="lSignatureSignDisclaimer">The l signature sign disclaimer.</param>
        /// <param name="documentTerm">The document term.</param>
        private void SetSignatureSignDisclaimerText(Literal lSignatureSignDisclaimer, string documentTerm )
        {
            lSignatureSignDisclaimer.Text = $"By clicking the sign button below, I agree to the above {documentTerm} and understand this is a legal representation of my signature.";
        }

        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            if ( !ScriptManager.GetCurrent( this.Page ).IsInAsyncPostBack )
            {
                RockPage.AddScriptLink( Page, "~/Scripts/signature_pad/signature_pad.umd.min.js" );
            }

            base.OnInit( e );
        }

        /// <summary>
        /// Have this control render as a div instead of a span
        /// </summary>
        /// <value>The tag key.</value>
        protected override HtmlTextWriterTag TagKey => HtmlTextWriterTag.Div;

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            // this is what will display validation message from _customValidator
            _customValidationSummary = new ValidationSummary();
            _customValidationSummary.ID = "_customValidationSummary";
            _customValidationSummary.ValidationGroup = $"vsElectronicSignatureControl_{this.ID}";
            //_customValidationSummary.HeaderText = "Please correct the following:";
            _customValidationSummary.CssClass = "alert alert-validation";
            Controls.Add( _customValidationSummary );

            // Add custom validator that will check for blank drawn or typed signature
            _customValidator = new CustomValidator();
            _customValidator.ID = "_customValidator";
            _customValidator.ClientValidationFunction = "Rock.controls.electronicSignatureControl.clientValidate";
            _customValidator.ErrorMessage = "Please enter a signature";
            _customValidator.Enabled = true;
            _customValidator.Display = ValidatorDisplay.Dynamic;
            _customValidator.ValidationGroup = _customValidationSummary.ValidationGroup;
            Controls.Add( _customValidator );

            _hfSignatureImageDataUrl = new HiddenFieldWithClass();
            _hfSignatureImageDataUrl.ID = "_hfSignatureImageDataUrl";
            _hfSignatureImageDataUrl.CssClass = "js-signature-data";
            Controls.Add( _hfSignatureImageDataUrl );

            /* Controls for Drawn Signature*/
            _pnlSignatureEntryDrawn = new Panel();
            _pnlSignatureEntryDrawn.ID = "_pnlSignatureEntryDrawn";
            _pnlSignatureEntryDrawn.CssClass = "signature-entry-drawn js-signature-entry-drawn";
            Controls.Add( _pnlSignatureEntryDrawn );

            // drawing canvas
            var pnlSignatureEntryDrawnCanvasDiv = new Panel() { CssClass = "todo" };
            _pnlSignatureEntryDrawn.Controls.Add( pnlSignatureEntryDrawnCanvasDiv );

            _lSignaturePadCanvas = new Literal();
            _lSignaturePadCanvas.ID = "_lSignaturePadCanvas";
            _lSignaturePadCanvas.Text = "<canvas class='js-signature-pad-canvas e-signature-pad'></canvas>";
            pnlSignatureEntryDrawnCanvasDiv.Controls.Add( _lSignaturePadCanvas );

            // clear signature button
            var pnlSignatureEntryDrawnActionDiv = new Panel() { CssClass = "todo" };
            _pnlSignatureEntryDrawn.Controls.Add( pnlSignatureEntryDrawnActionDiv );

            _clearSignatureLink = new Literal();
            _clearSignatureLink.ID = "_clearSignatureLink";
            _clearSignatureLink.Text = $@"<a class='btn btn-default js-clear-signature'><i class='fa fa-undo'></i></a>";
            pnlSignatureEntryDrawnActionDiv.Controls.Add( _clearSignatureLink );

            /* Controls for Typed Signature*/

            _pnlSignatureEntryTyped = new Panel();
            _pnlSignatureEntryTyped.ID = "_pnlSignatureEntryTyped";
            _pnlSignatureEntryTyped.CssClass = "signature-entry-typed";
            Controls.Add( _pnlSignatureEntryTyped );

            _tbSignatureTyped = new RockTextBox();
            _tbSignatureTyped.ID = "_tbSignatureTyped";
            _tbSignatureTyped.Placeholder = "Type Name";
            _tbSignatureTyped.CssClass = "js-signature-typed";
            _pnlSignatureEntryTyped.Controls.Add( _tbSignatureTyped );

            /* */
            _lSignatureSignDisclaimer = new Literal();
            _lSignatureSignDisclaimer.ID = "_lSignatureSignDisclaimer";
            SetSignatureSignDisclaimerText( _lSignatureSignDisclaimer, this.DocumentTerm );
            this.Controls.Add( _lSignatureSignDisclaimer );

            _btnSignSignature = new BootstrapButton();
            _btnSignSignature.ID = "_btnSignSignature";
            _btnSignSignature.CssClass = "btn btn-default js-save-signature";
            _btnSignSignature.Text = "Sign";
            _btnSignSignature.Click += _btnSignSignature_Click;
            this.Controls.Add( _btnSignSignature );

            _pnlSignatureEntryDrawn.Visible = SignatureType == SignatureType.Drawn;
            _pnlSignatureEntryTyped.Visible = SignatureType == SignatureType.Typed;
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            this.AddCssClass( "js-electronic-signature-control" );
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
                ScriptManager.RegisterClientScriptInclude( this.Page, this.Page.GetType(), "signature_pad-include", ResolveUrl( "~/Scripts/signature_pad/signature_pad.umd.min.js" ) );
            }

            var electronicSignatureControlScript =
$@"Rock.controls.electronicSignatureControl.initialize({{
    controlId: '{this.ClientID}',
    imageMimeType: '{this.DrawnSignatureImageMimeType}'
}})
";

            ScriptManager.RegisterStartupScript( this, this.GetType(), "electronicSignatureControl_script" + this.ClientID, electronicSignatureControlScript, true );
        }

        /// <summary>
        /// Handles the Click event of the _btnSignSignature control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void _btnSignSignature_Click( object sender, EventArgs e )
        {
            SignSignatureClicked?.Invoke( sender, e );
        }

        /// <summary>
        /// Occurs when the 'Sign' button under the typed or drawn signature is clicked
        /// </summary>
        public event EventHandler SignSignatureClicked;

        #endregion Base Control Methods
    }
}
