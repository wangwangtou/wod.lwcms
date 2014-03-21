﻿<%@ Page Language="C#" AutoEventWireup="true" 
MasterPageFile="~/management/managepage.master"%>
<script runat="server" type="text/C#">
    public override string ajaxCmd { get { return "artdelete"; } set { } }

    protected override void PreLoadCommand()
    {
        string path = Request.QueryString["path"];
        if (string.IsNullOrEmpty(path))
        {
            path = "";
        }
        ServerParams.Add("catPath", path);
        base.PreCommand();
    }
</script>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function _delete(id) {
            if (confirm("确定要删除该内容？")) {
                $.ajax({ url: "artlist.aspx", data: { artid: id }, type: "post", success: function (data) {
                    location.reload();
                }, error: function () { alert("删除失败！"); location.reload(); }
                });
            }
        }
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%List<category> allCats = PD.getObject<List<category>>("allCats");%>
<div class="g-left g-left-thin">
<fieldset>
    <legend>选择分类</legend>
<ul class="c-al">
<li><a href="artlist.aspx">全部</a></li>
<%foreach (category c in allCats)
  {
%><li><a href="artlist.aspx?path=<%=c.fullpath %>"><%=c.name %></a></li><%      
  } %>
</ul>
</fieldset>
</div>

<%List<article> al = PD.getObject<List<article>>("articleList");
  bool alt = false; %>
<div class="g-center">
<ul class="f-list">
<%foreach (article a in al)
  {
%><li>
<div class="f-list-item<%=(alt=!alt)?"":" alt"%>"><%=a.name %>
    <div class="f-cmd"><a href="artadd.aspx?id=<%=a.id %>">编辑</a><a href="javascript:_delete('<%=a.id %>');">删除</a></div>
</div></li><%      
  } %>
</ul>
<p class="c-pg"></p>
</div>
</asp:Content>