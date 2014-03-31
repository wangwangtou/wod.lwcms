<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/svrtheme/default/site.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="maincontent">
<div class="bg-main pd-20">
    <h1><%=PD.art.name %></h1>
    <span><%=PD.art.createOn.ToString("yyyy-MM-dd HH:mm:ss") %></span>
    <div><%=PD.art.content %></div>
</div>
</asp:Content>
