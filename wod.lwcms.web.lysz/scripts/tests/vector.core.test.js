/// <reference path="../../management/scripts/management.core.js" />
/// <reference path="../vector.core.js" />
/// <reference path="../../management/scripts/tests/management.testcore.js" />
test("wod.V.vector2D", !!wod.V.vector2D);
test("wod.V.vector2D init", new wod.V.vector2D(10, 2).x == 10);
test("wod.V.vector2D init", new wod.V.vector2D(10, 2).y == 2);
test("wod.V.vector2D length", new wod.V.vector2D(3, 4).length() == 5);
test("wod.V.vector2D sqrLength", new wod.V.vector2D(3, 4).sqrLength() == 25);
test("wod.V.vector2D negate",function () {
    var nr = new wod.V.vector2D(3, 4).negate();
    return nr.x == -3 && nr.y == -4;
});
test("wod.V.vector2D add",function () {
    var nr = new wod.V.vector2D(3, 4).add( new wod.V.vector2D(4, 7));
    return nr.x == 3+4 && nr.y == 4+7;
});
test("wod.V.vector2D subtract",function () {
    var nr = new wod.V.vector2D(3, 4).subtract( new wod.V.vector2D(4, 7));
    return nr.x == 3-4 && nr.y == 4-7;
});
test("wod.V.vector2D multiply",function () {
    var nr = new wod.V.vector2D(3, 4).multiply(12);
    return nr.x == 3*12 && nr.y == 4*12;
});
test("wod.V.vector2D divide",function () {
    var nr = new wod.V.vector2D(3, 4).divide(0.5);
    return nr.x == 3*2 && nr.y == 4*2;
});
test("wod.V.vector2D dot",function () {
    var nr = new wod.V.vector2D(3, 4).dot(new wod.V.vector2D(-4,3));
    return nr == 0;
});
test("wod.V.vector2D angelTo",function () {
    var nr = new wod.V.vector2D(3, 4).angelTo(new wod.V.vector2D(-4,3));
    return nr == Math.PI/ 2;
});
test("wod.V.vector2D equals",function () {
    return (new wod.V.vector2D(3, 4).equals(new wod.V.vector2D(-4,3))) === false
         && (new wod.V.vector2D(3, 4).equals(new wod.V.vector2D(3,4))) === true;
});
(function (vector2D) {
    test("wod.V.vector2D zero",function () {
        return vector2D.zero.equals(new wod.V.vector2D(0,0));
    });
    test("wod.V.vector2D f1 定比分点公式",function () {
        return vector2D.f1(new wod.V.vector2D(0,0),new wod.V.vector2D(10,0),1).equals(new wod.V.vector2D(5,0))
            && vector2D.f1(new wod.V.vector2D(10,6),new wod.V.vector2D(10,0),1/4).equals(new wod.V.vector2D(10,4.8));
    });
    test("wod.V.vector2D f2 三点共线定理",function () {
        return vector2D.f2(new wod.V.vector2D(0,0),new wod.V.vector2D(10,0),new wod.V.vector2D(10,0)) === true
            && vector2D.f2(new wod.V.vector2D(0,0),new wod.V.vector2D(10,0),new wod.V.vector2D(5,0)) === true
            && vector2D.f2(new wod.V.vector2D(0,0),new wod.V.vector2D(10,0),new wod.V.vector2D(0,0)) === true
            && vector2D.f2(new wod.V.vector2D(1,2),new wod.V.vector2D(3,4),new wod.V.vector2D(5,5)) === false
            && vector2D.f2(new wod.V.vector2D(1,2),new wod.V.vector2D(3,4),new wod.V.vector2D(5,6)) === true
            && vector2D.f2(new wod.V.vector2D(0,0),new wod.V.vector2D(10,0),new wod.V.vector2D(5,0)) === true;
    });
    test("wod.V.vector2D f2 三点共线定理",function () {
        return vector2D.f2(new wod.V.vector2D(0,0),new wod.V.vector2D(10,0),new wod.V.vector2D(10,0)) === true
            && vector2D.f2(new wod.V.vector2D(0,0),new wod.V.vector2D(10,0),new wod.V.vector2D(5,0)) === true
            && vector2D.f2(new wod.V.vector2D(0,0),new wod.V.vector2D(10,0),new wod.V.vector2D(0,0)) === true
            && vector2D.f2(new wod.V.vector2D(1,2),new wod.V.vector2D(3,4),new wod.V.vector2D(5,5)) === false
            && vector2D.f2(new wod.V.vector2D(1,2),new wod.V.vector2D(3,3),new wod.V.vector2D(5,6)) === false
            && vector2D.f2(new wod.V.vector2D(1,2),new wod.V.vector2D(3,4),new wod.V.vector2D(5,6)) === true;
    });
    test("wod.V.vector2D f3 三角形重心判断式",function () {
        return vector2D.f3(new wod.V.vector2D(1,2),new wod.V.vector2D(3,4),new wod.V.vector2D(5,5)).equals(new wod.V.vector2D(3,11/3))
            && vector2D.f3(new wod.V.vector2D(1,2),new wod.V.vector2D(3,3),new wod.V.vector2D(5,6)).equals(new wod.V.vector2D(3,11/3));
    });
    test("wod.V.vector2D f4 向量共线",function () {
        return vector2D.f4(new wod.V.vector2D(1,2),new wod.V.vector2D(3,4))===false
            && vector2D.f4(new wod.V.vector2D(1,2),new wod.V.vector2D(3,3)) === false
            && vector2D.f4(new wod.V.vector2D(1,2),new wod.V.vector2D(2,4)) === true;
    });
    test("wod.V.vector2D f5 向量垂直",function () {
        return vector2D.f5(new wod.V.vector2D(1,2),new wod.V.vector2D(3,4)) ===false
            && vector2D.f5(new wod.V.vector2D(1,2),new wod.V.vector2D(3,3)) === false
            && vector2D.f5(new wod.V.vector2D(3,4),new wod.V.vector2D(-4,3)) === true
            && vector2D.f5(new wod.V.vector2D(1,2),new wod.V.vector2D(2,4)) === false;
    });
})(wod.V.vector2D);
