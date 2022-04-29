News = function() {};
/**
 * Select the category of news that will be catered towards the user
 */
News.prototype.pickCategory = function (healthFilterLevel) {
    //if they have no personalization, display food news as default
    //else display personzalized news based on user preference
    if (healthFilterLevel == 0) {
        return "storeFood"
    }
    else {
        return"bothNews"
    }
    //call displayNews in actual
}
/**
 * Store food related news from API into array
 * @param {Number} displayAmnt The number of Food Articles that will be displayed
 */
News.prototype.storeFoodNews = function (displayAmnt) {
    //fetch from FOODAPI and then store
    var array = [];
    for (; displayAmnt < 5; displayAmnt++) {
        array.push("FoodNews" + displayAmnt);
    }
    return true;
}
/**
 * Store health related news from API into array
 * @param {Number} displayAmnt The number of Food Articles that will be displayed
 */
News.prototype.storeHealthNews = function (displayAmnt) {
    //fetch from HealthAPI and then store
    var array = [];
    for (var x = 0; x < displayAmnt; x++) {
        array.push("HealthNews" + x);
    }
    return true;
}
/**
 *  Populates and assigns news relative to html position
 */
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
        //create an anchor element to create hyperlinked html
        var a;
        a = "URL " + index;
        //add headline of URL
        innerText = "Text of the article " + index;
        //add the title of URL
        title = "Title of article " + index;

        //sotres a paragraph from document which will be JSON
        var p;
        p = "paragraph "+index
        innerHTML = "Paragraph from " + index;

        image = "image " + index;
        headlineText = a;
        text = innerText;
    }
    return true;
}
/**
 * Get the category of article clicked by user
 * @param {Number} index The index of article relative to homepage
 */
News.prototype.getCategory = function (index) {
    //input sample dataset
    var array = [];
    array.push("Health");
    array.push("Food");
    array.push("Health");
    array.push("Food");
    array.push("Food");
    //actual function
    var category = array[index];
    //get the category/type of news that is being currently displayed
    if (category == "Food") {
        return "decrement";
    }
    if (category == "Health") {
        return "increment";
    }
}
/**
 * Get the number of Health news to display
 */
News.prototype.getHealthFilter = function () {
    //make a get request to get value of HealthFilter and assign it to response
    var response = 3;
    //get response body as text
    var data = response
    return true;
}
/**
 * Change user preference to have more Health News
 */
News.prototype.incrementHealthFilter = function (bias) {
    //make post request to increment healthFilter
    this.bias = bias + 1;
    return (this.bias);
}
/**
 * Change user preference to have less Health News
 */
News.prototype.decrementHealthFilter = function (bias) {
    //make post request to decrement healthFilter
    this.bias = bias - 1;
    return (this.bias);
}
