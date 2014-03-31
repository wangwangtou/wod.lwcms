var ui = require("./httpS_ui.js");
var api = require("./windows/httpS_api.js");
var path = require("path");

var url = require("url");
var processSelector = function (req) {
    var pathname = url.parse(req.url).pathname;
    console.log(pathname);
    if (pathname.indexOf("/ui") == 0) {
        //console.log(path.join(process.cwd(), "./core/httpS_ui.js"));
        return require("./httpS_ui.js");
    }
    else if (pathname.indexOf("/api") == 0) {
        return require("./windows/httpS_api.js");
    }
    else if (pathname.indexOf("/reload") == 0) {
        var proPath = process.cwd();
        delete require.cache[path.join(proPath, "./core/windows/httpS_api.js")];
        delete require.cache[path.join(proPath, "./core/httpS_ui.js")];
        return {
            process: function (req, res) {
                console.log("Modules reload success!");
                res.writeHead(200, {
                    'Content-Type': 'text/plain'
                });
                res.write("{\"result\" : true }");
                res.end();
            }
        };
    }
    else {
        return {
            process: function (req, res) {
                console.log(pathname + " is not exists!");
                res.writeHead(404, {
                    'Content-Type': 'text/plain'
                });
                res.write("This request URL " + pathname + " was not found on this server.");
                res.end();
            }
        };
    }
};

var http = require("http");
var server=new http.Server();

server.on("request",function (req, res) {
    processSelector(req).process(req, res);
});
server.listen(8762);
console.log("已经打开");

api.open("http://127.0.0.1:8762/ui/index.html");
