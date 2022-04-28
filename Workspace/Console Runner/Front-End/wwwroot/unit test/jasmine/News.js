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