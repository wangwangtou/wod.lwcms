var path = require("path");
var fs = require("fs");
function getPath(rpath) {
    var corePath = module.paths[0];
    corePath = corePath.substring(0, corePath.length - "node_modules".length);
    return path.join(corePath, rpath);
}
function ncRequire(rpath) {
    delete require.cache[getPath(rpath)];
    return require(rpath);
}
function data(name) {
    return ncRequire("../data/" + name + ".js")["data"];
}
function writeData(name, data,callback) {
    var strData = "exports.data = " + JSON.stringify(data) + ";";
    fs.writeFile(getPath("../data/" + name + ".js"), strData, function (err) {
        if (err) console.log(err);
        else {
        }
        if (callback) callback(err);
    });
}
function sdata(name) {
    return ncRequire("./settings/" + name + ".js")[name];
}
function extend(source, target) {
    for (var i in target) {
        source[i] = target[i];
    }
    return source;
}
extend(exports, {
    ncRequire: ncRequire,
    data: data,
    sdata: sdata,
    writeData:writeData,
    extend:extend
});