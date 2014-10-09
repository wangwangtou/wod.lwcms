; (function (myDraw) {
    var drawing2d = myDraw.Drawing2D;
    function Drawing2DEventHelper(drawing2d) {
        var context = this;
        var canvas = drawing2d._canvas;
        this._tools = [];
        var curTool = null;
        this.setTool = function (tool) {
            curTool = tool;
            tool.drawing2d = drawing2d;
            context._tools.push(tool);
        };
        canvas.onclick = function (evt) {
            console.log("onclick" + evt);
            curTool && curTool.click(evt);
        };
        canvas.onmousedown = function (evt) {
            console.log("onmousedown" + evt);
            curTool && curTool.mousedown(evt);
        };
        canvas.onmouseup = function (evt) {
            console.log("onmouseup" + evt);
            curTool && curTool.mouseup(evt);
        };
        canvas.onmousemove = function (evt) {
            console.log("onmousemove" + evt);
            curTool && curTool.mousemove(evt);
        };
        canvas.onmouseout = function (evt) {
            console.log("onmouseout" + evt);
            curTool && curTool.mouseout(evt);
        };
        //canvas.onkeydown = function () {
        //    console.log("onkeydown");
        //};
        //canvas.onkeyup = function () {
        //    console.log("onkeyup");
        //};
    }

    function setTool(tool) {
        this.eventHelper = this.eventHelper || new Drawing2DEventHelper(this);
        this.eventHelper.setTool(tool);
    }
    drawing2d.prototype.setTool = setTool;

    function DragTool() {
        var selectdObject = null;
        this.click = function (evt) { };
        this.mousedown = function (evt) { };
        this.mouseup = function (evt) { };
        this.mousemove = function (evt) { };
        this.mouseout = function (evt) { };
    }
    function LineTool() {
        var context = this;
        var selectdObject = null;
        var isDrawing = false;
        var downPoint;
        var lastPoint;
        function drawLine(evt){
            var startPoint = lastPoint || downPoint;
            var endPoint = { x: evt.offsetX, y: evt.offsetY };
            context.drawing2d.draw({ type: "line", points: [startPoint, endPoint] });
            lastPoint = endPoint;
        }
        this.click = function (evt) {  };
        this.mousedown = function (evt) { isDrawing = true; downPoint = { x: evt.offsetX, y: evt.offsetY }; };
        this.mouseup = function (evt) { if (isDrawing) { isDrawing = false; drawLine(evt); downPoint = null; lastPoint = null; } };
        this.mousemove = function (evt) {if (isDrawing) { drawLine(evt); } };
        this.mouseout = function (evt) { };
    }
    var commonTools = {
        dragTool: new DragTool(),
        lineTool: new LineTool()
    };

    myDraw.tools = commonTools;
})(myDraw);