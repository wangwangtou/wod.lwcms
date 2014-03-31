<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/svrtheme/default/site.master" %>
<%@ Register Src="~/svrtheme/shared/pager.ascx" TagName="pager" TagPrefix="s" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="maincontent">
<% int pageIndex = PD.op.getObject<int>("pageIndex");
   int pageCount = PD.op.getObject<int>("pageCount");
   if (pageIndex == 0)
   {%>
<div class="aglin-center pd-v20">
    <div class="inline-block bg-f2 module pd-5 link-f2">
        <h3>程序设计</h3>
        <p>已经靠他吃了五年饭了，一个游戏爱好者成为了一个程序员，现在偶尔仍会玩会dota。</p>
        <p>现在希望做一点自己的事情，所以开了此站，祝贺一下我终于迈出这一步。目前会兼职做一些网站或程序，如果您有需要，可以查看我的<a href="index.aspx?path=/default/my-skill.html">相关技能</a>，希望能够为你服务。</p>
     </div>
    <div class="inline-block bg-f3 module pd-5 link-f1">
        <h3>炒小菜</h3>
        <p>基本的生活技能，虽说不是山珍海味，但是却吃不厌。湖北东北部清淡系！如果你想吃，可以Email我-.-。</p>
        <p>看看我曾经超过的菜吧：《<a href="index.aspx?path=/default/cookbook">我的菜单</a>》。</p>
    </div>
    <div class="inline-block bg-h2 module pd-5 link-main">
        <h3>经济学</h3>
        <p>突然冒出来的学习计划，正在进行中。学习文章将会在此处更新：《<a href="index.aspx?path=/mer">学习日志</a>》。</p>
        <p>正在看的书是：《宏观经济学》。</p>
    </div>
    <div class="clear"></div>
</div>
<%} %>
<h2 class="pd-5 bg-f1">文章列表</h2>
<%List<article> al = PD.op.getObject<List<article>>("articleList"); %>
<ul class="m-artlist link-h1">
<%foreach (article a in al)
  {
%><li>
<h3 class="pd-5"><a href="/index.aspx?path=<%=a.category.fullpath + "/" + a.code + ".html" %>"><%=a.name%></a></h3>
<div class="pd-10"><%=a.content %></div>
</li><%      
  } %>
</ul>
<%if (pageIndex == 0)
  {
      if (pageCount > 1)
      {
%><a href="index.aspx?pageIndex=2">更多</a><%      
      }
  }
  else
  {%>
<s:pager runat="server"/>
<%} %>
</asp:Content>
