﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Success.ascx.cs" Inherits="RockWeb.Blocks.CheckIn.Success" %>
<asp:UpdatePanel ID="upContent" runat="server">
<ContentTemplate>

    <Rock:ModalAlert ID="maWarning" runat="server" />

    <div class="checkin-header text-center">
        <h1><asp:Literal ID="lTitle" runat="server" /></h1>
    </div>

    <div class="checkin-body clearfix text-center">
        <div class="checkin-scroll-panel">
            <div class="scroller">
                <asp:Literal ID="lCheckinResultsHtml" runat="server" />
                <asp:Literal ID="lCheckinQRCodeHtml" runat="server" />
            </div>
        </div>
    </div>

    <div class="checkin-footer text-center clearfix">   
        <div class="checkin-actions text-center">
            <asp:LinkButton CssClass="btn btn-primary btn-block" ID="lbDone" runat="server" OnClick="lbDone_Click" Text="Done" />
            <asp:LinkButton CssClass="btn btn-default" ID="lbAnother" runat="server" OnClick="lbAnother_Click" Text="Another Person" />
         </div>
    </div>
        

</ContentTemplate>
</asp:UpdatePanel>
