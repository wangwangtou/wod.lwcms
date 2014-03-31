var http = require("http");
var fs = require("fs");
var url = require("url");
var querystring = require("querystring");
var common = require("./core/common.js");


exports = new uploadApi();

function uploadApi() {
}

uploadApi.prototype.uploadBuffer = function (stream, filename, setting, callback) {
    var defSetting = {
        url: "",
        auth: "wangx"
    };
    setting = common.extend(defSetting, setting);

    // set content length and other values to the header
    var header = {
        "Authorization": setting.auth,
        "Content-Type":"application/x-www-form-urlencoded",//使用Form的方式提交，在Asp.Net中才能解析到Form中
        "Accept":"*/*"
    };

    var data = { fn: filename, buffer: new Buffer(0) };

    // set request options
    var options = common.extend(url.parse(setting.url), {
        method: 'POST',
        headers: header
    });

    stream.on("data", function (chunk) {
        data.buffer = Buffer.concat([data.buffer, new Buffer(chunk)]);
    });
    stream.on("end", function (chunk) {
        if (chunk) {
            data.buffer = Buffer.concat([data.buffer, new Buffer(chunk)]);
        }
        data.buffer = data.buffer.toString("base64");
        var towrite = querystring.stringify(data);

        var wookieResponse = "";
        options.headers["content-length"] = String(towrite.length);
        // create http request
        var requ = http.request(options, function (res) {
            res.setEncoding('utf8');
            res.on('data', function (chunk) {
                wookieResponse = wookieResponse + chunk;
            });
            res.on('end', function (chunk) {
                wookieResponse = wookieResponse + chunk;
                callback(null, wookieResponse);
            })
        });
        requ.on('error', function (e) {
            console.log("请求出错！");
            console.log(e);
            callback(e);
        });

        requ.write(towrite);
        requ.end();
    });
    stream.read();
};

var api = exports;

var fileName = "f:\\电脑桌面更新用(1600X1200).jpg";

fs.stat(fileName, function (err, fi) {
    var fileStream = fs.createReadStream(fileName);

    api.uploadBuffer(fileStream, fileName, {}, function (err,res) {
        console.log("成功！");
    });
});