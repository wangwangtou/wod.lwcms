; (function (myDraw, window) {
    var drawing2d = myDraw.Drawing2D;
    function game1(container) {
        var inited = false;
        var ui = {};
        this._respath = "game1-res/";
        ui.d2d = null;
        this.init = function () {
            if (inited) return;
            container.innerHTML = "<canvas width=\"640\" height=\"320\" style=\"background:#eee;\"></canvas>";
            ui.d2d = new drawing2d(container.getElementsByTagName("canvas")[0]);
            ui.init();
            var context = this;
            inited = true;
        };
        var score = 0,life = 3;
        var uno = 0, uindex = 0, allnoarray = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"], aniarray = [], speed = 1000;
        var lock = true;
        this.next = function () {
            if (lock) return;
            ui.pvnext();
            uno = allnoarray[(++uindex) % allnoarray.length];
            ui.displayuno(uno);
            return uno;
        };
        this.hit = function () {
            if (lock) return;
            if (aniarray && aniarray.length && aniarray[0] == uno) {
                ui.pvhitsuc();
                aniarray.shift();
                ui.displayani(aniarray);
                score += 10;
                return true;
            }
            ui.pvhiterr();
            return false;
        };
        this._step = function () {
            this.pushNo();
            if (this.checkDead()) {
                lock = true;
                life--;
                if (this.checkOver()) {
                    ui.displayover(score);
                    ui.pvover();
                    this.endGame();
                }
                else {
                    ui.pvdead();
                    var context = this;
                    if (interval) clearInterval(interval);
                    setTimeout(function () { context.startLife(); }, 2000);
                }
            }
            else {
                ui.displayani(aniarray);
            }
        };
        var interval;
        this.pushNo = function () {
            aniarray.push(allnoarray[parseInt(allnoarray.length * Math.random())]);
        };
        this.startLife = function () {
            lock = false;
            ui.pvstart();
            if (interval) clearInterval(interval);
            aniarray = [];
            this.pushNo();
            var context = this;
            ui.displayuno(uno);
            ui.displayani(aniarray);
            ui.displaylife(life);
            interval = setInterval(function () { context._step(); }, speed);
        };
        this.checkDead = function () {
            return aniarray.length >= 9;
        };
        this.checkOver = function () {
            return life <= 0;
        };
        this.endGame = function () {
            if (interval) clearInterval(interval);
            aniarray = [];
        };
        this.start = function () {
            life = 3;
            score = 0;
            this.startLife();
        };
        ui.displayani = function (array) {
            this.d2d.clearRect(128, 100, 640 - 128, 50);
            for (var i = array.length - 1, j = allnoarray.length - 1; i >= 0; i--, j--) {
                this.d2d.draw({ type: "text", text: array[i], x: 64 * j + 20, y: 130 });
            }
        };
        ui.displayuno = function (uno) {
            this.d2d.clearRect(0, 100, 50, 50);
            this.d2d.draw({ type: "text", text :uno, x: 20, y: 130 });
        };
        ui.displaylife = function (life) {
            this.d2d.clearRect(64, 100, 50, 50);
            this.d2d.draw({ type: "text", text: life, x: 80, y: 130 });
        };
        ui.displayover = function (score) {
            this.d2d.clearRect(0, 0, 640, 320);
            this.d2d.draw({ type: "text", text: "Game over,your score is " + score +"!", x: 80, y: 130 });
        };
        var context = this;
        ui.init = function () {
            var cmd = window.document.createElement("div");
            cmd.innerHTML = "<input type='button' value='aim' id='btn_aim'><input type='button' value='fire' id='btn_fire'>";
            window.document.body.appendChild(cmd);
            var game = context;
            var ui = this;
            var aim = window.document.getElementById("btn_aim");
            var touched;
            var nextInterval;
            var holdfunc = function () { if (touched) game.next(); };
            var clickdown = function () {
                game.next();
                touched = true;
                nextInterval = setInterval(holdfunc, 150);
            };
            var clickup = function () {
                touched = false;
                if (nextInterval) clearInterval(nextInterval);
            };
            if (document.hasOwnProperty("ontouchstart")) {
                aim.ontouchstart = clickdown;
                aim.ontouchend = clickup;
            }
            else {
                aim.onmousedown = clickdown;
                aim.onmouseup = clickup;
                aim.onmouseout = clickup;
            }
            window.document.getElementById("btn_fire").onclick = function () {
                game.hit();
            };
        };
        ui.pvstart = function () { };
        ui.pvnext = function () { };
        ui.pvhitsuc = function () { };
        ui.pvhiterr = function () { };
        ui.pvdead = function () { };
        ui.pvover = function () { };
    }
    game1.prototype = {
    };
    window.Games = window.Games || {};
    window.Games.game1 = game1;
})(myDraw,window);