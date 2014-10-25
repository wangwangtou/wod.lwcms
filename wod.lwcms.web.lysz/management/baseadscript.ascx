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