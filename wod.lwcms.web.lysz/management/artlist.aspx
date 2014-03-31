<%@ Page Language="C#" AutoEventWireup="true" 
MasterPageFile="~/management/managepage.master"%>
<%@ Register Src="~/management/shared/pager.ascx" TagName="pager" TagPrefix="s" %>

<script runat="server" type="text/C#">
    public override string ajaxCmd { get { 
        return Request.QueryString["act"] == "pub" ? "artpublish" 
            : (Request.QueryString["act"] == "unpub" ? "artunpublish" 
                :"artdelete"); 
    } set { } }

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
                $.ajax({ url: "artlist.aspx?act=delete", data: { artid: id }, type: "post", success: function (data) {
                    location.reload();
                }, error: function () { alert("删除失败！"); location.reload(); }
                });
            }
        }
        function _publish(id) {
            $.ajax({ url: "artlist.aspx?act=pub", data: { artid: id }, type: "post", success: function (data) {
                location.reload();
            }, error: function () { alert("发布成功！"); location.reload(); }
            });
        }
        function _unpublish(id) {
            $.ajax({ url: "artlist.aspx?act=unpub", data: { artid: id }, type: "post", success: function (data) {
                location.reload();
            }, error: function () { alert("取消发布成功！"); location.reload(); }
            });
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
%><li><a href="artlist.aspx?path=<%=c.fullpath %>"><%=c.name %></a><%   
      if (c.subCategory.Count > 0)
      {%><ul><% foreach (category cc in c.subCategory)
                {
                 %><li><a href="artlist.aspx?path=<%=cc.fullpath %>">--<%=cc.name%></a></li><%    
                } %></ul><% 
      }       %></li><%                                                        
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
    <div class="f-cmd"><%=a.state == "pub" ? "<a href=\"javascript:_unpublish('" + a.id + "');\">取消发布</a>" : "<a href=\"javascript:_publish('" + a.id + "');\">发布</a>"%><a href="artadd.aspx?id=<%=a.id %>">编辑</a><a href="javascript:_delete('<%=a.id %>');">删除</a></div>
</div></li><%      
  } %>
</ul>
<s:pager runat="server" />
</div>
</asp:Content>