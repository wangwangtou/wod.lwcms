<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/svrtheme/default/site.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="maincontent">
<%List<article> al = PD.op.getObject<List<article>>("articleList"); %>
<ul class="c-al">
<%foreach (article a in al)
  {
%><li><a href="/index.aspx?path=<%=a.category.fullpath + "/" + a.code + ".html" %>"><%=a.name %></a></li><%      
  } %>
</ul>
<p class="c-pg"></p>
</asp:Content>
