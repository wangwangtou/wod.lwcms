<%@ Page Language="C#" AutoEventWireup="true" 
MasterPageFile="~/management/managepage.master"%>
<script runat="server" type="text/C#">
    public override string ajaxCmd { get {
        return Request.QueryString["act"] == "download" ? "updatedownload"
            : (Request.QueryString["act"] == "update" ? "updateupdate"
                : (Request.QueryString["act"] == "restart" ? "updaterestart"
                : "")); 
    } set { } }
    public override string loadCmd
    {
        get { return "updateload"; }
        set { }
    }
    
    protected override void PreLoadCommand()
    {
        //ServerParams.Add("catPath", path);
        base.PreCommand();
    }
</script>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function _update(id) {
            if (confirm("确定要应用该更新？")) {
                $.ajax({ url: "update.aspx?act=update", data: { cmtid: id }, type: "post", success: function (data) {
                    location.reload();
                }, error: function () { alert("删除失败！"); location.reload(); }
                });
            }
        }
        function downloadSubmit(form) {
            if (form.dpath.value) {
                return true;
            }
            else {
                alert("请输入路径");
                return false;
            }
        }
        function updateSubmit(form) {
            var f = form.updatepkg.value;
            if (f) {
                if (f.substring(f.length - 4, f.length).toLowerCase() == ".zip") {
                    return true;
                }
                else {
                    alert("请选择Zip文件");
                    return false;
                }
            }
            else {
                alert("请选择更新文件");
                return false;
            }
        }
        function _restart() {
            if (confirm("确定要重启？")) {
                $.ajax({ url: "update.aspx?act=restart", data: { a: 1 }, type: "post", success: function (data) {
                    location.reload();
                }, error: function () {
                    location.reload();
                }, timeout:5000 });
            }
        }
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="g-index">
    <fieldset>
        <legend>下载</legend>
        <form action="update.aspx?act=download" method="post" target="download" onsubmit="return downloadSubmit(this);">
            <label>路径：<input type="text" name="dpath" value="App_Data" /></label>
            <input type="submit" value="下载" />
        </form>
    </fieldset>
    <fieldset>
        <legend>更新</legend>
        <form action="update.aspx?act=update" method="post" enctype="multipart/form-data" onsubmit="return updateSubmit(this);">
            <label>更新包：<input type="file" name="updatepkg" /></label>
            <input type="submit" value="更新" />
        </form>
    </fieldset>
    <fieldset>
        <legend>重启</legend>
        <input type="button" value="重启" onclick="_restart()" />
    </fieldset>
</div>
</asp:Content>