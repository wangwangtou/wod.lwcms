<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/svrtheme/cms/site.master" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="maincontent">
<div class="comment_form">
    <form onsubmit="return commetsubmit(this);">
        <div class="form_settings">
          <p><span>姓名：</span><input type="text" name="name" value="" /></p>
          <p><span>邮箱：</span><input type="text" name="email" value="" /></p>
          <p><span>留言：</span><textarea name="comment"></textarea></p>
          <p><span>&nbsp;</span><input type="submit" class="submit" value="发表" /></p>
        </div>
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
          alert("留言内容不能少于10个字！");
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
                    lwcms.getComments(__aid, "#comments",50);
                  }
                });
        return false;
      }
    </script>
</div>
<script type="text/javascript">
  var __aid = "message_board";
</script>
    <h1>留言板</h1>
    <div id="comments"></div>
    <script type="text/javascript">
      lwcms.getComments(__aid, "#comments");
    </script>

</asp:Content>
