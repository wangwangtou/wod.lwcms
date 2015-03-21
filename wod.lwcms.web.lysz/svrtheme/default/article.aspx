<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/svrtheme/default/site.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="maincontent">
<div class="bg-main pd-20">
    <h1><%=PD.art.name %></h1>
    <span><%=PD.art.createOn.ToString("yyyy-MM-dd HH:mm:ss") %></span>
    <div class="m-doc"><%=PD.art.content %></div>
</div>
<script type="text/javascript">
    var __aid = "<%=PD.art.id %>";
    setTimeout(function () {
        lwcms.viewArticle(__aid);
    }, 1000 * 30);
</script>
<div class="bg-f1 pd-20">
    <h1>评论列表</h1>
    <div id="comments"></div>
    <script type="text/javascript">
        lwcms.getComments(__aid, "#comments");
    </script>
</div>
<div class="bg-f4 pd-20">
    <u:commentadd runat="server" />
</div>
</asp:Content>
