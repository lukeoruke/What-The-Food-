var checkBoxList = [];
var getIDs = [];
var searching = false;
var search;
async function AddFlagCheckBoxes() {
    console.log("ADD FLAG CHECK BOXES FUNCTION STARTING");
    if (!searching) {
        await getIngs();
    } else {

        searching = false;
        console.log(search);
    }
    
    displayIngs();
}

function displayIngs() {

    var jsonData = localStorage.getItem('allIngredients');
    console.log(jsonData);
    const jsonConst = JSON.parse(jsonData);
    var getNames = jsonConst.IngredientName;
    getIDs = jsonConst.IngredientID;


    console.log(jsonConst);

    // create the necessary elements

    for (data in getNames) {

        //for(var i = 0; i < 4; i++){
        var text = "Ingredient Name: " + getNames[data];
        //var text = i + ": " + i;
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
    await fetch('http://localhost:49200/api/GetNIngredients')
        .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
        .then(data => console.log(data));
}

async function searchIngs(e) {
    e.preventDefault();
    searching = true;
    let search = document.getElementById('search').value;

    await fetch('http://localhost:49200/api/AccountSearchIngredients?' + search)
        .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
        .then(data => console.log(data));
    var parent = document.getElementById('container');
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
    displayIngs();
}

async function sendFlagUpdate(e) {
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
    console.log(itemsToAdd);


    await fetch('http://localhost:49200/api/AccountAddFlags', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: (itemsToAdd),
    })

}

function myFunction() {
    console.log("TEST");
    checkBoxList[0].checked = true;
    console.log(checkBoxList[0].name);
}





