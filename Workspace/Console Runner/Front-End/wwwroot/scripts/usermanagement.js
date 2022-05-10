// Global vars for managing pagination
const UmAction = {
    ADD: 1,
    UPDATE: 2,
    DELETE: 3,
    ENABLE: 4,
    DISABLE: 5
}
Object.freeze(UmAction);

currentAction = UmAction.ADD;

function getFullAccountForm(action) {
    let base = document.createElement("form");
    base.id = "umform";
    base.name = "umform";
    base.append(...createLabeledInput("Email"));
    base.append(...createLabeledInput("Password"));
    base.append(...createLabeledInput("First Name"));
    base.append(...createLabeledInput("Last Name"));
    base.append(...createNewsBiasInput());
    base.append(...createIsEnabledInput());
    base.appendChild(createSubmitButton(action));
    return base;
}

function getAccountUpdateForm(action) {
    let base = document.createElement("form");
    base.id = "umform";
    base.name = "umform";
    base.append(...createLabeledInput("User Id", "number"));
    base.append(...createLabeledInput("Password"));
    base.append(...createLabeledInput("First Name"));
    base.append(...createLabeledInput("Last Name"));
    base.append(...createNewsBiasInput());
    base.append(...createIsEnabledInput());
    base.appendChild(createSubmitButton(action));
    return base;
}

function getUserIdForm(action) {
    let base = document.createElement("form");
    base.id = "umform";
    base.name = "umform";
    base.append(...createLabeledInput("User Id"));
    base.appendChild(createSubmitButton(action));
    return base;
}

function activateUmAction(actionStr) {
    currentAction = UmAction[actionStr];
    let container = document.getElementById("container");
    removeAllChildrenNodes(container);
    if (currentAction === UmAction.ADD) {
        container.appendChild(getFullAccountForm(currentAction));
    }
    else if (currentAction === UmAction.UPDATE) {
        container.appendChild(getAccountUpdateForm(currentAction));
    }
    else if (currentAction === UmAction.DELETE || currentAction === UmAction.ENABLE || currentAction === UmAction.DISABLE) {
        container.appendChild(getUserIdForm(currentAction));
    }
}

function sendAction() {
    let form = document.querySelector("form");
    const formData = new FormData(form);
    let dataToSubmit;

    switch (currentAction) {
        case UmAction.ADD:
            dataToSubmit = {
                email: formData.get('email'),
                password: formData.get('password'),
                fName: formData.get('firstname'),
                lName: formData.get('lastname'),
                newsBias: parseInt(formData.get('newsbias')),
                isActive: true,
                enabled: formData.get('isenabled') === 'true' ? true : false,
                salt: ""
            }
            fetch("http://47.151.24.23:49202/api/UserManagement/AddUser?" + new URLSearchParams({
                token: localStorage.getItem('JWT')
            }), {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(dataToSubmit)
            })
            .then((response) => response.status)
            .then((statuscode) => displayResponse(statuscode));
            break;
        case UmAction.UPDATE:
            dataToSubmit = {
                userId: parseInt(formData.get('userid')),
                email: 'placeholder',
                password: formData.get('password'),
                fName: formData.get('firstname'),
                lName: formData.get('lastname'),
                newsBias: parseInt(formData.get('newsbias')),
                isActive: true,
                enabled: formData.get('isenabled') === 'true' ? true : false,
                salt: ""
            }
            fetch("http://47.151.24.23:49202/api/UserManagement/UpdateUser?" + new URLSearchParams({
                token: localStorage.getItem('JWT')
            }), {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(dataToSubmit)
            })
            .then((response) => response.status)
            .then((statuscode) => displayResponse(statuscode));
            break;
        case UmAction.DELETE:
            fetch("http://47.151.24.23:49202/api/UserManagement/RemoveUser?" + new URLSearchParams({
                token: localStorage.getItem('JWT'),
                userId: formData.get('userid') 
            }), {
                method: "POST"
            })
            .then((response) => response.status)
            .then((statuscode) => displayResponse(statuscode));
            break;
        case UmAction.ENABLE:
            fetch("http://47.151.24.23:49202/api/UserManagement/EnableUser?" + new URLSearchParams({
                token: localStorage.getItem('JWT'),
                userId: formData.get('userid')
            }), {
                method: "POST"
            })
                .then((response) => response.status)
                .then((statuscode) => displayResponse(statuscode));
            break;
        case UmAction.DISABLE:
            fetch("http://47.151.24.23:49202/api/UserManagement/DisableUser?" + new URLSearchParams({
                token: localStorage.getItem('JWT'),
                userId: formData.get('userid')
            }), {
                method: "POST"
            })
                .then((response) => response.status)
                .then((statuscode) => displayResponse(statuscode));
            break;
    }
}

function displayResponse(statuscode) {
    let messageNode = document.getElementById("message");
    let message = document.createElement("h1");
    removeAllChildrenNodes(messageNode);
    switch (statuscode) {
        case 200:
            message.innerHTML = "User management action successful.";
            break;
        case 400:
            message.innerHTML = "Action failed. Something seems wrong with your input.";
            break;
        case 401:
            message.innerHTML = "Action failed. You are not authorized to perform this action.";
            break;
        case 500:
            message.innerHTML = "Action failed. Something went wrong in the server.";
            break;
        default:
            message.innerHTML = "Action failed. Something went wrong in the server.";
    }
    messageNode.appendChild(message);
}

function removeAllChildrenNodes(node) {
    while (node.firstChild) {
        node.removeChild(node.firstChild);
    }
}

function createLabeledInput(name, type = "") {
    let label = document.createElement("label");
    label.setAttribute("for", name.toLowerCase().replaceAll(" ", ""));
    label.innerHTML = name;
    let textBox = document.createElement("input");
    textBox.id = name.toLowerCase().replaceAll(" ", "");
    textBox.name = name.toLowerCase().replaceAll(" ", "");
    textBox.placeholder = name;
    // disable pressing enter to submit, else it refreshes the page
    textBox.onkeypress = function (e) {
        var key = e.charCode || e.keyCode || 0;
        if (key == 13) {
            return false;
        }
    };
    if (type !== "") {
        textBox.type = type;
    }
    return [label, textBox, document.createElement("br")];
}

function createNewsBiasInput() {
    let label = document.createElement("label");
    label.setAttribute("for", "newsbias");
    label.innerHTML = "News Bias";
    let selector = document.createElement("select");
    selector.id = "newsbias";
    selector.name = "newsbias";
    selector.appendChild(new Option('0', '0', true));
    selector.appendChild(new Option('1', '1', false));
    selector.appendChild(new Option('2', '2', false));
    selector.appendChild(new Option('3', '3', false));
    return [label, selector, document.createElement("br")];

}

function createIsEnabledInput() {
    let label = document.createElement("p");
    label.innerHTML = "Enabled";

    let trueLabel = document.createElement("label");
    trueLabel.htmlFor = "isEnabledTrue";
    trueLabel.innerHTML = "True";
    let trueButton = document.createElement("input");
    trueButton.type = "radio";
    trueButton.id = "isEnabledTrue";
    trueButton.value = "true";
    trueButton.name = "isenabled";
    trueButton.checked = true;

    let falseLabel = document.createElement("label");
    falseLabel.htmlFor = "isEnabledFalse";
    falseLabel.innerHTML = "False";
    let falseButton = document.createElement("input");
    falseButton.type = "radio";
    falseButton.id = "isEnabledFalse";
    falseButton.value = "true";
    falseButton.name = "isenabled";

    return [label, trueLabel, trueButton, falseLabel, falseButton, document.createElement("br")];
}

function createSubmitButton(action) {
    let button = document.createElement("button");
    button.id = "submit-button";
    button.type = "button";
    button.innerHTML = "Submit";
    button.onclick = sendAction;
    return button;
}