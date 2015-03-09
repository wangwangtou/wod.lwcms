/// <reference path="../management/scripts/management.core.js" />
(function (v) {
    var vector2D = wod.CLS.getClass({
        x: NaN,
        y: NaN,
        _init: function (x, y) {
            this.parent();
            if (arguments.length) {
                this.init(x, y);
            }
        },
        init: function (x, y) {
            this.x = x;
            this.y = y;
        },
        length: function () {
            return Math.sqrt(this.x * this.x + this.y * this.y);
        },
        sqrLength: function () {
            return this.x * this.x + this.y * this.y;
        },
        //        normalize: function () {
        //            var inv = 1 / this.length();
        //            return new v.vector2D(this.x * inv, this.y * inv);
        //        },
        negate: function () {
            return new vector2D(-this.x, -this.y);
        },
        add: function (v) {
            return new vector2D(this.x + v.x, this.y + v.y);
        },
        subtract: function (v) {
            return new vector2D(this.x - v.x, this.y - v.y);
        },
        multiply: function (f) {
            ///数乘向量
            return new vector2D(this.x * f, this.y * f);
        },
        divide: function (f) {
            ///数乘向量（除法）
            var invf = 1 / f;
            return new vector2D(this.x * invf, this.y * invf);
        },
        dot: function (v) {
            //数量积 a·b
            return this.x * v.x + this.y * v.y;
        },
        angelTo: function (v) {
            ///两向量的夹角，当前角度为起点，逆时针到v的角度
            //cos a = this.dot(v) / 
            var cosA = this.dot(v) / (this.length() * v.length());
            var a = Math.acos(cosA);
            return a;
        },
        /*2D没有向量积？
        cross: function (v) {
        //向量积 a×b
        return new v.vector2D(-this.z * v.y + this.y * v.z, this.z * v.x - this.x * v.z, -this.y * v.x + this.x * v.y);
        },*/
        equals: function (v) {
            return this.x == v.x && this.y == v.y;
        }
    }, "wod.V.vector2D");
    wod.statics.mixin(vector2D, {
        //0向量
        zero: new vector2D(0, 0),
        f1: function (v1, v2, p) {
            ///定比分点公式（向量P1P=λ•向量PP2）
            return new vector2D((v1.x + p * v2.x) / (1 + p), (v1.y + p * v2.y) / (1 + p));
        },
        f2: function (v1, v2, v3) {
            ///三点共线定理  若OC=λOA +μOB ,且λ+μ=1 ,则A、B、C三点共线

            //v3.x = x * v1.x + (1-x) * v2.x;
            //v3.y = x * v1.y + (1-x) * v2.y;
            if (v1.equals(v2)) return true;
            return (v3.x - v2.x) * (v1.y - v2.y) == (v3.y - v2.y) * (v1.x - v2.x);
        },
        f3: function (v1, v2, v3) {
            ///三角形重心判断式  在△ABC中，若GA +GB +GC=O,则G为△ABC的重心
            //GA = OA-OG
            //GB = OB-OG
            //GC = OC-OG
            //3OG = (OA + OB + OC)
            return v1.add(v2).add(v3).divide(3);
        },
        f4: function (v1, v2) {
            ///向量共线
            //  若b≠0，则a//b的充要条件是存在唯一实数λ，使a=λb。 
            //  a//b的充要条件是 xy'-x'y=0。 
            //  零向量0平行于任何向量。 
            if (v2.equals(vector2D.zero)) {
                return true;
            }
            else {
                return v1.x * v2.y == v1.y * v2.x;
            }
        },
        f5: function (v1, v2) {
            ///向量垂直
            //  a⊥b的充要条件是 a•b=0。 
            //  a⊥b的充要条件是 xx'+yy'=0。 
            //  零向量0垂直于任何向量.                
            return v1.dot(v2) == 0;
        }
    });

    var vector3D = wod.CLS.getClass({
        x: NaN,
        y: NaN,
        z: NaN,
        _init: function (x, y, z) {
            this.parent();
            if (arguments.length) {
                this.init(x, y, z);
            }
        },
        init: function (x, y, z) {
            this.x = x;
            this.y = y;
            this.z = z;
        },
        length: function () {
            return Math.sqrt(this.sqrLength());
        },
        sqrLength: function () {
            return this.x * this.x + this.y * this.y + this.z * this.z;
        },
        normalize: function () {
            var inv = 1 / this.length();
            return new vector3D(this.x * inv, this.y * inv, this.z * inv);
        },
        negate: function () {
            return new vector3D(-this.x, -this.y, -this.z);
        },
        add: function (v) {
            return new vector3D(this.x + v.x, this.y + v.y, this.z + v.z);
        },
        subtract: function (v) {
            return new vector2D(this.x - v.x, this.y - v.y, this.z - v.z);
        },
        multiply: function (f) {
            ///数乘向量
            return new vector2D(this.x * f, this.y * f, this.z * f);
        },
        divide: function (f) {
            ///数乘向量（除法）
            var invf = 1 / f;
            return this.multiply(invf);
        },
        dot: function (v) {
            //数量积 a·b
            return this.x * v.x + this.y * v.y + this.z * v.z;
        },
        angelTo: function (v) {
            ///两向量的夹角，当前角度为起点，逆时针到v的角度
            //cos a = this.dot(v) / 
            var cosA = this.dot(v) / (this.length() * v.length());
            var a = Math.acos(cosA);
            return a;
        },
        cross: function (v) {
        //向量积 a×b
            return new vector3D(-this.z * v.y + this.y * v.z, this.z * v.x - this.x * v.z, -this.y * v.x + this.x * v.y);
        },
        equals: function (v) {
            return this.x == v.x && this.y == v.y && this.z == v.z;
        }
    }, "wod.V.vector3D");


    wod.statics.mixin(vector3D, {
        //0向量
        zero: new vector3D(0, 0, 0),
        f1: function (v1, v2, p) {
            ///定比分点公式（向量P1P=λ•向量PP2）
            return new vector3D((v1.x + p * v2.x) / (1 + p), (v1.y + p * v2.y) / (1 + p), (v1.z + p * v2.z) / (1 + p));
        },
        f2: function (v1, v2, v3) {
            ///三点共线定理  若OC=λOA +μOB ,且λ+μ=1 ,则A、B、C三点共线

            //v3.x = x * v1.x + (1-x) * v2.x;
            //v3.y = x * v1.y + (1-x) * v2.y;
            //v3.z = x * v1.z + (1-x) * v2.z;
            if (v1.equals(v2)) return true;
            return (v3.x - v2.x) * (v1.y - v2.y) == (v3.y - v2.y) * (v1.x - v2.x)
                && (v3.x - v2.x) * (v1.z - v2.z) == (v3.z - v2.z) * (v1.x - v2.x);
        },
        f3: function (v1, v2, v3) {
            ///三角形重心判断式  在△ABC中，若GA +GB +GC=O,则G为△ABC的重心
            //GA = OA-OG
            //GB = OB-OG
            //GC = OC-OG
            //3OG = (OA + OB + OC)
            return v1.add(v2).add(v3).divide(3);
        },
        f4: function (v1, v2) {
            ///向量共线
            //  若b≠0，则a//b的充要条件是存在唯一实数λ，使a=λb。 
            //  a//b的充要条件是 xy'-x'y=0。 
            //  零向量0平行于任何向量。 
            if (v2.equals(vector3D.zero)) {
                return true;
            }
            else {
                return v1.x * v2.y == v1.y * v2.x
                    && v1.x * v2.z == v1.z * v2.x;
            }
        },
        f5: function (v1, v2) {
            ///向量垂直
            //  a⊥b的充要条件是 a•b=0。 
            //  a⊥b的充要条件是 xx'+yy'=0。 
            //  零向量0垂直于任何向量.                
            return v1.dot(v2) == 0;
        }
    });
})(wod.CLS.getNS("wod.V"));
