console.log("TOP OF FILE");
var checkBoxList = [];
var getIDs = [];
var searching = false;
var search;
var page = "0";
var currentPage = "default";

//Initalization of the page, creates the various check boxes and populates the information
async function addFlagCheckBoxes() {

    await getIngs();
    displayIngs();
}

//Displays the page content with information recieved from a json that was kept in local storage
function displayIngs() {

    var jsonData = localStorage.getItem('allIngredients');
    

    const jsonConst = JSON.parse(jsonData);

    var getNames = jsonConst.IngredientName;
    getIDs = jsonConst.IngredientID;


    //gets the names of each ingredient
    for (data in getNames) {


        var text = "Ingredient Name: " + getNames[data];

        var label = document.createElement("data"); 
        var description = document.createTextNode(text);

        var checkbox = document.createElement("input");
        checkbox.id = getIDs[data];

        checkbox.type = "checkbox";
        checkbox.name = "box" + data;
        checkbox.value = description;
        label.appendChild(checkbox);
        label.appendChild(description);
        label.id = data * 100;



        document.getElementById('container').appendChild(label);
        document.getElementById('container').innerHTML += "<br/>";

        checkBoxList[data] = checkbox;
    }
}

//gets the ingredients from the DB
async function getIngs() {

    await fetch('http://47.151.24.23:49202/api/GetNIngredients?' + page)
        .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
        .then(data => console.log(data));
}

async function getUserFlagButtonPressed(e) {
    page = "0";

    getUserFlags(e);
}

//retrieves the flags associated with our current user
async function getUserFlags(e) {
    e.preventDefault();
   
    currentPage = "displayFlags";


    var btn = document.getElementById("viewFlagsLabel");
    btn.value = "return to add flags page";
    btn = document.getElementById("viewFlags");
    btn.onsubmit = function () { returnToAddFlags() };

    var btn2 = document.getElementById("updateFlagsLabel");
    btn2.value = "Remove Flag";


    var btn4 = document.getElementById("searchTypeLabel");
    btn4.value = "Search Your Flags";


    deleteCurrentData(e);
    await fetch('http://47.151.24.23:49202/api/GetNAccountFlags?' + page)
        .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
        .then(data => console.log(data));



    displayIngs();

}

//Uses a user provided string as a search param to look through the users flags
async function searchAccountFlags(e) {
    e.preventDefault();
    try {
        deleteCurrentData(e);
        currentPage = "searchFlags";
        let search = document.getElementById('search').value;

        await fetch('http://47.151.24.23:49202/api/GetAccountFlagBySearch?' + search + "?" + page)
            .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
            .then(data => console.log(data));
        console.log("made it to the otherside of searchAccountFlags");

        displayIngs();
    } catch (ex) {
        console.log("searchAccountFlags: " + ex);
        throw ex;
    }

}

//uses a user provided string to search through ingredients
async function searchIngs(e) {
    e.preventDefault();
    

    deleteCurrentData(e);
    currentPage = "searchIngredients";
    let search = document.getElementById('search').value;

    await fetch('http://47.151.24.23:49202/api/FlagSearchIngredients?' + search + "?" + page)
        .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
        .then(data => console.log(data));

    displayIngs();
}
async function searchButtonPressed(e) {
    if (currentPage == "displayFlags") {

        searchAccountFlags(e);
        return;
    }
    page = "0";
    searching = true;
    currentPage = "searchIngredients";
    var btn = document.getElementById("viewFlagsLabel");
    btn.value = "Return to start";
    btn = document.getElementById("viewFlags");
    btn.onsubmit = function () { getUserFlagButtonPressed(e) };

    var btn2 = document.getElementById("updateFlagsLabel");
    btn2.value = "Add Flags";

    await searchIngs(e);
}

//returns to the default state of the page
async function returnToAddFlags() {

    page = "0";
    currentPage = "default";
    deleteCurrentData();

    getIngs();
    displayIngs();
    return;
}


async function updateFlagsButtonPressed(e) {
    e.preventDefault();
    if (currentPage == "displayFlags") {
        removeFlag(e);
    } else if (currentPage == "default") {
        sendNewFlag(e);
    } else if (currentPage == "searchIngredients") {
        sendNewFlag(e);
    } else if (currentPage == "searchFlags") {
        removeFlag(e);
    } else{

        throw ("CurrentPage is not one of the values it is allowed to take.: Currently = " + currentPage);
    }
}

//adds flag(s) to the db
async function sendNewFlag(e) {
    e.preventDefault();

    const itemsToAdd = [];
    var counter = 0;
    for (data in checkBoxList) {

        if (document.getElementById(getIDs[data]).checked) {
            itemsToAdd[counter] = getIDs[data];
            counter += 1;
        }
    }
    alert("Flag(s) added to your account!");



    await fetch('http://47.151.24.23:49202/api/AccountAddFlags', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Origin': '*',
        },
        body: (itemsToAdd),
    })

}

//removes flag(s) from the db
async function removeFlag(e) {
    e.preventDefault();

    const itemsToRemove = [];
    var counter = 0;
    for (data in checkBoxList) {

        if (document.getElementById(getIDs[data]).checked) {
            itemsToRemove[counter] = getIDs[data];
            counter += 1;
        }
    }

    await fetch('http://47.151.24.23:49202/api/AccountRemoveFlag?' + page, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Origin': '*',
        },
        body: (itemsToRemove),
    })
    alert("Flag(s) removed from your account");

    //returnToAddFlags();
    getUserFlagButtonPressed(e);

}


//Deletes the current Data being displayed on the page
function deleteCurrentData() {

    var parent = document.getElementById('container');
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
    checkBoxList = [];
}

//used to navigate forward through lists of ingredients or flags
async function loadnextPage(e) {
    e.preventDefault();
    deleteCurrentData(e);

    var pageNumber = parseInt(page);
    pageNumber += 1;
    page = String(pageNumber);

    if (currentPage == "default") {
        await getIngs();
        displayIngs();
    } else if (currentPage == "searchIngredients") {
        await searchIngs(e);
    } else if (currentPage == "searchFlags") {
        await searchAccountFlags(e);
    }else if (currentPage == "displayFlags") {
        await getUserFlags(e);
    } else {
        displayIngs();
    }


}
//used to navigate backwards through lists of ingredients or flags
async function loadPreviousPage(e) {
    e.preventDefault();
    deleteCurrentData(e);

    var pageNumber = parseInt(page);
    pageNumber -= 1;
    page = String(pageNumber);
    if (currentPage == "default") {
        await getIngs();
        displayIngs();
    } else if (currentPage == "searchIngredients") {
        await searchIngs(e);
    } else if (currentPage == "searchFlags") {
        await searchAccountFlags(e);
    } else if (currentPage == "displayFlags") {
        await getUserFlags(e);
    } else {
        displayIngs();
    }


}

