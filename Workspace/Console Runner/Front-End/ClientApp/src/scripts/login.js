async function sendLogin(e) {
    e.preventDefault();

    console.log('Attempting to login...');
    let email = document.getElementById('email').value;
    let password = document.getElementById('password').value;

    const formData = new FormData();
    formData.append('email', email);
    formData.append('password', password);

    console.log(email);
    console.log(password);

    // HTTP Get Request
    await fetch('https://localhost:7002/api/AccountLogin')
        .then(response => console.log(response.text()))
        .then(data => console.log(data));

    // HTTP Post Request
    await fetch('https://localhost:7002/api/AccountLogin', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        console.log(response.text()); // returns 200;
    });

    alert('Successfully sent login request!');
}
