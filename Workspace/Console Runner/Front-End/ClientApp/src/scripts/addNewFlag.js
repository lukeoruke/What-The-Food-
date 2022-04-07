namespace Front_End.ClientApp.src.scripts
{
    var label = document.createElement("label");
    var description = document.createTextNode(pair);
    var checkbox = document.createElement("input");

    checkbox.type = "checkbox";    // make the element a checkbox
    checkbox.name = "slct[]";      // give it a name we can check on the server side
    checkbox.value = pair;         // make its value "pair"

    label.appendChild(checkbox);   // add the box to the element
    label.appendChild(description);// add the description to the element

    // add the label element to your div
    document.getElementById('box1').appendChild(label);
    document.getElementById('box1').innerHTML = 'box1213123123';
}
