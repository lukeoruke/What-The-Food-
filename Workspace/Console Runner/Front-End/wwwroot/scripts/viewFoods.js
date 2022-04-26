console.log("TOP OF FILE");
var tryingNextPage = false;
var page = 0;
var getFoodFailed = false;


//Displays the page content with information recieved from a json that was kept in local storage
function displayFoods() {
    // TODO: modify to stop using localStorage
    console.log(page);
    var foodList = JSON.parse(localStorage.getItem('foodList'));
    deleteCurrentData();

    //gets the names of each ingredient
    for (data in foodList) {
        console.log("inside displayFoods loop");
        console.log(foodList[data].ProductName);
        var item = document.createElement("a");
        // TODO: modify to whatthefood for deployment?
        item.href = "http://localhost:49202/api/GetUpdatesFromBarcode?barcode=" + foodList[data].Barcode;
        var name = document.createTextNode(foodList[data].ProductName);
        item.appendChild(name);
        item.appendChild(document.createElement("br"));
        document.getElementById('container').appendChild(item);
    }
}

//gets the ingredients from the DB
async function getFoods() {
    console.log('getFoods page:' + page.toString());
    await fetch('http://localhost:49202/api/ViewFoodItems?' + new URLSearchParams({
        pageno: page
    }))
        .then(response => { return response.json(); })
        .then(async data => {
            if (await data === undefined || data.length === 0) {
                console.log('getfoodfailed');
                console.log(JSON);
                getFoodFailed = true;
            }
            else {
                localStorage.setItem('foodList', JSON.stringify(data));
            }
        });
}

function processUpdateList(jsonData) {
}

//used to navigate forward through lists of ingredients or flags
async function loadNextPage(e) {
    e.preventDefault();
    tryingNextPage = true;
    page++;
    await getFoods();
    if (getFoodFailed) {
        undoNextPage();
        getFoodFailed = false;
    }
    else {
        displayFoods();
    }
}

function undoNextPage() {
    console.log("page was" + page);
    page--;
    alert("Could not reach the desired page.");
    console.log("page is now" + page);
}

//used to navigate backwards through lists of ingredients or flags
async function loadPreviousPage(e) {
    e.preventDefault();
    tryingNextPage = false;
    if (page > 0) {
        page--;
        await getFoods();
        if (getFoodFailed) {
            undoPreviousPage();
            getFoodFailed = false;
        }
        else {
            displayFoods();
        }
    }
    else {
        alert("There are no more previous pages.");
    }
}

function undoPreviousPage() {
    page++;
    alert("Could not reach the desired page.");
}

//Deletes the current Data being displayed on the page
function deleteCurrentData() {

    var parent = document.getElementById('container');
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
}