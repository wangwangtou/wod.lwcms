<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/svrtheme/default/site.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="maincontent">
<div class="bg-main pd-20">
    <h1><%=PD.art.name %></h1>
    <span><%=PD.art.createOn.ToString("yyyy-MM-dd HH:mm:ss") %></span>
    <div class="m-doc"><%=PD.art.content %></div>
</div>
<script type="text/javascript">
    var __aid = "<%=PD.art.id %>";
    setTimeout(function () {
        lwcms.viewArticle(__aid);
    }, 1000 * 30);
</script>
<div class="bg-f1 pd-20">
    <h1>评论列表</h1>
    <div id="comments"></div>
    <script type="text/javascript">
        lwcms.getComments(__aid, "#comments");
    </script>
</div>
<div class="bg-f4 pd-20">
    <h1>发表评论</h1>
    <form class="m-cmtform" onsubmit="return commetsubmit(this);">
        <label>姓名：<br /><input type="text" name="name" value="" /></label><br />
        <label>邮箱：<br /><input type="text" name="email" value="" /></label><br />
        <label>评论：<br /><textarea name="comment"></textarea></label><br />
        <input type="submit" value="发表评论" />
    </form>
    <script type="text/javascript">
        function commetsubmit(form) {
            if (!form.name.value) {
                alert("姓名不能为空！");
                return false;
            }
            if (!form.email.value) {
                alert("邮箱不能为空！");
                return false;
            }
            if (!form.comment.value || form.comment.value.length <= 10) {
                alert("评论内容不能少于10个字！");
                return false;
            }
            $(form).attr("disabled", true);
            lwcms.commentArticle(__aid, { name: form.name.value, email: form.email.value, content: form.comment.value },
                function (data) {
                    $(form).attr("disabled", false);
                    if (data && data.result && !data.result.IsValid) {
                        alert(data.result.Messages[0].message);
                    }
                    else {
                        $(form).find("input[type='text'],textarea").val("");
                        lwcms.getComments(__aid, "#comments");
                    }
                });
            return false;
        }
    </script>
</div>
</asp:Content>
