var articleFields = [
    { name: "name", type: "wodtext" },
    { name: "description", type: "wodtext" },
    { name: "keywords", type: "wodtext" },
    { name: "category", type: "woddrop", optionscmd: "op_category" },
    { name: "code", type: "wodtext" },
    { name: "content", type: "wodrichtext" },
    { name: "preContent", type: "wodrichtext", setting: { type: "simple"} },
    { name: "page", type: "wodtext" },
    { name: "image", type: "wodsiteImage" },
    { name: "extendData", type: "wodtext" }
];
var articleValidate = function (data, errorCallback) {
    return true;
};