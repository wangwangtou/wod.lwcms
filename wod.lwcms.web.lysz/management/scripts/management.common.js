var articleFields = [
    { name: "name", type: "wodtext" },
    { name: "description", type: "wodtext" },
    { name: "keywords", type: "wodtext" },
    { name: "category", type: "woddrop", optionscmd: "op_category" },
    { name: "code", type: "wodtext" },
    { name: "content", type: "wodrichtext" },
    { name: "preContent", type: "wodtext" },
    { name: "page", type: "wodtext" },
    { name: "image", type: "wodsiteImage" },
    { name: "extendData", type: "wodtext" }
];
var articleValidate = function (data, errorCallback) {
    return true;
};
var naviFields = [
    { name: "name", type: "wodtext" },
    { name: "title", type: "wodtext" },
    { name: "naviUrl", type: "wodtext" },
    { name: "target", type: "woddrop",
        options: [
            { name: "当前窗口", value: "_self" },
            { name: "新窗口", value: "_blank" }
            ] },
];
var naviValidate = function (data, errorCallback) {
    return true;
};
var categoryFields = [
    { name: "name", type: "wodtext" },
    { name: "code", type: "wodtext" },
    { name: "description", type: "wodtext" },
    { name: "content", type: "wodrichtext", richtype: "simple" },
    { name: "keywords", type: "wodtext" },
    { name: "page", type: "wodtext" },
    { name: "contentpage", type: "wodtext" },
    { name: "extendform", type: "wodtext" }
];
var categoryValidate = function (data, errorCallback) {
    return true;
};
var siteBaseFields = [
    { name: "siteName", type: "wodtext" },
    { name: "siteUrl", type: "wodtext" },
    { name: "description", type: "wodtext" },
    { name: "copyright", type: "wodrichtext", richtype: "simple" },
    { name: "keywords", type: "wodtext" },
    { name: "title", type: "wodtext" },
    { name: "navis", type: "wodnavitree" ,
        names: { "top": "顶部导航", "bottom": "底部导航", "main": "主导航" },
        formname: "naviform", formtitle: "导航", titleprop: "name", childrenprop: "subNavis",
        validate: naviValidate,
        fields: naviFields
    },
    { name: "allCats", type: "wodtreesubform",
        formname: "catform", formtitle: "分类", titleprop: "name", childrenprop: "subCategory",
        validate: categoryValidate,
        fields: categoryFields
    }
];
var siteBaseValidate = function (data, errorCallback) {
    return true;
};