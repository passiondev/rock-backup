<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ABWETests.ascx.cs" Inherits="RockWeb.Blocks.Finance.ABWETests" %>

<asp:UpdatePanel ID="pnlABWEUpdatePanel" runat="server">
    <ContentTemplate>
        <h1>Account Picker</h1>
        <Rock:AccountPicker ID="apAccount" runat="server" EnhanceForLongLists="true" DisplayChildItemCountLabel="true" />
        <br />
         <h1>Account Picker (Multi Select)</h1>
        <Rock:AccountPicker ID="AccountPicker1" runat="server" EnhanceForLongLists="true" DisplayChildItemCountLabel="true" AllowMultiSelect="true" />
        <br />
        <h1>Group Picker</h1>
        <Rock:GroupPicker ID="gpGroups" runat="server" Required="true" EnhanceForLongLists="true" DisplayChildItemCountLabel="true" />
        <br />
        <%--<h1>Entity Type Picker</h1>
        <Rock:EntityTypePicker ID="epEntityType" runat="server" Required="true" Label="Entity Type" IncludeGlobalOption="false" EnhanceForLongLists="true" />
        <br />--%>
    </ContentTemplate>
</asp:UpdatePanel>
