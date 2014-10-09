; (function (window,$) { 
    //实现逻辑
        function Drawing2D(dom,width,height) {
            var canvas = window.document.createElement("canvas");
            canvas.setAttribute("width", width);
            canvas.setAttribute("height", height);
            dom.appendChild(canvas);
            var context = canvas.getContext("2d");
            //画图，传入参数为htmlGeo，基于浏览器坐标的geo对象
            //该坐标如下
            //0,0    10,0
            //0,1    10,1
            this.draw = function (htmlGeo) {
                switch (htmlGeo.type) {
                    case "point":
                        context.fillRect(htmlGeo.x - 1, htmlGeo.y - 1, 3, 3);
                        break;
                    case "label":
                        context.fillText(htmlGeo.text, htmlGeo.x, htmlGeo.y);
                        break;
                    case "line":
                        context.beginPath(); context.moveTo(htmlGeo.points[0].x, htmlGeo.points[0].y);
                        for (var i = 1; i < htmlGeo.points.length; i++) { context.lineTo(htmlGeo.points[i].x, htmlGeo.points[i].y); }
                        context.stroke();
                        break;
                    default: break;
                }
            };
        }
        function GraphTransfer(hRect,gRect) {
            var hX1, hY1, hX2, hY2; //html的两个对角坐标值
            var gX1, gY1, gX2, gY2; //graph的两个对角坐标值

            this.Graph2Html = function (gGeo) {
                var hGeo = { type: gGeo.type };
                if (hGeo.type == "point") {
                    var point = this.GraphPoint2Html(gGeo);
                    hGeo.x = point.x;
                    hGeo.y = point.y;
                }
                else {
                    hGeo.points = [];
                    for (var i = 0; i < gGeo.points.length; i++) {
                        hGeo.points.push(this.GraphPoint2Html(gGeo.points[i]));
                    }
                }
                return hGeo;
            };
            this.GraphPoint2Html = function (p) {
                return { 
                    x: (p.x - gX1) * (hX2 - hX1) / (gX2 - gX1),
                    y: (hY2 - hY1) - (p.y - gY1) * (hY2 - hY1) / (gY2 - gY1)
                };
            };
            this.Html2Graph = function(hGeo){
                var gGeo = { type: hGeo.type };
                if (gGeo.type == "point") {
                    var point = this.HtmlPoint2Graph(hGeo);
                    gGeo.x = point.x;
                    gGeo.y = point.y;
                }
                else {
                    gGeo.points = [];
                    for (var i = 0; i < hGeo.points.length; i++) {
                        gGeo.points.push(this.HtmlPoint2Graph(hGeo.points[i]));
                    }
                }
                return gGeo;
            };
            this.HtmlPoint2Graph = function (p) {
                return {
                    x: (p.x - hX1) * (gX2 - gX1) / (hX2 - hX1),
                    y: (gY2 - gY1) - (p.y - hY1) * (gY2 - gY1) / (hY2 - hY1)
                };
            };
            this.UpdateSetting = function (hRect, gRect) {
                hX1 = hRect.X;
                hY1 = hRect.Y;
                hX2 = hRect.Width;
                hY2 = hRect.Height;
                gX1 = gRect.X;
                gY1 = gRect.Y;
                gX2 = gRect.Width;
                gY2 = gRect.Height;
            };
            if (hRect && gRect) {
                this.UpdateSetting(hRect, gRect);
            }
        }
        function MerGraph(dom,width, height) {
            var drawing2d = new Drawing2D(dom,width, height);
            var transfer = new GraphTransfer();
            this.paintGraph = function (setting) {
                this.setting = setting;
                this.updateSetting();
                this.paintBackground();
                for (var i = 0; i < setting.Expresses.length; i++) {
                    this.paintExp(setting.Expresses[i]);
                }
            };
            this.updateSetting = function () {
                var gRect = { X: 0, Y: 0 };
                gRect.Width = this.setting.Part.x2 + this.setting.Part.x1;
                gRect.Height = this.setting.Part.y2 + this.setting.Part.y1;
                this.gRect = gRect;
                transfer.UpdateSetting(
                    { X: 0, Y: 0, Height: height, Width: width },
                    gRect);
            };
            this.paintBackground = function () {
                var xline = { type: "line", points: [{ x: 0, y: 0 }, { x: this.gRect.Width, y: 0}] };
                var yline = { type: "line", points: [{ x: 0, y: 0 }, { x: 0, y: this.gRect.Height}] };
                xline = transfer.Graph2Html(xline);
                yline = transfer.Graph2Html(yline);
                drawing2d.draw(xline);
                drawing2d.draw(yline);
                drawing2d.draw({ type: "label", text: this.setting.XName, x: xline.points[1].x - 15, y: xline.points[1].y - 15 });
                drawing2d.draw({ type: "label", text: this.setting.YName, x: yline.points[1].x + 5, y: yline.points[1].y + 15 });
                drawing2d.draw({ type: "label", text: "0", x: xline.points[0].x + 5, y: xline.points[0].y - 15 });
            };
            this.paintExp = function (exp) {
                var geos = this.getExpGeo(exp.express);
                if (geos && typeof (geos.type) == "string") {
                    geos = [geos];
                }
                var geo;
                for (var i = 0; i < geos.length; i++) {
                    geo = transfer.Graph2Html(geos[i]);
                    drawing2d.draw(geo);
                }
                var lastpoint = geo.points ? geo.points[geo.points.length - 1] : geo;
                drawing2d.draw({ type: "label", text: exp.name, x: lastpoint.x - 20, y: lastpoint.y - 20 });
            }
            this.getExpGeo = function (exp) {
                var gd = { type: "point", x: this.setting.Part.x2 - this.setting.Part.x1, y: this.setting.Part.y2 - this.setting.Part.y1 };
                var hd = transfer.Graph2Html(gd);
                var dx = gd.x / hd.x;
                var dy = gd.y / hd.y;

                var left = exp.split("=")[0];
                if (left == this.setting.XName) {
                    var cal = this.getCal(exp);
                    var geo = { type: "line", points: [] };
                    for (var y = this.setting.Part.y1; y < this.setting.Part.y2; y += dy) {
                        geo.points.push({ x: cal(y), y: y });
                    }
                    geo.points.push({ x: cal(this.setting.Part.y2), y: this.setting.Part.y2 });
                    return geo;
                }
                else if (left == this.setting.YName) {
                    var cal = this.getCal(exp);
                    var geo = { type: "line", points: [] };
                    for (var x = this.setting.Part.x1; x < this.setting.Part.x2; x += dx) {
                        geo.points.push({ y: cal(x), x: x });
                    }
                    geo.points.push({ y: cal(this.setting.Part.x2), x: this.setting.Part.x2 });
                    return geo;
                }
                else {
                    return [];
                }
            };
            this.getCal = function (exp) {
                if (this.calCache && this.calCache[exp])
                    return this.calCache[exp];
                else {
                    this.calCache = this.calCache || {};
                    var left_right = exp.split("=");
                    var funStrs = [];
                    funStrs.push("function(");
                    if (left_right[0] == this.setting.XName) {
                        funStrs.push(this.setting.YName);
                    }
                    else {
                        funStrs.push(this.setting.XName);
                    }
                    funStrs.push("){ var ");
                    var initValues = [];
                    for (var name in this.setting.InitValue) {
                        initValues.push(name + "=" + this.setting.InitValue[name]);
                    }
                    funStrs.push(initValues.join(",") + ";");
                    funStrs.push("return " + left_right[1] + ";}");
                    var cal = eval("(" + funStrs.join("") + ")");
                    this.calCache[exp] = cal;
                    return cal;
                }
            };
        }
        $.fn.extend({
            merGraph : function(setting){
                this.each(function(){
                    var datagraph = setting ? setting :$(this).data("graph")||eval("("+$(this).attr("data-graph")+")");
                    var mer = new MerGraph(this,datagraph.Width,datagraph.Height);
                    mer.paintGraph(datagraph);
                });
            }
        });
        $(function(){
            $("[data-graph]").merGraph();
        });
})(window,jQuery);