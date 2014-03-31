<%@ Control Language="C#" AutoEventWireup="true" %>
<h1>
用户名：<%=TD.getStr("name") %>
</h1>
<h1>
密码：<%=TD.getStr("password")%>
</h1>
<%=TD.getList("records")[0].getList("selects")[0].getStr("name") %><br />
<%=TD.getList("records")[0].getList("selects")[1].getStr("name") %><br />
<%=TD.getList("records")[0].getStr("name") %><br />
<%=TD.getList("records")[1].getStr("name") %><br />
<%=TD.getStr("records.1.name")%><br />
<%=TD.getStr("records.1.address.x")%><br />