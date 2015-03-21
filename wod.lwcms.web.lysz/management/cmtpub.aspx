<%@ Page Language="C#" AutoEventWireup="true" 
MasterPageFile="~/management/managepage.master"%>
<%@ Register Src="~/management/shared/pager.ascx" TagName="pager" TagPrefix="s" %>

<script runat="server" type="text/C#">
    public override string ajaxCmd { get { 
        return Request.QueryString["act"] == "pub" ? "cmtpublish"
            : (Request.QueryString["act"] == "unpub" ? "cmtunpublish"
                : "cmtdelete"); 
    } set { } }

    public override string loadCmd
    {
        get { return "cmtlist"; }
        set { } 
    }
   
</script>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function _delete(id) {
            if (confirm("确定要删除该内容？")) {
                $.ajax({ url: "cmtpub.aspx?act=delete", data: { cid: id }, type: "post", success: function (data) {
                    location.reload();
                }, error: function () { alert("删除失败！"); location.reload(); }
                });
            }
        }
        function _publish(id) {
            $.ajax({ url: "cmtpub.aspx?act=pub", data: { cid: id }, type: "post", success: function (data) {
                location.reload();
            }, error: function () { alert("发布成功！"); location.reload(); }
            });
        }
        function _unpublish(id) {
            $.ajax({ url: "cmtpub.aspx?act=unpub", data: { cid: id }, type: "post", success: function (data) {
                location.reload();
            }, error: function () { alert("取消发布成功！"); location.reload(); }
            });
        }
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%%>
<div class="g-left g-left-thin">
<fieldset>
    <legend>文章内容</legend>
<ul class="c-al">
<li><a href="artlist.aspx">全部</a></li>
</ul>
</fieldset>
</div>

<%List<comment> cmtList = PD.getObject<List<comment>>("cmtList");
  bool alt = false; %>
<div class="g-center">
<ul class="f-list">
<%foreach (comment c in cmtList)
  {
%><li>
<div class="f-list-item<%=(alt=!alt)?"":" alt"%>">
    <div></div>
    <p><%=Server.HtmlEncode(c.commentContent) %></p>
    <div class="f-cmd"><%=c.state == "pub" ? "<a href=\"javascript:_unpublish('" + c.id + "');\">取消发布</a>" : "<a href=\"javascript:_publish('" + c.id + "');\">发布</a>"%><a href="javascript:_delete('<%=c.id %>');">删除</a></div>
</div></li><%      
  } %>
</ul>
<s:pager runat="server" />
</div>
</asp:Content>