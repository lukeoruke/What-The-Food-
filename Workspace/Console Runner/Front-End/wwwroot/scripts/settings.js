async function loadSettings() {
    let jwt = localStorage.getItem('JWT');

    await fetch('http://localhost:49202/api/AccountSettings?' + new URLSearchParams({
        token: jwt
    }))
        .then(async response => sessionStorage.setItem('userInfo', JSON.stringify(await response.json())))
        .then(data => console.log(data));

    let userInfo = sessionStorage.getItem('userInfo');
    const jsonConst = JSON.parse(userInfo);

    let name = jsonConst.name;
    let email = jsonConst.email;

    let nameField = document.getElementById("nameField");
    let emailField = document.getElementById("emailField");

    nameField.value = name;
    emailField.value = email;
}
