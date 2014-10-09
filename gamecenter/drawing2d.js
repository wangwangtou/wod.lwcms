;(function () {
    function _mapSetting() {
        this.DefaultFillStyle = "#000000";
        this.DefaultStrokeStyle = "#000000";
        this.PointSize = 10;
        this.LineFillStyle = "transparent";
        this.DefaultLineWidth = 3;

        this.DefaultStrokeWidth = 3;
    }
    var _map = {};
    _map.setting = new _mapSetting();
    var imgCache = {};
    function Drawing2D(canvas) {
        var drawing2d = this;
        this._canvas = canvas;
        this._2d = canvas.getContext("2d");

        this._drawImage = function (img, callback) {
            if (img.complete)
                callback();
            else
                img.addEventListener("load",function () { callback(); });
        };
        this.clearRect = function (x, y, width, height) {
            this._2d.clearRect(x, y, width, height);
        };
        this.draw = function (geo) {
            if (geo) {
                switch (geo.type) {
                    case "point":
                        this._2d.fillStyle = geo.color || _map.setting.DefaultFillStyle;
                        this._2d.fillRect(geo.x - (_map.setting.PointSize / 2), geo.y - (_map.setting.PointSize / 2), _map.setting.PointSize, _map.setting.PointSize); break;
                    case "line":
                        this._2d.strokeStyle = geo.color || _map.setting.DefaultStrokeStyle;
                        this._2d.fillStyle = _map.setting.LineFillStyle;
                        this._2d.lineWidth = geo.lineWidth || _map.setting.DefaultLineWidth;
                        this._2d.beginPath(); this._2d.moveTo(geo.points[0].x, geo.points[0].y); for (var i = 1; i < geo.points.length; i++) this._2d.lineTo(geo.points[i].x, geo.points[i].y); this._2d.stroke(); break;
                    case "polygon":
                        this._2d.strokeStyle = geo.stroke || _map.setting.DefaultStrokeStyle;
                        this._2d.fillStyle = geo.fill || _map.setting.DefaultFillStyle;
                        this._2d.lineWidth = geo.strokeWidth || _map.setting.DefaultStrokeWidth;
                        this._2d.beginPath(); this._2d.moveTo(geo.points[0].x, geo.points[0].y); for (var i = 1; i < geo.points.length; i++) this._2d.lineTo(geo.points[i].x, geo.points[i].y); this._2d.stroke(); this._2d.fill(); break;
                    case "image":
                        var img = imgCache[geo.src] 
                        if (!img) { img = new Image(geo.rect.width, geo.rect.height); img.src = geo.src; imgCache[geo.src] = img;}
                        this._drawImage(img, function () {
                            if(geo.clip)
                            {
                            drawing2d._2d.drawImage(img, geo.clip.x, geo.clip.y, geo.clip.width, geo.clip.height,geo.rect.x, geo.rect.y, geo.rect.width, geo.rect.height);
                            }
                            else
                            {
                            drawing2d._2d.drawImage(img, geo.rect.x, geo.rect.y, geo.rect.width, geo.rect.height);}
                        }); break;
                    case "text":
                        this._2d.fillText(geo.text, geo.x, geo.y);
                        break;
                    default:
                        break;
                }
            }
        };
    }
    var myDraw = window.myDraw || {};
    window.myDraw = myDraw;

    myDraw.Drawing2D = Drawing2D;
})(window);