console.log("window/httpS_api.js loaded!");
var exec = require("child_process").exec;
var fs = require("fs");
var path = require("path");
var common = require("../common.js");

var apis = {
    upload: function (resHelper, args) {
        if (args && args.length) {
            var dataName = args[0];
            var callback = function (err) {
                if (err)
                    resHelper.write500();
                else {
                    resHelper.writeJson({ result: true });
                }
            };
            var d = common.data(dataName);
            var syncApi = require("../syncApi.js");
            syncApi.sync(d, callback);
        }
    },
    setdata: function (resHelper, args) {
        if (args && args.length) {
            var dataName = args[0];
            var callback = function (err) {
                if (err)
                    resHelper.write500();
                else {
                    resHelper.writeJson({ result: true });
                }
            };
            resHelper.getPost("jsonstring", function (paramData) {
                console.log(paramData);
                var data = JSON.parse(paramData);
                common.writeData(dataName, data, callback);
            });
        }
    },
    getdata: function (resHelper, args) {
        if (args && args.length) {
            var data = args[0];
            console.log(data);
            try {
                var d = common.data(data);
                resHelper.writeJson(d);
            } catch (e) {
                resHelper.write500();
            }
        }
    },
    imgres: function (resHelper, args) {
        if (args && args.length) {
            var img = args[0];
            console.log(img);
            fs.exists(img, function (exists) {
                if (exists) {
                    fs.readFile(img, "binary", function (err, file) {
                        if (err) {
                            resHelper.write500();
                        } else {
                            var ext = path.extname(img).substring(1);
                            resHelper.res.writeHead(200, {
                                'Content-Type': common.sdata("imgmimes")[ext] || "text/plain"
                            });
                            resHelper.res.write(file, "binary");
                            resHelper.res.end();
                        }
                    });
                }
                else {
                    resHelper.write500();
                }
            });
        }
    },
    getdir: function (resHelper, args) {
        var wr = resHelper.writeJson;
        if (args && args.length) {
            console.log(args[0]);
            fs.readdir(args[0], function (err, paths) {
                if (err) {
                    console.log(err);
                    wr([]);
                } else {
                    for (var i = 0; i < paths.length; i++) {
                        var rp = path.join(args[0], paths[i]);
                        console.log(rp);
                        try {
                            var stat = fs.lstatSync(rp);
                            paths[i] = { type: stat.isDirectory() ? "folder" : path.extname(rp).substring(1), name: paths[i], fullpath: rp };
                        } catch (e) {
                            paths[i] = { type: "error", name: paths[i], fullpath: rp };
                        }
                    }
                    wr(paths);
                }
            });
        }
        else {
            var cmd = 'fsutil fsinfo drives';
            exec(cmd, function (err, stdout, stderr) {
                if (err) {
                    console.log(err);
                    wr([]);
                }
                else {
                    console.log('stdout data');
                    console.log(stdout);
                    console.log('stderr data');
                    console.log(stderr);

                    var result = stdout.trim().split(" ");
                    result.shift();
                    for (var i = 0; i < result.length; i++) {
                        result[i] = { type: "folder", name: result[i], fullpath: result[i] };
                    }
                    wr(result);
                }
            });
        }
    }
};
var url = require("url");
var querystring = require("querystring");
exports.process = function (req, res) {
    var pathname = url.parse(req.url).pathname;
    var apicmd = pathname.substring("/api/".length);
    var args = apicmd.split("/");
    var api = args.shift();
    if (apis[api]) {
        for (var i = 0; i < args.length; i++) {
            args[i] = decodeURI(args[i]);
        }
        apis[api]({
            req: req,
            res: res,
            getPost: function (param, callback) {
                //var __post = this;
                //if (__post.__postData) {
                //}
                var postData = "";
                req.addListener('data', function (postDataChunk) {
                    postData += postDataChunk;
                });
                req.addListener('end', function () {
                    console.log("end");
                    var paramData = querystring.parse(postData)[param];
                    callback(paramData && decodeURI(paramData));
                });
            },
            writeJson: function (result) {
                res.writeHead(200, {
                    'Content-Type': "text/plain"
                });
                var str = JSON.stringify(result);
                res.write(str);
                res.end();
            },
            write500: function () {
                res.writeHead(500, {
                    'Content-Type': "text/plain"
                });
                res.end();
            }
        }, args);
    }
    else {
        res.writeHead(500, {
            'Content-Type': 'text/plain'
        });
        res.end();
    }
};
exports.open = function (url) {
    exec("explorer \""+url+"\"");
};