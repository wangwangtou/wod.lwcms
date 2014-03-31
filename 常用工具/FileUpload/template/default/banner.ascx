<%@ Control Language="C#" AutoEventWireup="true" %>
<h1>
地名：<%=TD.getStr("title") %>
</h1>
<ul>
<% foreach (var item in TD.getList("shengfen"))
   {
%><li><a href="#"><%=item.getStr("name")%></a>
    <ul>
<% foreach (var sitem in item.getList("shiqu"))
   {
%><li><%=sitem.getStr("")%></li>
<%} %>
    </ul>
    
    </li><%       
   } %>
</ul>
