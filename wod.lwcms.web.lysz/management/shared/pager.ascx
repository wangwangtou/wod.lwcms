<%@ Control Language="C#" AutoEventWireup="true" %>
<%  int totalCount = PD.getObject<int>("totalCount");
    int pageSize = PD.getObject<int>("pageSize");
    int pageIndex = PD.getObject<int>("pageIndex");
    int pageCount = Math.Max(1, (int)Math.Ceiling(totalCount / (decimal)pageSize)); 
    int startRowIndex = PD.getObject<int>("startRowIndex"); 
    int endRowIndex = PD.getObject<int>("endRowIndex");

    string url = common.GetRemovedParamUrl(Request.RawUrl,"pageIndex");
    url = url + (url.IndexOf("?") > 0 ? "&" : "?");
    
    string nolink = "javascript:;";
    
    pageIndex = pageIndex + 1;
    int start = pageIndex - 3;
    int end = pageIndex + 3;
    if (pageIndex < 8)
    {
        start = 1;
        end = 8;
    }
    if (pageIndex > pageCount - 9)
    {
        start = pageCount - 9;
        end = pageCount;
    }
    start = Math.Max(1, start);
    end = Math.Min(pageCount, end);
       %>
<div class="m-page m-page-rt">
    <a href="<%=pageIndex == 1 ? nolink : url +"pageIndex=" + (pageIndex-1) %>" class="pageprv<%=pageIndex == 1 ? " z-dis" : ""%>">上一页</a>
    <a href="<%=url+"pageIndex=1" %>"<%=pageIndex==1?" class=\"z-crt\"":"" %>>1</a>
    <%if (start >= 3)
      { %><i>...</i> <% }
      else { start = 2; } %>
    <% for (int i = start; i <= end; i++)
       {%>
    <a href="<%=url +"pageIndex=" + i %>"<%=pageIndex==i?" class=\"z-crt\"":"" %>><%=i %></a>     
       <%} %>
    <%if (end <= pageCount - 2)
      { %><i>...</i><a href="<%=url +"pageIndex=" + pageCount %>"<%=pageIndex==pageCount?" class=\"z-crt\"":"" %>><%=pageCount%></a> <% }
      else if(end == pageCount - 1){ %>
    <a href="<%=url +"pageIndex=" + pageCount %><%=pageIndex==pageCount?" class=\"z-crt\"":"" %>"><%=pageCount%></a>
      <%  } %>
    <a href="<%=pageIndex == pageCount ? nolink : url +"pageIndex=" + (pageIndex+1) %>" class="pagenxt<%=pageIndex == pageCount ? " z-dis" : ""%>">下一页</a>
</div>
<%--<p><%=common.GetRemovedParamUrl("a.aspx?pageSize=1", "pageSize")%></p>
<p><%=common.GetRemovedParamUrl("a.aspx?p1=2&pageSize=1", "pageSize") %></p>
<p><%=common.GetRemovedParamUrl("a.aspx?pageSize=1&p2=3", "pageSize") %></p>
<p><%=common.GetRemovedParamUrl("a.aspx?p1=2&pageSize=1&p2=3", "pageSize")%></p>
<p><%=common.GetRemovedParamUrl("a.aspx?p1=2&p2=3", "pageSize")%></p>--%>
