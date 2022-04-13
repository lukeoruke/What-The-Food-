var checkBoxList = [];
var getIDs = [];
var searching = false;
var search;
var page = "0";
var displayingFlags = false;
var currentPage = "default";
async function AddFlagCheckBoxes() {
    console.log(currentPage);
    console.log("ADD FLAG CHECK BOXES FUNCTION STARTING");
    if (!searching) {
        await getIngs();
    } else {

        searching = false;
        console.log("Search: " + search);
    }
    
    displayIngs();
}

function displayIngs() {

    var jsonData = localStorage.getItem('allIngredients');
    console.log(jsonData);
    const jsonConst = JSON.parse(jsonData);
    console.log(jsonConst)
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
    displayingFlags = false;
    console.log(page);
    await fetch('http://localhost:49200/api/GetNIngredients?' + page)
        .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
        .then(data => console.log(data));
}

async function getUserFlags(e) {
    e.preventDefault();
    var btn = document.getElementById("viewFlags");
    btn.value = "return to add flags page";
    btn = document.getElementById("returnButton");
    btn.onsubmit = function () { returnToAddFlags };
    currentPage = "userFlags";
    

    deleteCurrentData(e);
    await fetch('http://localhost:49200/api/GetAllAccountFlags?' + page)
        .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
        .then(data => console.log(data));
    console.log("display user flags was clicked");
    displayingFlags = true;


    displayIngs();

}
async function returnToAddFlags() {
    console.log("test");
    getIngs();
    displayIngs();
    return;
}

async function addOrRemoveFlags(e) {
    e.preventDefault();
    if (displayingFlags) {
        removeFlag(e);
    } else {
        sendNewFlag(e);
    }
}

async function searchIngs(e) {
    e.preventDefault();
    deleteCurrentData(e);
    let search = document.getElementById('search').value;
    if (search == "" || search == " ") {
        returnToAddFlags(e);
        return;
    }
    await fetch('http://localhost:49200/api/AccountSearchIngredients?' + search  + "?" + page)
        .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
        .then(data => console.log(data));
    
    displayIngs();
}
async function searchButtonPressed(e) {
    page = "0";
    searching = true;
    currentPage = "search";
    searchIngs(e);
}

async function sendNewFlag(e) {
    e.preventDefault();
    console.log("IN SENDFLAGUPDATE");
    const itemsToAdd = [];
    var counter = 0;
    for (data in checkBoxList) {
        console.log("check box[" + data + "] = " + checkBoxList[data].checked);
        console.log(document.getElementById(getIDs[data]).checked);
        if (document.getElementById(getIDs[data]).checked) {
            itemsToAdd[counter] = getIDs[data];
            counter += 1;
        }
    }
    alert("Flag(s) added to your account!");
    console.log(itemsToAdd);


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
        console.log("check box[" + data + "] = " + checkBoxList[data].checked);
        console.log(document.getElementById(getIDs[data]).checked);
        if (document.getElementById(getIDs[data]).checked) {
            itemsToRemove[counter] = getIDs[data];
            counter += 1;
        }
    }


    console.log(itemsToRemove);
    await fetch('http://localhost:49200/api/AccountRemoveFlag', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: (itemsToRemove),
    })
    alert("Flag(s) removed from your account");
    returnToAddFlags(e);

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
    } else if (currentPage == "search") {
        await searchIngs(e);
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
    } else if (currentPage == "search") {
        await searchIngs(e);
    } else {
        displayIngs();
    }
    console.log("page: " + page);
    
}

function deleteCurrentData(e) {
    e.preventDefault();
    var parent = document.getElementById('container');
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
    checkBoxList = [];
}


