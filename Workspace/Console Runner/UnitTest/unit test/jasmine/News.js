News = function() {};


News.prototype.incrementHealthFilter = function (bias) {
    this.bias = bias + 1;
    return (this.bias);
}
News.prototype.decrementHealthFilter = function (bias) {
    this.bias = bias - 1;
    return (this.bias);
}
News.prototype.storeFoodNews = function (displayAmnt) {
    var array = [];
    for (; displayAmnt < 5; displayAmnt++) {
        array.push("FoodNews" + displayAmnt);
    }
    return true;
}
News.prototype.storeHealthNews = function (displayAmnt) {
    var array = [];
    for (var x = 0; x < displayAmnt; x++) {
        array.push("HealthNews" + x);
    }
    return true;
}
News.prototype.displayNews = function () {
    //populate test samples
    var array = [];
    for (var x = 0; x < 5; x++) {
        array.push("NewsToDisplay" + x);
    }
    //actual test
    var innerText;
    var title;
    var target;

    var innerHTML;
    var headlineText;
    var image;
    var text;
    for (var index = 0; index < 5; index++) {
        var a;
        a = "URL " + index;
        innerText = "Text of the article " + index;
        title = "Title of article " + index;

        var p;
        p = "paragraph "+index
        innerHTML = "Paragraph from " + index;

        image = "image " + index;
        headlineText = a;
        text = innerText;
    }
    return true;
}
