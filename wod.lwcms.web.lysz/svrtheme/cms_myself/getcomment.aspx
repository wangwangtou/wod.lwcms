<%@ Page Language="C#" AutoEventWireup="true"%>
<%@ Register Src="~/svrtheme/shared/pager.ascx" TagName="pager" TagPrefix="s" %>
<% List<comment> cmtList = PD.op.getObject<List<comment>>("cmtList"); %>

<ul class="m-commentlist">
<%foreach (comment c in cmtList)
  {
%><li>
<div class="pd-5 bg-f4"><%=EcHtml(c.userName)%> &nbsp; <%=c.commentTime.ToString("yyyy-MM-dd HH:mm")%></div>
<div class="pd-10 bg-f3"><%=EcHtml(c.commentContent) %></div>
</li><%      
  } %>
</ul>
<s:pager runat="server" />