; (function (myDraw) {
    var drawing2d = myDraw.Drawing2D;
    function FramesManager(drawing2d) {
        var frameses = {};
        var _starts = {};
        var _interval;
        var _intervalSpan = 30;
        var _timecache = {};
        var _updatestate = {};
        var _drawFrames = {};
        var animateCal = {
            liner: function (from, to, part) {
                return from + part * (to - from);
            },
            sin: function (from, to, part) {
                return from + Math.sin(part * Math.PI/2) * (to - from);
            }
        };
        this.addFrames = function (setting) {
            frameses[setting.name] = setting;
            _drawFrames[setting.name] = function (time) {
                //totalFrame,frames[ obj , steps[ start , end , changes[ { attr , from , to } ] ] ]
                time = time > setting.totalFrame ? time % setting.totalFrame : time;
                for (var i = 0; i < setting.frames.length; i++) {
                    var frame =setting.frames[i];
                    for (var j = 0; j < frame.steps.length; j++) {
                        var step = frame.steps[j];
                        if (step.start <= time && step.end >= time)
                        {
                            for (var k = 0; k < step.changes.length; k++) {
                                var change = step.changes[k];
                                var cal = typeof (change.animate) == "function" ? change.animate : animateCal[change.animate || "liner"];
                                var val = cal(change.from,change.to,(time-step.start)/(step.end-step.start));
                                change.attr(frame.obj,val);
                            }
                            break;
                        }
                    }
                    drawing2d.draw(frame.obj);
                }
            };
            if (setting.auto)
                this.startFrames(setting.name);
        };
        function reDraw() {
            var w = drawing2d._canvas.width;
            var h = drawing2d._canvas.height;
            drawing2d.draw({ type: 'polygon', points: [{ x: 0, y: 0 }, { x: 0, y: h }, { x: w, y: h }, { x: w, y: 0 }, { x: 0, y: 0 }], fill: "transparent", stroke: "#ffffff", strokeWidth: 1 });
        }
        this.startFrames = function (name) {
            var frames = frameses[name];
            if (frames) {
                _starts[name] = name;
                _timecache[name] = _timecache[name] || 0;
                if (!_interval) {
                    _interval = setInterval(function () {
                        reDraw();
                        for (var i in _starts) {
                            _timecache[i] += _intervalSpan;
                            _drawFrames[i](_timecache[i]);
                        }
                    }, _intervalSpan);
                }
            }
        };
        this.stopFrames = function (name) {
            if (_starts[name]) {
                delete _starts[name];
                var count = 0;
                for (var i in _starts) {
                    count += 1;
                    break;
                }
                count || clearInterval(_interval);
                _interval = null;
            }
        };
    }

    function addFrames(setting) {
        this.framesManager = this.framesManager || new FramesManager(this);
        this.framesManager.addFrames(setting);
    }
    function startFrames(name) {
        this.framesManager = this.framesManager || new FramesManager(this);
        this.framesManager.startFrames(name);
    }
    function stopFrames(name) {
        this.framesManager = this.framesManager || new FramesManager(this);
        this.framesManager.stopFrames(name);
    }
    drawing2d.prototype.addFrames = addFrames;
    drawing2d.prototype.startFrames = startFrames;
    drawing2d.prototype.stopFrames = stopFrames;
})(myDraw);