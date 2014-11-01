<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/svrtheme/cms/site.master" %>
<%@ Register Src="~/svrtheme/shared/pager.ascx" TagName="pager" TagPrefix="s" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="maincontent">

<div class="aglin-center pd-v20">
    <div class="inline-block bg-f2 module pd-5 link-f2">
        <h3>内容模版</h3>
        <p></p>
        <p></p>
     </div>
    <div class="inline-block bg-f3 module pd-5 link-f1">
        <h3>样式模版</h3>
        <p></p>
        <p></p>
    </div>
    <div class="inline-block bg-h2 module pd-5 link-main">
        <h3>命令模式</h3>
        <p></p>
        <p></p>
    </div>
    <div class="clear"></div>
</div>

<div class="aglin-center pd-v20">
    <div class="inline-block bg-f4 module pd-5 link-f2">
        <h3>extensions</h3>
        <p><ol><li>authentication</li><li>cacheable</li></ol></p>
        <p></p>
     </div>
    <div class="inline-block bg-f1 module pd-5 link-f1">
        <h3>plugins support</h3>
        <p></p>
        <p></p>
    </div>
    <div class="inline-block bg-h2 module pd-5 link-main">
        <h3>advanced editor</h3>
        <p></p>
        <p></p>
    </div>
    <div class="clear"></div>
</div>
<% int pageIndex = PD.op.getObject<int>("pageIndex");
   int pageCount = PD.op.getObject<int>("pageCount");
   if (pageIndex == 0)
   {%>
<%} %>
</asp:Content>
