<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/svrtheme/lysz/site.master" %>
<%@ Register Src="~/svrtheme/shared/pager.ascx" TagName="pager" TagPrefix="s" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="maincontent">
<%List<article> al = PD.op.getObject<List<article>>("articleList"); %>

<h2 class="pd-5 bg-h2"><%=PD.cat.name %></h2>
<div class="pd-5 bg-h1"><%=PD.cat.content %></div>
<ul class="m-artlist link-h1">
<%foreach (article a in al)
  {
%><li>
<h3 class="pd-5"><a href="/index.aspx?path=<%=a.category.fullpath + "/" + a.code + ".html" %>"><%=a.name%></a></h3>
<div class="pd-10 m-doc"><%=a.content %></div>
</li><%      
  } %>
</ul>
<s:pager ID="Pager1" runat="server" />
</asp:Content>
