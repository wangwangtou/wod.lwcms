<%@ Control Language="C#" AutoEventWireup="true" Inherits="wod.lwcms.web.tempControl,wod.lwcms.core" %>
<script type="text/javascript">
  $(document).on("click", "a", function () {
    var href = $(this).attr("href");
    if (href.indexOf("/index.aspx") == 0 && $(this).parents(".wodbody").length) {
      location.href = href.replace("/index.aspx", "/management/basead.aspx");
      return false;
    }
  });
</script>
<script type="text/javascript" src="<%=ResourceUrl("management/scripts/basead/editor.js") %>?v=1"></script>
<script type="text/javascript">
    var ws_hash = "<%=common.GetHash(PD.ws) %>";
    var allCats_hash = "<%=common.GetHash(PD.allCats) %>";
    var ws_edit = [];//{type="edit", dataexp="[0].siteName", value="abc"},{type="edit", dataexp="[0].siteName", value="abc"}
    var allCats_edit = [];
</script>
<script type="text/javascript">
    function saveCallback(result) {
        if (!result.ws_IsValid) {
            alert(result.ws_Messages[0]);
            location.reload();
            return;
        }
        else {
            ws_hash = result.ws_NewHash;
            ws_edit = [];
        }
        if (!result.cat_IsValid) {
            alert(result.cat_Messages[0]);
            location.reload();
            return;
        }
        else {
            cat_hash = result.cat_NewHash;
            allCats_edit = [];
        }
    }
    $(function () {
        var btn_save = $('<input type="button" id="btn_save" value="保存" style="position:fixed;top:0px;right:0px;" />');
        btn_save.appendTo("body");
        btn_save.click(function () {
            var data = {
                "ae_ws_edit": $.toJSON(ws_edit),
                "ae_ws_edit_hash": ws_hash,
                "ae_allCats_edit_hash": $.toJSON(allCats_edit),
                "ae_allCats_edit_hash": allCats_hash
            };
            saveBase(data, saveCallback);
        });
    });
</script>