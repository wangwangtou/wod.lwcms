<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/svrtheme/default/site.master" %>
<%@ Register Src="~/svrtheme/shared/pager.ascx" TagName="pager" TagPrefix="s" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="maincontent">
<%List<article> al = PD.op.getObject<List<article>>("articleList"); %>

<h2 class="pd-5 bg-f2"><%=PD.cat.name %></h2>
<p class="pd-5 bg-f3"><%=PD.cat.content %></p>
<ul class="m-artlist link-h1">
<%foreach (article a in al)
  {
%><li>
<h3 class="pd-5"><a href="/index.aspx?path=<%=a.category.fullpath + "/" + a.code + ".html" %>"><%=a.name%></a></h3>
<div class="pd-10 m-doc"><%=a.content %></div>
</li><%      
  } %>
</ul>
<s:pager runat="server" />
</asp:Content>
