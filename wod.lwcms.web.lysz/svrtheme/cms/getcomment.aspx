<%@ Page Language="C#" AutoEventWireup="true"%>
<%@ Register Src="~/svrtheme/shared/pager.ascx" TagName="pager" TagPrefix="s" %>
<% List<comment> cmtList = PD.op.getObject<List<comment>>("cmtList");
   if (cmtList.Count == 0)
   { 
     %><h4>暂无内容</h4><%
   }
   %>

<%foreach (comment c in cmtList)
  {
%>
<h5><%=EcHtml(c.userName)%> &nbsp; <%=c.commentTime.ToString("yyyy-MM-dd HH:mm")%></h5>
<p><%=EcHtml(c.commentContent) %></p>
<%      
  } %>
<s:pager runat="server" />