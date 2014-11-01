<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/svrtheme/cms/site.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="maincontent">
<div class="bg-f4 pd-20">
    <h1>登录</h1>
    <form class="m-form" onsubmit="return commetsubmit(this);">
        <label>帐号：<br /><input type="text" name="account" value="" /></label><br />
        <label>密码：<br /><input type="password" name="password" value="" /></label><br />
        <label>验证码：<img onclick="this.src='/index.aspx?path=/common/vercode&_r='+Math.random()" src="/index.aspx?path=/common/vercode&_r=<%=Guid.NewGuid().ToString() %>" alt="验证码" title="点击更换验证码" /><br /><input type="text" name="vercode" value="" /></label><br />
        <input type="submit" value="登录" />
    </form>
    <script type="text/javascript">
      function commetsubmit(form) {
        if (!form.account.value) {
          alert("帐号不能为空！");
          return false;
        }
        if (!form.password.value) {
          alert("密码不能为空！");
          return false;
        }
        if (!form.vercode.value) {
          alert("验证码不能为空！");
          return false;
        }
        $(form).attr("disabled", true);
        lwcms.login({ account: form.account.value
                , password: form.password.value, vercode: form.vercode.value
        },
                function (data) {
                  $(form).attr("disabled", false);
                  if (data && data.result && !data.result.IsValid) {
                    alert(data.result.Messages[0].message);
                    $(form).find("img").click();
                  }
                  else {
                    alert("登录成功！");
                    location.href = "/index.aspx";
                  }
                });
        return false;
      }
    </script>
</div>
</asp:Content>
