(function (window, body) {
    var trp = window.document.createElement("div");
    if (body) {
        body.appendChild(trp);
    }
    else {
        window.addEventListener("load", function () {
            window.document.body.appendChild(trp);
        });
    }
    function writeResult(panel, msg) {
        var result = window.document.createElement("div");
        result.innerHTML = encodeHTML(msg);
        panel.appendChild(result);
    }
    window.test = function (item, arg) {
        var itemPanel = window.document.createElement("div");
        itemPanel.innerHTML = "<p>" + encodeHTML(item) + "</p>";
        trp.appendChild(itemPanel);
        var msgPanel = window.document.createElement("div");
        itemPanel.appendChild(msgPanel);
        if (typeof (arg) == "boolean") {
        }
        else if (typeof (arg) == "function") {
            try {
                arg = arg(function (msg) {
                    writeResult(msgPanel, msg);
                }, function (result,msg) {
                    if (!result) {
                        throw new Error(msg);
                    }
                });
            } catch (e) {
                arg = false;
                writeResult(msgPanel, e);
            }
        }
        writeResult(msgPanel, arg ? "成功" : "失败");
        if (arg) {
            itemPanel.style["background"] = "#0f0";
        }
        else {
            itemPanel.style["background"] = "#f00";
        }
    };
    window.testWithUI = function (item,ui,confirmToTest,autoConfirm) {
        var itemPanel = window.document.createElement("div");
        itemPanel.innerHTML = "<p>" + encodeHTML(item) + "</p>";
        trp.appendChild(itemPanel);
        var msgPanel = window.document.createElement("div");
        itemPanel.appendChild(msgPanel);

        msgPanel.appendChild(ui);

        var setResult = function (result) {
            writeResult(msgPanel, result ? "成功" : "失败");
            if (result) {
                itemPanel.style["background"] = "#0f0";
            }
            else {
                itemPanel.style["background"] = "#f00";
            }
        }
        if (!autoConfirm) {
            var confirmBar = window.document.createElement("div");
            confirmBar.innerHTML = "<button type='button'>确定</button><button type='button'>失败</button>";
            confirmBar.getElementsByTagName("button")[0].onclick = function () {
                msgPanel.removeChild(confirmBar);
                setResult(confirmToTest(function (msg) {
                    writeResult(msgPanel, msg);
                }));
            };
            confirmBar.getElementsByTagName("button")[1].onclick = function () {
                msgPanel.removeChild(confirmBar);
                setResult(false);
            };
            msgPanel.appendChild(confirmBar);
        }
        else {
            setResult(confirmToTest(function (msg) {
                writeResult(msgPanel, msg);
            }));
        }
    };
})(window, window.document.body);