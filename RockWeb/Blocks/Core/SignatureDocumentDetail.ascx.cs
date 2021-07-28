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
using System.Collections.Generic;
using System.Linq;

using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;

using Rock.Model;
using Rock.Web;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using Rock.Security;
using System.Data.Entity;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI;
using Rock.Web.Cache;

namespace RockWeb.Blocks.Core
{
    /// <summary>
    /// User controls for managing signature documents
    /// </summary>
    [DisplayName( "Signature Document Detail" )]
    [Category( "Core" )]
    [Description( "Displays the details of a given signature document." )]

    public partial class SignatureDocumentDetail : RockBlock, IDetailBlock
    {
        /// <summary>
        /// The ua parser
        /// </summary>
        private static UAParser.Parser uaParser = UAParser.Parser.GetDefault();

        #region PageParameterKeys

        private static class PageParameterKey
        {
            public const string SignatureDocumentTemplateId = "SignatureDocumentTemplateId";
            public const string SignatureDocumentId = "SignatureDocumentId";
            public const string PersonId = "personId";
        }

        #endregion PageParameterKey

        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );


            RockPage.AddScriptLink( "~/Scripts/pdfobject.min.js" );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlSettings );

            rbStatus.BindToEnum<SignatureDocumentStatus>();

            ddlDocumentType.Items.Clear();
            using ( var rockContext = new RockContext() )
            {
                ddlDocumentType.DataSource = new SignatureDocumentTemplateService( rockContext )
                    .Queryable().AsNoTracking()
                    .OrderBy( t => t.Name )
                    .Select( t => new
                    {
                        t.Id,
                        t.Name
                    } )
                    .ToList();
                ddlDocumentType.DataBind();
                ddlDocumentType.Items.Insert( 0, new ListItem( "", "" ) );
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            nbErrorMessage.Visible = false;

            if ( !Page.IsPostBack )
            {
                ShowDetail( PageParameter( "SignatureDocumentId" ).AsInteger() );
            }
            else
            {
                RouteAction();
            }
        }




        #endregion

        #region Events

        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnEdit_Click( object sender, EventArgs e )
        {
            var signatureDocument = new SignatureDocumentService( new RockContext() ).Get( hfSignatureDocumentId.Value.AsInteger() );
            ShowEditDetails( signatureDocument, false );
        }

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            ShowDetail( PageParameter( "SignatureDocumentId" ).AsInteger() );
        }

        /// <summary>
        /// Handles the Click event of the btnSaveType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            bool inviteCancelled = false;

            var rockContext = new RockContext();

            int? documentTemplateId = ddlDocumentType.SelectedValueAsInt();
            if ( !documentTemplateId.HasValue )
            {
                nbErrorMessage.Title = string.Empty;
                nbErrorMessage.Text = "Document Template is Required!";
                nbErrorMessage.NotificationBoxType = NotificationBoxType.Danger;
                nbErrorMessage.Visible = true;
                return;
            }

            SignatureDocument signatureDocument = null;
            SignatureDocumentService service = new SignatureDocumentService( rockContext );

            int signatureDocumentId = hfSignatureDocumentId.ValueAsInt();

            int? origBinaryFileId = null;

            if ( signatureDocumentId != 0 )
            {
                signatureDocument = service.Get( signatureDocumentId );
            }

            if ( signatureDocument == null )
            {
                signatureDocument = new SignatureDocument();
                service.Add( signatureDocument );
            }

            signatureDocument.Name = tbName.Text;

            var newStatus = rbStatus.SelectedValueAsEnum<SignatureDocumentStatus>( SignatureDocumentStatus.None );
            if ( signatureDocument.Status != newStatus )
            {
                signatureDocument.Status = newStatus;
                signatureDocument.LastStatusDate = RockDateTime.Now;
                inviteCancelled = newStatus == SignatureDocumentStatus.Cancelled;
            }

            signatureDocument.AppliesToPersonAliasId = ppAppliesTo.PersonAliasId;
            signatureDocument.AssignedToPersonAliasId = ppAssignedTo.PersonAliasId;
            signatureDocument.SignedByPersonAliasId = ppSignedBy.PersonAliasId;

            signatureDocument.SignatureDocumentTemplateId = documentTemplateId.Value;

            origBinaryFileId = signatureDocument.BinaryFileId;
            signatureDocument.BinaryFileId = fuDocument.BinaryFileId;

            if ( !signatureDocument.IsValid )
            {
                // Controls will render the error messages                    
                return;
            }

            BinaryFileService binaryFileService = new BinaryFileService( rockContext );
            if ( origBinaryFileId.HasValue && origBinaryFileId.Value != signatureDocument.BinaryFileId )
            {
                // if a new the binaryFile was uploaded, mark the old one as Temporary so that it gets cleaned up
                var oldBinaryFile = binaryFileService.Get( origBinaryFileId.Value );
                if ( oldBinaryFile != null && !oldBinaryFile.IsTemporary )
                {
                    oldBinaryFile.IsTemporary = true;
                }
            }

            // ensure the IsTemporary is set to false on binaryFile associated with this document
            if ( signatureDocument.BinaryFileId.HasValue )
            {
                var binaryFile = binaryFileService.Get( signatureDocument.BinaryFileId.Value );
                if ( binaryFile != null && binaryFile.IsTemporary )
                {
                    binaryFile.IsTemporary = false;
                }
            }

            rockContext.SaveChanges();

            if ( inviteCancelled && !string.IsNullOrWhiteSpace( signatureDocument.DocumentKey ) )
            {
                var errorMessages = new List<string>();
                if ( new SignatureDocumentTemplateService( rockContext ).CancelDocument( signatureDocument, out errorMessages ) )
                {
                    rockContext.SaveChanges();
                }
            }

            NavigateToCurrentPageReference( new Dictionary<string, string>
            {
                { PageParameterKey.SignatureDocumentId, signatureDocument.Id.ToString() }
            } );
        }

        /// <summary>
        /// Handles the Click event of the btnCancelType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            if ( hfSignatureDocumentId.Value.Equals( "0" ) )
            {
                // Cancelling on Add
                Dictionary<string, string> qryString = new Dictionary<string, string>();
                qryString[PageParameterKey.SignatureDocumentTemplateId] = hfSignatureTemplateId.Value;
                NavigateToParentPage( qryString );
            }
            else
            {
                // Cancelling on Edit
                var signatureDocument = new SignatureDocumentService( new RockContext() ).Get( int.Parse( hfSignatureDocumentId.Value ) );
                ShowReadonlyDetails( signatureDocument );
            }
        }

        /// <summary>
        /// Registers the startup script.
        /// </summary>
        private void RegisterScript( string fileUrl )
        {
            string scriptFormat = @"
    Sys.Application.add_load(function () {{
       var options = {{
            height: '400px'
      }};
        PDFObject.embed('{0}', '#divPDF', options);
    }});
";
            string script = string.Format(
                scriptFormat,
                fileUrl      // {0}
            );

            ScriptManager.RegisterStartupScript( upnlSettings, this.GetType(), "pdf-document", script, true );
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="signatureDocument">Type of the defined.</param>
        private void ShowReadonlyDetails( SignatureDocument signatureDocument )
        {
            SetEditMode( false );

            hfSignatureDocumentId.SetValue( signatureDocument.Id );

            lTitle.Text = string.Format( "{0} for {1}", signatureDocument.Name, signatureDocument.AppliesToPersonAlias.Person.FullName ).FormatAsHtmlTitle();
            hlTemplate.Text = signatureDocument.SignatureDocumentTemplate.Name;

            var lDetails = new DescriptionList();
            var rDetails = new DescriptionList();
            string rockUrlRoot = ResolveRockUrl( "/" );

            if ( signatureDocument.SignedByPersonAlias != null && signatureDocument.SignedByPersonAlias.Person != null )
            {
                lDetails.Add( "Signed By", signatureDocument.SignedByPersonAlias.Person.GetAnchorTag( rockUrlRoot ) );
            }

            if ( signatureDocument.AssignedToPersonAlias != null && signatureDocument.AssignedToPersonAlias.Person != null && signatureDocument.AssignedToPersonAlias.Person.Email.IsNotNullOrWhiteSpace() )
            {
                var str = new StringBuilder();
                str.Append( signatureDocument.AssignedToPersonAlias.Person.Email );
                str.AppendLine( "<br/>" );
                if ( signatureDocument.CompletionEmailSentDateTime.HasValue )
                {
                    str.AppendLine( "Last Sent On: " + signatureDocument.CompletionEmailSentDateTime.Value.ToShortDateTimeString() );
                }

                var lavaStr = @"<a class='btn-sm btn btn-default' onclick=""{{ SignatureDocument.Id | Postback:'Send' }}"">{{ LinkText }}</a>";
                var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( this.RockPage, this.CurrentPerson );
                mergeFields.Add( "SignatureDocument", signatureDocument );
                mergeFields.Add( "LinkText", signatureDocument.Status == SignatureDocumentStatus.Sent ? "Resend" : "Send Invite" );
                str.Append( lavaStr.ResolveMergeFields( mergeFields ) );
                lDetails.Add( "Confirmation Email", str.ToString() );
            }

            if ( signatureDocument.AppliesToPersonAlias != null && signatureDocument.AppliesToPersonAlias.Person != null )
            {
                lDetails.Add( "Applies To", signatureDocument.AppliesToPersonAlias.Person.FullName );
            }

            if ( signatureDocument.BinaryFile != null )
            {
                var getFileUrl = string.Format( "{0}GetFile.ashx?guid={1}", System.Web.VirtualPathUtility.ToAbsolute( "~" ), signatureDocument.BinaryFile.Guid );
                RegisterScript( getFileUrl );
            }

            var signedOnDetails = new List<string>();
            if ( signatureDocument.SignedDateTime.HasValue )
            {
                signedOnDetails.Add( signatureDocument.SignedDateTime.Value.ToShortDateTimeString() );
            }

            if ( signatureDocument.SignedClientUserAgent.IsNotNullOrWhiteSpace() )
            {
                var deviceApplication = uaParser.ParseUserAgent( signatureDocument.SignedClientUserAgent ).ToString();
                var deviceOs = uaParser.ParseOS( signatureDocument.SignedClientUserAgent ).ToString();
                var deviceClientType = InteractionDeviceType.GetClientType( signatureDocument.SignedClientUserAgent );
                if ( deviceApplication.IsNotNullOrWhiteSpace() )
                {
                    signedOnDetails.Add( deviceApplication );
                }

                if ( deviceOs.IsNotNullOrWhiteSpace() )
                {
                    signedOnDetails.Add( deviceOs );
                }

                if ( deviceClientType.IsNotNullOrWhiteSpace() )
                {
                    signedOnDetails.Add( deviceClientType );
                }
            }

            if ( signatureDocument.SignedClientIp.IsNotNullOrWhiteSpace() )
            {
                signedOnDetails.Add( signatureDocument.SignedClientIp );
            }

            if ( signedOnDetails.Any() )
            {
                rDetails.Add( "Signed On", signedOnDetails.AsDelimited( "<br/>" ) );
            }

            if ( signatureDocument.EntityTypeId.HasValue && signatureDocument.EntityId.HasValue )
            {
                var entityType = EntityTypeCache.Get( signatureDocument.EntityTypeId.Value );
                if ( entityType != null )
                {
                    var entity = Reflection.GetIEntityForEntityType( entityType.GetEntityType(), signatureDocument.EntityId.Value );
                    if ( entity != null )
                    {
                        string txt = string.Empty;
                        if ( entityType.LinkUrlLavaTemplate.IsNotNullOrWhiteSpace() )
                        {
                            var url = entityType.LinkUrlLavaTemplate.ResolveMergeFields( new Dictionary<string, object> { { "Entity", entity } } );
                            txt = string.Format( "<a href='{0}'>{1}</a>", ResolveRockUrl( url ), entity.ToString() );
                        }
                        else
                        {
                            txt = entity.ToString();
                        }
                        
                        rDetails.Add( "Related Entity", txt );
                    }
                }
            }

            lLeftDetails.Text = lDetails.Html;
            lRightDetails.Text = rDetails.Html;

        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="signatureDocument">Type of the defined.</param>
        private void ShowEditDetails( SignatureDocument signatureDocument, bool onlyStatusDetails )
        {
            if ( !onlyStatusDetails )
            {
                string titleName = signatureDocument.SignatureDocumentTemplate != null ? signatureDocument.SignatureDocumentTemplate.Name + " Document" : SignatureDocument.FriendlyTypeName;
                if ( signatureDocument.Id > 0 )
                {
                    lTitle.Text = ActionTitle.Edit( titleName ).FormatAsHtmlTitle();
                }
                else
                {
                    lTitle.Text = ActionTitle.Add( titleName ).FormatAsHtmlTitle();
                }

                SetEditMode( true );

                tbName.Text = signatureDocument.Name;

                ppAppliesTo.SetValue( signatureDocument.AppliesToPersonAlias != null ? signatureDocument.AppliesToPersonAlias.Person : null );
                ppAssignedTo.SetValue( signatureDocument.AssignedToPersonAlias != null ? signatureDocument.AssignedToPersonAlias.Person : null );
                ppSignedBy.SetValue( signatureDocument.SignedByPersonAlias != null ? signatureDocument.SignedByPersonAlias.Person : null );

                ddlDocumentType.Visible = signatureDocument.SignatureDocumentTemplate == null;
                ddlDocumentType.SetValue( signatureDocument.SignatureDocumentTemplateId );

                if ( signatureDocument.SignatureDocumentTemplate != null && signatureDocument.SignatureDocumentTemplate.BinaryFileType != null )
                {
                    fuDocument.BinaryFileTypeGuid = signatureDocument.SignatureDocumentTemplate.BinaryFileType.Guid;
                }
                fuDocument.BinaryFileId = signatureDocument.BinaryFileId;
            }

            rbStatus.SelectedValue = signatureDocument.Status.ConvertToInt().ToString();
            hlStatusLastUpdated.Visible = signatureDocument.LastStatusDate.HasValue;
            hlStatusLastUpdated.Text = signatureDocument.LastStatusDate.HasValue ?
                string.Format( "<span title='{0}'>Last Status Update: {1}</span>", signatureDocument.LastStatusDate.Value.ToString(), signatureDocument.LastStatusDate.Value.ToElapsedString() ) :
                string.Empty;

            lDocumentKey.Text = signatureDocument.DocumentKey;
            lDocumentKey.Visible = !string.IsNullOrWhiteSpace( signatureDocument.DocumentKey );

            lRequestDate.Visible = signatureDocument.LastInviteDate.HasValue;
            lRequestDate.Text = signatureDocument.LastInviteDate.HasValue ?
                string.Format( "<span title='{0}'>{1}</span>", signatureDocument.LastInviteDate.Value.ToString(), signatureDocument.LastInviteDate.Value.ToElapsedString() ) :
                string.Empty;
        }

        /// <summary>
        /// Sets the edit mode.
        /// </summary>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        private void SetEditMode( bool editable )
        {
            pnlEditDetails.Visible = editable;
            vsDetails.Enabled = editable;
            fieldsetViewDetails.Visible = !editable;

            this.HideSecondaryBlocks( editable );
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="signatureDocumentId">The signature document type identifier.</param>
        public void ShowDetail( int signatureDocumentId )
        {
            pnlDetails.Visible = true;
            SignatureDocument signatureDocument = null;

            using ( var rockContext = new RockContext() )
            {
                if ( !signatureDocumentId.Equals( 0 ) )
                {
                    signatureDocument = new SignatureDocumentService( rockContext ).Get( signatureDocumentId );
                }

                if ( signatureDocument == null )
                {
                    signatureDocument = new SignatureDocument { Id = 0 };

                    int? personId = PageParameter( PageParameterKey.PersonId ).AsIntegerOrNull();
                    if ( personId.HasValue )
                    {
                        var person = new PersonService( rockContext ).Get( personId.Value );
                        if ( person != null )
                        {
                            var personAlias = person.PrimaryAlias;
                            if ( personAlias != null )
                            {
                                signatureDocument.AppliesToPersonAlias = personAlias;
                                signatureDocument.AppliesToPersonAliasId = personAlias.Id;
                                signatureDocument.AssignedToPersonAlias = personAlias;
                                signatureDocument.AssignedToPersonAliasId = personAlias.Id;
                            }
                        }
                    }

                    int? documentTypeId = PageParameter( PageParameterKey.SignatureDocumentTemplateId ).AsIntegerOrNull();
                    if ( documentTypeId.HasValue )
                    {
                        var documentType = new SignatureDocumentTemplateService( rockContext ).Get( documentTypeId.Value );
                        if ( documentType != null )
                        {
                            signatureDocument.SignatureDocumentTemplate = documentType;
                            signatureDocument.SignatureDocumentTemplateId = documentType.Id;
                        }
                    }
                }

                hfSignatureDocumentId.SetValue( signatureDocument.Id );
                hfSignatureTemplateId.SetValue( signatureDocument.SignatureDocumentTemplateId );
                bool readOnly = false;
                nbEditModeMessage.Text = string.Empty;
                bool canEdit = UserCanEdit || signatureDocument.IsAuthorized( Authorization.EDIT, CurrentPerson );
                bool canView = canEdit || signatureDocument.IsAuthorized( Authorization.VIEW, CurrentPerson );
                if ( !canEdit )
                {
                    readOnly = true;
                    nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( MediaAccount.FriendlyTypeName );
                }

                if ( !canView )
                {
                    pnlDetails.Visible = false;
                }
                else
                {
                    pnlDetails.Visible = true;
                    if ( readOnly )
                    {
                        btnEdit.Visible = false;
                        ShowReadonlyDetails( signatureDocument );
                    }
                    else
                    {
                        btnEdit.Visible = false;
                        if ( signatureDocument.Id > 0 )
                        {
                            if ( signatureDocument.SignatureDocumentTemplate.ProviderEntityTypeId.HasValue
                                || signatureDocument.SignatureDocumentTemplate.ProviderTemplateKey.IsNotNullOrWhiteSpace() )
                            {
                                btnEdit.Visible = true;
                            }

                            ShowReadonlyDetails( signatureDocument );
                        }
                        else
                        {
                            ShowEditDetails( signatureDocument, false );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns to parent.
        /// </summary>
        private void ReturnToParent()
        {
            var qryParams = new Dictionary<string, string>();
            int? personId = PageParameter( "PersonId" ).AsIntegerOrNull();
            if ( personId.HasValue )
            {
                qryParams.Add( "PersonId", personId.Value.ToString() );
            }
            else
            {
                qryParams.Add( "SignatureDocumentTemplateId", PageParameter( "SignatureDocumentTemplateId" ) );
            }

            NavigateToParentPage( qryParams );
        }

        /// <summary>
        /// Routes any actions
        /// </summary>
        private void RouteAction()
        {
            if ( this.Page.Request.Form["__EVENTARGUMENT"] != null )
            {
                string[] eventArgs = this.Page.Request.Form["__EVENTARGUMENT"].Split( '^' );

                if ( eventArgs.Length == 2 )
                {
                    string action = eventArgs[0];
                    string parameters = eventArgs[1];
                    int? signatureDocumentId;

                    switch ( action )
                    {
                        case "Send":
                            signatureDocumentId = parameters.AsIntegerOrNull();
                            if ( signatureDocumentId.HasValue )
                            {
                                SendInvite( signatureDocumentId.Value );
                            }

                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Send an Invite
        /// </summary>
        private void SendInvite( int? signatureDocumentId )
        {
            if ( signatureDocumentId.HasValue && signatureDocumentId.Value > 0 )
            {
                using ( var rockContext = new RockContext() )
                {
                    var signatureDocument = new SignatureDocumentService( rockContext ).Get( signatureDocumentId.Value );
                    if ( signatureDocument != null )
                    {
                        var errorMessages = new List<string>();
                        if ( new SignatureDocumentTemplateService( rockContext ).SendDocument( signatureDocument, string.Empty, out errorMessages ) )
                        {
                            rockContext.SaveChanges();

                            nbErrorMessage.Title = string.Empty;
                            nbErrorMessage.Text = "Signature Invite Was Successfully Sent";
                            nbErrorMessage.NotificationBoxType = NotificationBoxType.Success;
                            nbErrorMessage.Visible = true;
                        }
                        else
                        {
                            nbErrorMessage.Title = "Error Sending Signature Invite";
                            nbErrorMessage.Text = string.Format( "<ul><li>{0}</li></ul>", errorMessages.AsDelimited( "</li><li>" ) );
                            nbErrorMessage.NotificationBoxType = NotificationBoxType.Danger;
                            nbErrorMessage.Visible = true;
                        }
                    }
                }
            }
        }

        #endregion

    }
}