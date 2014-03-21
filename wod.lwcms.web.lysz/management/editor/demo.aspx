<%@ Page Language="C#" AutoEventWireup="true" validateRequest="false" %>

<script runat="server">
protected void Page_Load(object sender, EventArgs e)
{
    this.Label1.Text = Request.Form["content1"];
}

</script>

<!doctype html>

<html>
<head runat="server">
    <meta charset="utf-8" />
    <title>KindEditor ASP.NET</title>
    <link rel="stylesheet" href="themes/default/default.css" />
	<link rel="stylesheet" href="plugins/code/prettify.css" />
	<script charset="utf-8" src="kindeditor.js"></script>
	<script charset="utf-8" src="lang/zh_CN.js"></script>
	<script charset="utf-8" src="plugins/code/prettify.js"></script>
	<script>
	    KindEditor.ready(function (K) {
	        var editor1 = K.create('#content1', {
	            cssPath: 'plugins/code/prettify.css',
	            uploadJson: '../editor/upload_json.ashx',
	            fileManagerJson: '../editor/file_manager_json.ashx',
	            allowFileManager: true,
	            afterCreate: function () {
	                var self = this;
	                K.ctrl(document, 13, function () {
	                    self.sync();
	                    K('form[name=example]')[0].submit();
	                });
	                K.ctrl(self.edit.doc, 13, function () {
	                    self.sync();
	                    K('form[name=example]')[0].submit();
	                });
	                window.selImage = function (text) {
	                    window.__insertimage = self.edit.cmd.insertimage;
	                    self.edit.cmd.insertimage = function (url, title, width, height, border, align) {
	                        text.value = url;
	                        self.edit.cmd.insertimage = window.__insertimage;
	                    };
	                    self.clickToolbar("image");
	                };
	            }
	        });
	        prettyPrint();
	    });
    </script>
<%--    <style type="text/css">
    .ke-container { display:none;}
    </style>--%>
</head>
<body>
    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
    <form id="example" runat="server">
        <textarea id="content1" cols="100" rows="8" style=" width:1px;height:1px;visibility:hidden;" runat="server"></textarea>
        <br />
        <asp:Button ID="Button1" runat="server" Text="提交内容" /> (提交快捷键: Ctrl + Enter)
    <input type="textbox" name="tb" value=" " />
    <input type="button" name="name" value="button " onclick="selImage(example.tb);" />
    </form>
</body>
</html>
