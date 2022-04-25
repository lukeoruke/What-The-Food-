console.log("TOP OF FILE");
var getIDs = [];
var searching = false;
var search;
var page = 0;


//Displays the page content with information recieved from a json that was kept in local storage
function displayFoods() {
    console.log(page);
    var jsonData = localStorage.getItem('foodList');
    const jsonConst = JSON.parse(jsonData);
    console.log("jsonConst" + jsonConst);
    var getNames = jsonConst.map(item => item.ProductName);
    console.log("getNames" + getNames);
    getIDs = jsonConst.map(item => item.Barcode);

    //gets the names of each ingredient
    for (data in jsonConst) {
        console.log("inside displayFoods loop");
        console.log(jsonConst[data].ProductName);
        var item = document.createElement("a");
        // TODO: modify to whatthefood for deployment?
        item.href = "http://localhost:49202/api/GetUpdatesFromBarcode?barcode=" + jsonConst[data].Barcode;
        var name = document.createTextNode(jsonConst[data].ProductName);
        item.appendChild(name);
        item.appendChild(document.createElement("br"));
        document.getElementById('container').appendChild(item);
    }
}

//gets the ingredients from the DB
async function getFoods() {
    await fetch('http://localhost:49202/api/ViewFoodItems?pageno=' + page.toString())
        .then(async response => localStorage.setItem('foodList', JSON.stringify(await response.json())))
        .then(data => console.log(data));
}

//used to navigate forward through lists of ingredients or flags
async function loadnextPage(e) {
    e.preventDefault();
    deleteCurrentData(e);

    page += 1;
    await getFoods().then(displayFoods());
}
//used to navigate backwards through lists of ingredients or flags
async function loadPreviousPage(e) {
    e.preventDefault();
    deleteCurrentData(e);

    if (page > 0) {
        page -= 1;
        await getFoods().then(displayFoods());
    }
    else {
        alert("There are no more previous pages.");
    }
}

//Deletes the current Data being displayed on the page
function deleteCurrentData() {

    var parent = document.getElementById('container');
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
}