async function sendForgotPassword(e) {
    e.preventDefault();

    //console.log('Attempting to send forgot password...');
    let email = document.getElementById('email').value;
    let confirmEmail = document.getElementById('confirmEmail').value;

    if (email ===''||confirmEmail==='') {
        alert('Please fill out all fields!');
        return;
    }

    if (email === confirmEmail) {
        //console.log('Matched');
    } else {
        //console.log('Doesn\'t Match');
        document.getElementById('confirmLabel').innerText = 'Emails must match.';
        alert('Emails must match!');
        return;
    }

    const formData = new FormData();
    formData.append('email', email);

    //console.log(email);
    //console.log(confirmEmail);

    // HTTP Get Request
    /*await fetch('http://47.151.24.23:49202/gateway/AccountLogin')
        .then(response => console.log(response.text()))
        .then(data => console.log(data));*/

    // HTTP Post Request
    await fetch('http://47.151.24.23:49202/gateway/AccountLogin', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        //console.log(response.status); // returns 200;
    });

    alert('Successfully sent forgot password request!');
}
