<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Form.aspx.cs" Inherits="FileUpload.Form" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="js/jquery.json-2.4.min.js" type="text/javascript"></script>
    <script src="js/form.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(
        function () {
            $.achiveForm("[data-form=form]",
                function (data) {
                    alert($.toJSON(data));
                    $.post("Form.aspx", data, function (html) { $("#parse").html(html); });
                },
                function (data) {
                    return true;
                });
        }
    );
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div data-form="form">
        <input type="text" name="name" value="" />
        <input type="text" name="password" value="" />
        <div data-form="listcmdbar" list-name="records">
            <input type="button" value="增加" />
            <input type="button" value="删除选中" />
        </div>
        <table border="0" cellpadding="0" cellspacing="0" data-form="list" list-name="records">
            <tr>
                <td>
                名称
                </td>
                <td>
                花费
                </td>
            </tr>
            <tr data-form="row">
                <td>
                    <input type="text" name="records.name" value="" />
                </td>
                <td>
                    <input type="text" name="records.cost" value="" />
                </td>
                <td>
                    <div data-form="list" list-name="records.selects">
                        <div data-form="row">
                            <input type="text" name="records.selects.name" value="" />
                        </div>
                        <div data-form="row">
                            <input type="text" name="records.selects.name" value="" />
                        </div>
                    </div>
                </td>
            </tr>
            <tr data-form="row">
                <td>
                    <input type="text" name="records.name" value="" />
                </td>
                <td>
                    <input type="text" name="records.cost" value="" />
                </td>
                <td>
                    <div data-form="subform" subform-name="records.address">
                        <input type="text" name="records.address.x" value="" />
                        <input type="text" name="records.address.y" value="" />
                    </div>
                </td>
            </tr>
        </table>
        <asp:Button Text="提交" runat="server" />
        <input type="button" value="获取form-data" data-form="ok" />
        <input type="reset" value="" />
    </div>
    <hr />
    <div ID="parse" ></div>
    </form>
</body>
</html>
