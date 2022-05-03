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



    // HTTP Post Request\
    if (await fetch('http://localhost:49201/api/AccountLogin', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        console.log(response.status); // returns 200;
    }).response == 200) {
        // HTTP Get Request
        await fetch('http://localhost:49201/api/AccountLogin')
            .then(response => console.log(response.text()))
            .then(data => console.log(data));
    }
        




    alert('Successfully sent login request!');
}
