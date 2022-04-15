console.log("TOP OF FILE");
var checkBoxList = [];
var getIDs = [];
var searching = false;
var search;
var page = "0";
var currentPage = "default";

async function AddFlagCheckBoxes() {
    console.log(currentPage);
    console.log("ADD FLAG CHECK BOXES FUNCTION STARTING");
    await getIngs();

    
    displayIngs();
}

function displayIngs() {

    var jsonData = localStorage.getItem('allIngredients');
    

    const jsonConst = JSON.parse(jsonData);

    var getNames = jsonConst.IngredientName;
    getIDs = jsonConst.IngredientID;


    console.log(jsonConst);

    // create the necessary elements

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


        // add the label element to your div
        document.getElementById('container').appendChild(label);
        document.getElementById('container').innerHTML += "<br/>";

        checkBoxList[data] = checkbox;
    }
}


async function getIngs() {

    await fetch('http://localhost:49200/api/GetNIngredients?' + page)
        .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
        .then(data => console.log(data));
}

async function getUserFlagButtonPressed(e) {
    page = "0";

    getUserFlags(e);
}

async function getUserFlags(e) {
    e.preventDefault();
   
    currentPage = "displayFlags";


    var btn = document.getElementById("viewFlagsLabel");
    btn.value = "return to add flags page";
    btn = document.getElementById("viewFlags");
    btn.onsubmit = function () { returnToAddFlags() };

    var btn2 = document.getElementById("updateFlagsLabel");
    btn2.value = "Remove Flag";

    //var btn3 = document.getElementById("searchType");
    //btn3.onsubmit = function () { searchAccountFlags(e) };
    var btn4 = document.getElementById("searchTypeLabel");
    btn4.value = "Search Your Flags";


    deleteCurrentData(e);
    await fetch('http://localhost:49200/api/GetNAccountFlags?' + page)
        .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
        .then(data => console.log(data));
    console.log("display user flags was clicked");


    displayIngs();

}

async function searchAccountFlags(e) {
    e.preventDefault();
    try {
        deleteCurrentData(e);
        currentPage = "searchFlags";
        let search = document.getElementById('search').value;
        console.log(search);
        await fetch('http://localhost:49200/api/GetAccountFlagBySearch?' + search + "?" + page)
            .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
            .then(data => console.log(data));
        console.log("made it to the otherside of searchAccountFlags");

        displayIngs();
    } catch (ex) {
        console.log("searchAccountFlags: " + ex);
        throw ex;
    }

}

async function searchIngs(e) {
    e.preventDefault();
    

    deleteCurrentData(e);
    currentPage = "searchIngredients";
    let search = document.getElementById('search').value;

    await fetch('http://localhost:49200/api/AccountSearchIngredients?' + search + "?" + page)
        .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
        .then(data => console.log(data));

    displayIngs();
}
async function searchButtonPressed(e) {
    if (currentPage == "displayFlags") {
        console.log("Go to searchAccountFlags");
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


async function returnToAddFlags() {

    page = "0";
    currentPage = "default";
    deleteCurrentData();

    getIngs();
    displayIngs();
    return;
}

async function UpdateFlagsButtonPressed(e) {
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


async function sendNewFlag(e) {
    e.preventDefault();
    console.log("IN SENDFLAGUPDATE");
    const itemsToAdd = [];
    var counter = 0;
    for (data in checkBoxList) {

        if (document.getElementById(getIDs[data]).checked) {
            itemsToAdd[counter] = getIDs[data];
            counter += 1;
        }
    }
    alert("Flag(s) added to your account!");



    await fetch('http://localhost:49200/api/AccountAddFlags', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: (itemsToAdd),
    })

}

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

    await fetch('http://localhost:49200/api/AccountRemoveFlag?' + page, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: (itemsToRemove),
    })
    alert("Flag(s) removed from your account");

    //returnToAddFlags();
    getUserFlagButtonPressed(e);

}



function deleteCurrentData() {

    var parent = document.getElementById('container');
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
    checkBoxList = [];
}

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
    console.log("page: " + page);

}

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
    console.log("page: " + page);

}

