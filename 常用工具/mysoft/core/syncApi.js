function sync(album, callback) {
    console.log("----开始同步----");
    console.log(album);
    try {

        callback();
    } catch (e) {
        callback(e);
    }
}
exports.sync = sync;