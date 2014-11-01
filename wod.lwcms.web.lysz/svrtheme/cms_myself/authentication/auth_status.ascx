<%@ Control Language="C#" AutoEventWireup="true" %>
<% bool islogin = PD.op.getObject<bool>("auth_islogin");
   string name = PD.op.getObject<string>("auth_uname");

   if (!islogin)
   {
     %><a href="/index.aspx?path=/common/authentication/login">登录</a><a href="/index.aspx?path=/common/authentication/regist">注册</a><%
   }
   else
   { 
     %><a title="进入用户中心" href="/index.aspx?path=/common/authentication/userinfo"><%=name %></a><a href="/index.aspx?path=/common/authentication/logout">退出</a><%
   }
   %>