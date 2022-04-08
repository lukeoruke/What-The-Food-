async function AddFlagCheckBoxes() {
    await getIngs();

    var jsonData = localStorage.getItem('allIngredients');
    console.log(jsonData);
    const jsonConst = JSON.parse(jsonData);
    console.log(jsonConst);
    
    // create the necessary elements
    for (data in jsonConst) {
    //for(var i = 0; i < 4; i++){
        var text = data + ": " + jsonConst[data];
        //var text = i + ": " + i;
        var label = document.createElement("label");
        var description = document.createTextNode(text);

        var checkbox = document.createElement("input");

        checkbox.type = "checkbox";    // make the element a checkbox
        checkbox.name = "box1";      // give it a name we can check on the server side
        checkbox.value = description;         // make its value "pair"

        label.appendChild(checkbox);   // add the box to the element
        label.appendChild(description);// add the description to the element

        // add the label element to your div
        document.getElementById('container').appendChild(label);
        document.getElementById('container').innerHTML += "<br/>";


    }


}
async function getIngs() {
    console.log("SADASDASDAWDASDAWDA????");
    await fetch('http://localhost:49200/api/GetAllIngredients')
        .then(async response => localStorage.setItem('allIngredients', JSON.stringify(await response.json())))
        .then(data => console.log(data));
}

