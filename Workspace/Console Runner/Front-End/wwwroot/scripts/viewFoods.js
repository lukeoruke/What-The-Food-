// Global vars for managing pagination
var page = 0;
var getFoodFailed = false;


// Call on pageload - get the 0th page of foods and display them
async function pageLoad() {
    const data = await getFoods();
    displayFoods(data);
}

/**
 * Displays the fills the content div with food items from foodList
 * @param {any} foodList - The array of food items to display.
 */
function displayFoods(foodList) {
    // clear the current page of food items
    deleteCurrentData();

    for (data in foodList) {
        // create a new div for each food item in the list, create an anchor tag with the item's name
        // anchor onclick calls getAndDisplayUpdates with the respective food item's barcode
        var itemDiv = document.createElement("div");
        itemDiv.id = foodList[data].Barcode.toString();
        itemDiv.classList.add('foodItemEntry');
        var text = document.createElement("a");
        text.href = "#";
        var textAsHeader = document.createElement("h1");
        textAsHeader.appendChild(text);
        text.setAttribute("onclick", `getAndDisplayUpdates('${foodList[data].Barcode}');`);
        var name = document.createTextNode(foodList[data].ProductName);
        text.appendChild(name);
        itemDiv.appendChild(textAsHeader);
        document.getElementById('container').appendChild(itemDiv);
    }
}

/**
 * Fetch a page of food items from the backend.
 */
async function getFoods() {
    const response = await fetch('http://47.151.24.23:49202/api/ViewFoodItems?' + new URLSearchParams({
        pageno: page
    }));
    const data = await response.json();
    // if fetched data is not a JSON object or is an empty array, then return null
    if (data === undefined || data.length === 0) {
        getFoodFailed = true;
        return Promise.resolve(null);
    }
    else {
        return Promise.resolve(data);
    }
}

/**
 * Increment page number and attempt to get foods for that page.
 * @param {any} e - The calling element, to prevent default behavior of.
 */
async function loadNextPage(e) {
    e.preventDefault();
    page++;
    const foods = await getFoods();
    if (getFoodFailed) {
        page--;
        pageAlert("Could not reach the desired page.");
        getFoodFailed = false;
    }
    else {
        displayFoods(foods);
    }
}

/**
 * Decrement page number and attempt to get foods for that page.
 * @param {any} e - The calling element, to prevent default behavior of.
 */
async function loadPreviousPage(e) {
    e.preventDefault();
    if (page > 0) {
        page--;
        var foods = await getFoods();
        if (getFoodFailed) {
            page++;
            pageAlert("Could not reach the desired page.");
            getFoodFailed = false;
        }
        else {
            displayFoods(foods);
        }
    }
    else {
        pageAlert("There are no more previous pages.");
    }
}


/**
 * Empty the current page of food items to prep for next page.
 */
function deleteCurrentData() {
    var parent = document.getElementById('container');
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
}

/**
 * Add a message on the page to alert the user.
 * @param {string} - The string to display to the user.
 */
function pageAlert(message) {
    document.getElementById('pageAlert').innerHTML = message;
}

/** 
 * If the div has not been expanded, get the div's respective updates and display them inside the div.
 * If the div has already been expanded, empty it.
 */
async function getAndDisplayUpdates(clickedDivId) {
    let clickedDiv = document.getElementById(clickedDivId);
    // if the div has more than 1 child, then it contains updates.
    if (clickedDiv.childElementCount > 1) {
        while (clickedDiv.childElementCount > 1) {
            clickedDiv.removeChild(clickedDiv.lastChild);
        }
    }
    // the div is not showing updates yet - get and display its updates
    else {
        // fetch updates from the backend and parse the json into an object
        const fetchData = await fetch("http://47.151.24.23:49202/api/GetUpdatesFromBarcode?" + new URLSearchParams({
            barcode: clickedDivId
        }));
        const dataAsObject = await fetchData.json();
        // create an element to add to the div
        let message = document.createElement('h2');
        // if the fetched response is not json or is an empty array, then just use this message
        if (dataAsObject === undefined || dataAsObject.length === 0) {
            message.innerHTML = 'No updates are available for this food item.';
        }
        else {
            message = convertUpdateListToHtml(dataAsObject);
        }
        clickedDiv.appendChild(message);
    }
}
/* Updates are structured like so:
 * {
 *  "UpdateType":"FoodIngredientChange",
 *  "UpdateInfo":
 *     {
 *      (Update type-specific parameters i.e. IngredientUpdates or Locations/LotNumbers/ExpirationDates),
 *      "UpdateTime":"2022-04-26T00:00:00",
 *      "Message":"ing change for monster"
 *     }
 * }
 */ 


/**
 * Returns a div containing a div for each update object in updateList.
 * Each inner div contains its date and message, as well as its specific update info
 * @param {Update[]} updateList - An array of update objects parsed from the backend.
 */
function convertUpdateListToHtml(updateList) {
    let updateListAsHtml = document.createElement("div");
    updateList.forEach(update => {
        // date and message are common across all food update types
        let updateHeader = document.createElement("div");
        // add update date
        let updateDate = document.createElement("h2");
        updateDate.innerHTML = `Update on ${getDateOnly(update.UpdateInfo.UpdateTime)}:`;
        updateHeader.appendChild(updateDate);
        // add update message
        let updateMessage = document.createElement("h2");
        updateMessage.innerHTML = update.UpdateInfo.Message;
        updateHeader.appendChild(updateMessage);
        // if the update is a FoodIngredientChange, then it has property IngredientUpdates
        if (update.UpdateType === "FoodIngredientChange") {
            let addIngHeader = document.createElement("h3");
            addIngHeader.innerHTML = "Added Ingredients:";
            // get only the ingredients that were added and parse into an HTML list
            let addedIngredients = update.UpdateInfo.IngredientUpdates.filter(ing => ing.IsAdded);
            let addIngList = getHtmlListOrErrorMessage(addedIngredients, ing => ing.IngredientName);

            let remIngHeader = document.createElement("h3");
            remIngHeader.innerHTML = "Removed Ingredients:";
            // get only the ingredients that were removed and parse into an HTML list
            let removedIngredients = update.UpdateInfo.IngredientUpdates.filter(ing => !ing.IsAdded);
            let remIngList = getHtmlListOrErrorMessage(removedIngredients, ing => ing.IngredientName);

            updateHeader.appendChild(addIngHeader);
            updateHeader.appendChild(addIngList);
            updateHeader.appendChild(remIngHeader);
            updateHeader.appendChild(remIngList);
        }
        // if the update is a FoodRecall, then it has property Locations, LotNumbers, and ExpirationDates
        else if (update.UpdateType === "FoodRecall") {
            let locationHeader = document.createElement("h3");
            locationHeader.innerHTML = "Locations:";
            let locationList = getHtmlListOrErrorMessage(update.UpdateInfo.Locations);
            updateHeader.appendChild(locationHeader);
            updateHeader.appendChild(locationList);

            let lotNumberHeader = document.createElement("h3");
            lotNumberHeader.innerHTML = "Lot Numbers:";
            let lotNumberList = getHtmlListOrErrorMessage(update.UpdateInfo.LotNumbers);
            updateHeader.appendChild(lotNumberHeader);
            updateHeader.appendChild(lotNumberList);

            let expDateHeader = document.createElement("h3");
            expDateHeader.innerHTML = "Expiration Dates:";
            let expDateList = getHtmlListOrErrorMessage(update.UpdateInfo.ExpirationDates, exp => getDateOnly(exp));
            updateHeader.appendChild(expDateHeader);
            updateHeader.appendChild(expDateList);
        }
        updateListAsHtml.appendChild(updateHeader);
    });
    return updateListAsHtml;
}

/**
 * Takes a date-time string formatted by .NET JsonSerializer.Serialize and parse it to only get the date.
 * @param {string} dateTimeString - The DateTime string to get the date from.
 */
function getDateOnly(dateTimeString) {
    return dateTimeString.split('T')[0];
}

/**
 * Returns either: a <ul> element containing each element of the mapping of elementList with propertyMapping as <li> elements,
 * or a <p> element containing "None found." if elementList is undefined or empty.
 * @param {any} elementList - The list of objects to map and convert into li elements of the unordered list.
 * @param {function} propertyMapping - The anonymous function to map elementList with. Default returns the object itself (i.e. use if elementList is an array of strings).
 */
function getHtmlListOrErrorMessage(elementList, propertyMapping = (obj => obj)) {
    // if elementList is undefined or an empty array, return an error message instead.
    if (elementList === undefined || elementList.length == 0) {
        listHead = document.createElement("p");
        listHead.innerHTML = "None found.";
        return listHead;
    }
    // elementList is a non-empty array
    else {
        let listHead = document.createElement("ul");
        // map elementList to list of <li>s containing the desired properties
        propertyList = elementList.map(el => {
            let listItem = document.createElement("li");
            listItem.innerHTML = propertyMapping(el).toString();
            return listItem;
        });
        // append <li>s to <ul> and return
        propertyList.forEach(property => listHead.appendChild(property));
        return listHead;
    }
}