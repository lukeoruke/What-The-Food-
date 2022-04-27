var page = 0;
var getFoodFailed = false;


async function pageLoad() {
    const data = await getFoods();
    displayFoods(data);
}

//Displays the page content with information recieved from a json that was kept in local storage
function displayFoods(foodList) {
    deleteCurrentData();

    //gets the names of each ingredient
    for (data in foodList) {
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

//gets the ingredients from the DB
async function getFoods() {
    const response = await fetch('http://localhost:49202/api/ViewFoodItems?' + new URLSearchParams({
        pageno: page
    }));
    const data = await response.json();
    if (data === undefined || data.length === 0) {
        getFoodFailed = true;
        return Promise.resolve(null);
    }
    else {
        return Promise.resolve(data);
    }
}

function processUpdateList(jsonData) {
}

//used to navigate forward through lists of ingredients or flags
async function loadNextPage(e) {
    e.preventDefault();
    page++;
    const foods = await getFoods();
    if (getFoodFailed) {
        undoNextPage();
        getFoodFailed = false;
    }
    else {
        displayFoods(foods);
    }
}

function undoNextPage() {
    page--;
    pageAlert("Could not reach the desired page.");
}

//used to navigate backwards through lists of ingredients or flags
async function loadPreviousPage(e) {
    e.preventDefault();
    if (page > 0) {
        page--;
        var foods = await getFoods();
        if (getFoodFailed) {
            undoPreviousPage();
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

function undoPreviousPage() {
    page++;
    pageAlert("Could not reach the desired page.");
}

//Deletes the current Data being displayed on the page
function deleteCurrentData() {

    var parent = document.getElementById('container');
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
}

function pageAlert(message) {
    document.getElementById('pageAlert').innerHTML = message;
}

async function getAndDisplayUpdates(clickedDivId) {
    let clickedDiv = document.getElementById(clickedDivId);
    if (clickedDiv.childElementCount > 1) {
        while (clickedDiv.childElementCount > 1) {
            clickedDiv.removeChild(clickedDiv.lastChild);
        }
    }
    else {
        const fetchData = await fetch("http://localhost:49202/api/GetUpdatesFromBarcode?" + new URLSearchParams({
            barcode: clickedDivId
        }));
        const dataAsObject = await fetchData.json();
        let message = document.createElement('h2');
        if (dataAsObject === undefined || dataAsObject.length === 0) {
            message.innerHTML = 'No updates are available for this food item.';
        }
        else {
            message = convertUpdateListToHtml(dataAsObject);
        }
        clickedDiv.appendChild(message);
    }
}

function convertUpdateListToHtml(updateList) {
    let updateListAsHtml = document.createElement("div");
    updateList.forEach(update => {
        // date and reason are common across all food update types
        let updateHeader = document.createElement("div");
        /*updateHeader.style.outline = "red";
        updateHeader.style.outlineWidth = "1";*/
        // add update date
        let updateDate = document.createElement("h2");
        updateDate.innerHTML = `Update on ${getDateOnly(update.UpdateInfo.UpdateTime)}:`;
        updateHeader.appendChild(updateDate);
        // add update message
        let updateMessage = document.createElement("h2");
        updateMessage.innerHTML = update.UpdateInfo.Message;
        updateHeader.appendChild(updateMessage);

        if (update.UpdateType === "FoodIngredientChange") {
            console.log(`update type is FoodIngredientChange: ${update.UpdateType}`);
            console.log(update.UpdateInfo.IngredientUpdates);

            let addIngHeader = document.createElement("h3");
            addIngHeader.innerHTML = "Added Ingredients:";
            let addedIngredients = update.UpdateInfo.IngredientUpdates.filter(ing => ing.IsAdded);
            let addIngList = getHtmlListOrErrorMessage(addedIngredients, ing => ing.IngredientName);

            let remIngHeader = document.createElement("h3");
            remIngHeader.innerHTML = "Removed Ingredients:";
            let removedIngredients = update.UpdateInfo.IngredientUpdates.filter(ing => !ing.IsAdded);
            let remIngList = getHtmlListOrErrorMessage(removedIngredients, ing => ing.IngredientName);

            updateHeader.appendChild(addIngHeader);
            updateHeader.appendChild(addIngList);
            updateHeader.appendChild(remIngHeader);
            updateHeader.appendChild(remIngList);
        }
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

function getDateOnly(dateTimeString) {
    return dateTimeString.split('T')[0];
}


function getHtmlListOrErrorMessage(elementList, propertyMapping = (obj => obj)) {
    console.log('elementList: ' + elementList);
    elementList.forEach(el => console.log(el));
    if (elementList === undefined || elementList.length == 0) {
        listHead = document.createElement("p");
        listHead.innerHTML = "None found.";
        return listHead;
    }
    else {
        let listHead = document.createElement("ul");
        propertyList = elementList.map(el => {
            let listItem = document.createElement("li");
            console.log('appending ' + propertyMapping(el).toString());
            listItem.innerHTML = propertyMapping(el).toString();
            return listItem;
        });
        propertyList.forEach(property => listHead.appendChild(property));
        return listHead;
    }
}