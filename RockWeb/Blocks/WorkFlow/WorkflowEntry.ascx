<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WorkflowEntry.ascx.cs" Inherits="RockWeb.Blocks.WorkFlow.WorkflowEntry" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <style>
            .e-signature-pad {
                border-bottom: 2px solid magenta
            }
        </style>

        <div class="row">
            <div id="divForm" runat="server" class="col-md-6">
                <div class="panel panel-block">

                    <div class="panel-heading">
                        <h1 class="panel-title">
                            <asp:Literal ID="lIconHtml" runat="server"><i class="fa fa-gear"></i></asp:Literal>
                            <asp:Literal ID="lTitle" runat="server">Workflow Entry</asp:Literal>
                        </h1>
                        <div class="panel-labels">
                            <Rock:HighlightLabel ID="hlblWorkflowId" runat="server" LabelType="Info" />
                            <Rock:HighlightLabel ID="hlblDateAdded" runat="server" LabelType="Default" />
                        </div>
                    </div>
                    <div class="panel-body">

                        <asp:Literal ID="lSummary" runat="server" Visible="false" />

                        <asp:Panel ID="pnlWorkflowUserForm" CssClass="workflow-entry-panel" runat="server">

                            <asp:ValidationSummary ID="vsDetails" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" />

                            <asp:Literal ID="lFormHeaderText" runat="server" />

                            <%-- Person Entry --%>
                            <asp:Panel ID="pnlPersonEntry" runat="server">
                                <asp:Literal ID="lPersonEntryPreHtml" runat="server" />

                                <div class="row">
                                    <div class="col-md-6">
                                        <Rock:CampusPicker ID="cpPersonEntryCampus" runat="server" Required="true" />
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>

                                <%-- Special input with rock-fullname class --%>
                                <Rock:RockTextBox ID="tbRockFullName" runat="server" CssClass="rock-fullname" ValidationGroup="vgRockFullName" Placeholder="Please enter name (Required)" />
                                <Rock:NotificationBox ID="nbRockFullName" runat="server" NotificationBoxType="Validation" />

                                <asp:Panel ID="pnlPersonEntryRow1" runat="server" CssClass="row">
                                    <%-- Person 1 --%>
                                    <asp:Panel ID="pnlPersonEntryRow1Column1" runat="server" CssClass="col-md-6">
                                        <Rock:PersonBasicEditor ID="pePerson1" runat="server" />
                                    </asp:Panel>

                                    <%-- Person 2 (Spouse) --%>
                                    <asp:Panel ID="pnlPersonEntryRow1Column2" runat="server" CssClass="col-md-6">
                                        <Rock:PersonBasicEditor ID="pePerson2" runat="server" />
                                    </asp:Panel>
                                </asp:Panel>

                                <Rock:RockCheckBox ID="cbShowPerson2" runat="server" Text="Show Person2" Checked="false" AutoPostBack="true" OnCheckedChanged="cbShowPerson2_CheckedChanged" />

                                <%-- Person Entry Address and Marital Status --%>
                                <asp:Panel ID="pnlPersonEntryRow2" CssClass="row" runat="server">
                                    <asp:Panel ID="pnlPersonEntryRow2Column1" runat="server" CssClass="col-md-6">
                                        <Rock:AddressControl ID="acPersonEntryAddress" runat="server" Label="Address" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlPersonEntryRow2Column2" runat="server" CssClass="col-md-6">
                                        <Rock:DefinedValuePicker runat="server" ID="dvpMaritalStatus" Label="Marital Status" />
                                    </asp:Panel>
                                </asp:Panel>

                                <asp:Literal ID="lPersonEntryPostHtml" runat="server" />
                            </asp:Panel>

                            <%-- Workflow Form Attribute Controls  --%>
                            <asp:PlaceHolder ID="phWorkflowFormAttributes" runat="server" />

                            <asp:Literal ID="lFormFooterText" runat="server" />

                            <div class="actions">
                                <asp:PlaceHolder ID="phActions" runat="server" />
                            </div>

                        </asp:Panel>

                        <%-- eSignature UI --%>
                        <asp:Panel ID="pnlWorkflowActionElectronicSignature" runat="server" Visible="false">
                            <Rock:HiddenFieldWithClass ID="hfSignatureImageDataUrl" CssClass="js-signature-data" runat="server" />
                            <a id="signature_waiver"></a>
                            <asp:Literal ID="lSignatureDocumentHTML" runat="server" />
                            <asp:Panel ID="pnlSignatureEntryDrawn" runat="server" CssClass="signature-entry-drawn js-signature-entry-drawn">
                                <canvas class="js-signature-pad-canvas e-signature-pad"></canvas>
                            </asp:Panel>
                            <asp:Panel ID="pnlSignatureEntryTyped" runat="server" CssClass="signature-entry-typed">
                            </asp:Panel>
                            <asp:Literal ID="lSignatureSignDisclaimer" runat="server">
                                By clicking the sign button below, I agree to the <a href='#signature_waiver'>waiver</a> above and understand this is a legal representation of my signature.
                            </asp:Literal>
                            <div class="alert alert-danger js-signature-empty-alert" style="display: none">
                                Please enter a signature
                            </div>
                            <asp:LinkButton ID="btnSignSignature" runat="server" CssClass="btn btn-default js-save-signature" OnClick="btnSignSignature_Click">Sign</asp:LinkButton>
                            <a id="btnClearSignature" class="btn btn-default js-clear-signature">Clear</a>
                        </asp:Panel>



                        <%-- This needs a 'js-workflow-entry-message-notification-box' javascript hook so that Rock.Workflow.Action.ShowHtml can find it.--%>
                        <Rock:NotificationBox ID="nbMessage" runat="server" Dismissable="true" CssClass="margin-t-lg js-workflow-entry-message-notification-box" />
                    </div>

                </div>

            </div>

            <div id="divWorkflowActionUserFormNotes" runat="server" class="col-md-6">
                <Rock:NoteContainer ID="ncWorkflowNotes" runat="server" NoteLabel="Note"
                    ShowHeading="true" Title="Notes" TitleIconCssClass="fa fa-comment"
                    DisplayType="Full" UsePersonIcon="false" ShowAlertCheckBox="true"
                    ShowPrivateCheckBox="false" ShowSecurityButton="false"
                    AllowAnonymousEntry="false" AddAlwaysVisible="false"
                    SortDirection="Descending" />
            </div>

        </div>

        <script>
            function handleWorkflowActionButtonClick (validationGroup, causesValidation) {
                if (causesValidation) {
                    // make sure page is valid before doing the postback (from this button's href)
                    if (!Page_ClientValidate(validationGroup)) {
                        return false;
                    }
                }

                //$(this).button('loading');
                return true;
            }

            function resizeSignatureCanvas () {
                // If the window is resized, that'll affect the drawing canvas
                // also, if there is an existing signature, it'll get messed up, so clear it and
                // make them sign it again. See additional details why 
                // https://github.com/szimek/signature_pad
                var $signaturePadCanvas = $('.js-signature-pad-canvas');
                var signaturePadCanvas = $signaturePadCanvas[ 0 ];
                var containerWidth = $('.js-signature-entry-drawn').width();
                if (!containerWidth) {
                    containerWidth = 400;
                }

                const ratio = Math.max(window.devicePixelRatio || 1, 1);
                signaturePadCanvas.width = containerWidth * ratio;
                signaturePadCanvas.height = 100 * ratio;
                signaturePadCanvas.getContext("2d").scale(ratio, ratio);

                var signaturePad = $signaturePadCanvas.data('signatureComponent');
                signaturePad.clear();
            }

            function initializeSignaturePad () {
                var $signaturePadCanvas = $('.js-signature-pad-canvas');
                if (!$signaturePadCanvas.length) {
                    return
                }

                var signaturePad = new SignaturePad($signaturePadCanvas[ 0 ]);
                $signaturePadCanvas.data('signatureComponent', signaturePad);

                $('.js-clear-signature').click(function () {
                    signaturePad.clear();
                })

                $('.js-save-signature').click(function () {
                    if (signaturePad.isEmpty()) {

                        $('.js-signature-empty-alert').show();
                        return false;
                    }

                    var signatureImageDataUrl = signaturePad.toDataURL("image/jpeg");
                    $('.js-signature-data').val(signatureImageDataUrl);
                });

                window.addEventListener("resize", resizeSignatureCanvas);

                signaturePad.addEventListener("beginStroke", () => {
                    // if there was an error showing, hide the error if they start signing again
                    $('.js-signature-empty-alert').hide();
                }, { once: true });

                resizeSignatureCanvas();
            }

            Sys.Application.add_load(function () {
                initializeSignaturePad();
            });


        </script>

    </ContentTemplate>
</asp:UpdatePanel>
