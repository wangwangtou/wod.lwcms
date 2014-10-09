<%@ Page Language="C#" AutoEventWireup="true" %>

<script runat="server">
    public override string loadCmd
    {
        get { return ""; }
        set { }
    }

    public override string ajaxCmd
    {
        get{return "login";}set{}
    }

    protected override void PreCommand()
    {
        base.ServerParams.Add("isRemember", false);
        base.PreCommand();
    }
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="scripts/jquery-1.10.2.js" type="text/javascript"></script>
</head>
<body>
    <div data-form="form">
        <label>登录名：<input type="text" name="account" value="xvycn" /></label>
        <label>密码：<input type="password" name="password" value="a123456" /></label>
        <input type="button" value="登录" id="btnLogin">
    </div>
    <script type="text/javascript">
        $(function () {
            $("#btnLogin").click(
                function () {
                    var name = $("[name='account']").val();
                    var password = $("[name='password']").val();
                    var data = { account: name, password: password };
                    if (data.account && data.password) {
                        $.post("login.aspx", data, function (result) {
                            if (result.status) {
                                if (result.result) {
                                    alert(result.result);
                                }
                                else {
                                    location = "index.aspx";
                                }
                            }
                            else {
                                alert(result.message);
                            }
                        });
                    }
                    else {
                        alert("请输入用户名和密码");
                        return false;
                    }
                }
            );
        });
    </script>
</body>
</html>
