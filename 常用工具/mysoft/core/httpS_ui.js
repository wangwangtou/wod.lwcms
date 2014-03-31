console.log("httpS_ui.js loaded!");
var url = require("url");
var fs = require('fs');
var path = require('path');
var common = require('./common.js');

function responseFile(res, fp) {
    fs.readFile(fp, "binary", function (err, file) {
        if (err) {
            res.writeHead(500, {
                'Content-Type': 'text/plain'
            });
            res.end(err);
        } else {
            var ext = path.extname(fp);
            ext = ext ? ext.slice(1) : 'unknown';

            res.writeHead(200, {
                'Content-Type': common.sdata("mimes")[ext] || "text/plain"
            });
            res.write(file, "binary");
            res.end();
        }
    });
}
exports.process = function (req, res) {
    var pathname = url.parse(req.url).pathname;
    var realPath = "." + pathname;
    fs.exists(realPath, function (exists) {
        if (exists) {
            responseFile(res, realPath);
        }
        else {
            console.log(realPath + " is not exists!");

            res.writeHead(404, {
                'Content-Type': 'text/plain'
            });
            res.write("This request URL " + pathname + " was not found on this server.");
            res.end();
        }
    });
};