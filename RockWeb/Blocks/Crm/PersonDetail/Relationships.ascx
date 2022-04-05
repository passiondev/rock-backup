﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Relationships.ascx.cs" Inherits="RockWeb.Blocks.Crm.PersonDetail.Relationships" %>
<asp:UpdatePanel ID="upRelationships" runat="server">
    <ContentTemplate>

        <div class="card card-profile overflow-hidden">
            <div class="card-header">
                <asp:Literal ID="lGroupName" runat="server"></asp:Literal>
                <asp:PlaceHolder ID="phEditActions" runat="server">
                    <div class="actions rollover-item pull-right">
                        <asp:LinkButton ID="lbAdd" runat="server" CssClass="edit" Text="Add Relationship" OnClick="lbAdd_Click" CausesValidation="false"><i class="fa fa-plus"></i></asp:LinkButton>
                    </div>
                </asp:PlaceHolder>
            </div>
            <div class="">
                <asp:Repeater ID="rGroupMembers" runat="server">
                    <ItemTemplate>
                        <dl class="m-0 px-3 py-3 d-flex justify-content-between text-sm font-medium">
                            <dt class="text-gray-900"><Rock:PersonLink runat="server"
                                    PersonId='<%# Eval("PersonId") %>'
                                    PersonName='<%# Eval("Person.FullName") %>' /></dt>
                            <dd class="text-gray-500"><%# ShowRole ? Eval("GroupRole.Name") : "" %><div class="control-actions pull-right">
                                    <asp:LinkButton ID="lbEdit" runat="server" CssClass="btn btn-sm btn-minimal px-1 edit" Text="Edit Relationship" Visible='<%# IsInverseRelationshipsOwner %>'
                                        CommandName="EditRole" CommandArgument='<%# Eval("Id") %>'><i class="fa fa-pencil"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lbRemove" runat="server" CssClass="btn btn-sm btn-minimal px-1 edit remove-relationship" Text="Remove Relationship" Visible='<%# IsInverseRelationshipsOwner %>'
                                        CommandName="RemoveRole" CommandArgument='<%# Eval("Id") %>'><i class="fa fa-times"></i></asp:LinkButton>
                                </div></dd>
                        </dl>
                    </ItemTemplate>
                </asp:Repeater>

            </div>
        </div>
        <section class="panel panel-persondetails">

            <div class="panel-heading rollover-container clearfix">
                <h3 class="panel-title pull-left"></h3>

            </div>

            <div class="panel-body">
                <asp:Literal ID="lAccessWarning" runat="server" />
            </div>

            <asp:HiddenField ID="hfRoleId" runat="server" />

            <Rock:ModalDialog ID="modalAddPerson" runat="server" Title="Add Relationship" ValidationGroup="NewRelationship">
                <Content>

                    <asp:ValidationSummary ID="valSummaryTop" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" ValidationGroup="NewRelationship" />

                    <div id="divExistingPerson" runat="server">
                        <fieldset>
                            <Rock:GroupRolePicker ID="grpRole" runat="server" Label="Relationship Type" ValidationGroup="NewRelationship" />
                            <Rock:PersonPicker ID="ppPerson" runat="server" Label="Person" Required="true" ValidationGroup="NewRelationship" />
                            <asp:Panel ID="pnlSelectedPerson" runat="server" />
                        </fieldset>
                    </div>

                </Content>
            </Rock:ModalDialog>

        </section>

    </ContentTemplate>
</asp:UpdatePanel>
