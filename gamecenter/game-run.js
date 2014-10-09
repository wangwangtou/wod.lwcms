; (function (myDraw, window) {
    var drawing2d = myDraw.Drawing2D;
    function runApp(container) {
        var inited = false;
        this._respath = "game-run-res/";
        this.bgDraw = null;
        this.acDraw = null;
        this.init = function () {
            if (inited) return;

            container.innerHTML = "<canvas width=\"800\" height=\"600\" style=\"background:#eee;\"></canvas><canvas width=\"800\" height=\"600\"></canvas>";
            this.bgDraw = new drawing2d(container.getElementsByTagName("canvas")[0]);
            this.acDraw = new drawing2d(container.getElementsByTagName("canvas")[1]);

            this.drawBackground();

            var context = this;
            window.document.onkeyup = function (evt) {                
                if (context.keyEnable) { context.keyup(evt); evt.stopPropagation(); }
            }
            window.document.onkeydown = function (evt) {
                if (context.keyEnable) { context.keydown(evt); evt.stopPropagation(); }
            }

            this.initActiveFrame();
            inited = true;
        };
    }
    runApp.prototype = {
        drawBackground: function () {
            this.bgFrames = {
                name: "run-bg",
                auto: false, totalFrame: 1000 * 60, frames: [
                    {
                        obj: { type: 'image', rect: { x: 0, y: 0, width: 800, height: 600 },clip:{x: 0, y: 0, width: 800, height: 600}, src: this._respath + "bg.png" },
                        steps: [{ start: 1000 * 0, end: 1000 * 60, changes: [{ attr: function (obj, val) { obj.clip.x = val; }, from: 0, to: 2000 }] }
                        ]
                    },
                    {
                        obj: { type: 'image', rect: { x: 0, y: 0, width: 800, height: 600 },clip:{x: 0, y: 0, width: 800, height: 600}, src: this._respath + "bg.png" },
                        steps: [{ start: 1000 * 0, end: 1000 * 36, changes: [{ attr: function (obj, val) { obj.rect.width = 0; obj.rect.height = 0; }, from: 0, to: 0 }] }, { start: 1000 * 36, end: 1000 * 60, changes: [{ attr: function (obj, val) { obj.rect.width = 800 - val; obj.rect.height = 600; obj.rect.x = val; obj.clip.width = 800 - val; }, from: 800, to: 0 }] }
                        ]
                    }
                ]
            };
            this.bgDraw.addFrames(this.bgFrames);
        },
        start: function () {
            this.keyEnable = true;
            this.bgDraw.startFrames(this.bgFrames.name);
        },
        initActiveFrame: function () {
            var minInterval = 100;
            this.plFrames = {
                name: "run-player",
                auto: false, totalFrame: minInterval * 5, frames: [
                    {
                        obj: { type: 'image', rect: { x: 100, y: 400, width: 32, height: 32 }, clip: { x: 0, y: 32, width: 32, height: 32 }, src: this._respath + "image.png" },
                        steps: [{ start: minInterval * 0, end: minInterval * 1, changes: [{ attr: function (obj, val) { obj.clip.x = 32; }, from: 0, to: 0 }] },
                            { start: minInterval * 1, end: minInterval * 2, changes: [{ attr: function (obj, val) { obj.clip.x = 64; }, from: 0, to: 0 }] },
                            { start: minInterval * 2, end: minInterval * 3, changes: [{ attr: function (obj, val) { obj.clip.x = 32; }, from: 0, to: 0 }] },
                            { start: minInterval * 3, end: minInterval * 4, changes: [{ attr: function (obj, val) { obj.clip.x = 0; }, from: 0, to: 0 }] },
                            { start: minInterval * 4, end: minInterval * 5, changes: [{ attr: function (obj, val) { obj.clip.x = 32; }, from: 0, to: 0 }] }
                        ]
                    }
                ]
            };
            //96*128  3*4 == 32*32
            //left   0,32 32,32 64,32
            //right  0,64 32,64 64,64
            this.acDraw.addFrames(this.plFrames);
        },
        toleft: function () {
            this.plFrames.frames[0].obj.clip.y = 32;
            this.__run();
        },
        toright: function () {
            this.plFrames.frames[0].obj.clip.y = 64;
            this.__run();
        },
        __run: function () {
            this.acDraw.startFrames(this.plFrames.name);
            var context = this;
            setTimeout(function () {
                context.acDraw.stopFrames(context.plFrames.name);
            }, this.plFrames.totalFrame);
        },
        keyup: function (evt) {
            switch (evt.which) {
                case 65: this.toleft(); break;//a 0
                case 83: break;//s 18
                case 68: this.toright(); break;//d 3
                case 87: break;//w 22
                default: break;
            }
        },
        keydown: function (evt) {
        }
    };
    window.Games = window.Games || {};
    window.Games.run = runApp;
})(myDraw,window);