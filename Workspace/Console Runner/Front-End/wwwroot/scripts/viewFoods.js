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
        var anchor = document.createElement("a");
        // TODO: modify to whatthefood for deployment?
        anchor.href = "http://localhost:49202/api/GetUpdatesFromBarcode?barcode=" + foodList[data].Barcode;
        var name = document.createTextNode(foodList[data].ProductName);
        anchor.appendChild(name);
        itemDiv.appendChild(anchor);
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

function getUpdates(barcode) {

}