const checkBoxList = [];
async function AddFlagCheckBoxes() {
    console.log("ADD FLAG CHECK BOXES FUNCTION STARTING");
    await getIngs();

    var jsonData = localStorage.getItem('allIngredients');
    console.log(jsonData);
    const jsonConst = JSON.parse(jsonData);
    var getNames = jsonConst.IngredientName;
    var getIDs = jsonConst.IngredientID;
    

    console.log(jsonConst);
    
    // create the necessary elements

    for (data in getNames) {

    //for(var i = 0; i < 4; i++){
        var text = "Ingredient Name: " + getNames[data];
        //var text = i + ": " + i;
        var label = document.createElement("data");
        var description = document.createTextNode(text);

        var checkbox = document.createElement("input");


        checkbox.type = "checkbox";
        checkbox.name = "box" + data;
        checkbox.value = description;
        label.appendChild(checkbox);
        label.appendChild(description);

        // add the label element to your div
        document.getElementById('container').appendChild(label);
        document.getElementById('container').innerHTML += "<br/>";

        checkBoxList[data] = checkbox;
        
    }

    
}
async function getIngs() {
    await fetch('http://localhost:49200/api/GetAllIngredients')
        .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
        .then(data => console.log(data));
}

async function sendFlagUpdate(e) {
    console.log("IN SENDFLAGUPDATE");
    for (data in checkBoxList) {
        console.log("check box[" + data + "] = " + checkBoxList[data].checked);
        console.log(checkBoxList[data].name + " " + + checkBoxList[data].checked);
    }

}

function myFunction() {
    console.log("TEST");
    checkBoxList[0].checked = true;
    console.log(checkBoxList[0].name);
}





