async function sendSignup(e) {
    e.preventDefault();

    console.log('Attempting to sign up...');

    let name = document.getElementById('name').value;
    let email = document.getElementById('email').value;
    let password = document.getElementById('password').value;
    let confirmPassword = document.getElementById('confirmPassword').value;

    if (name === '' || email === '' || password === '' || confirmPassword === '') {
        alert('Please fill out all fields!');
        return;
    }

    if (password === confirmPassword) {
        console.log('Matched');
    } else {
        console.log('Doesn\'t Match');
        document.getElementById('confirmLabel').innerText = 'Passwords must match.';
        alert('Passwords must match!');
        return;
    }

    const formData = new FormData();
    formData.append('email', email);
    formData.append('password', password);
    formData.append('name', name);

    console.log(email);
    console.log(password);
    console.log(confirmPassword);

    // HTTP Get Request
    /*await fetch('http://localhost:49200/api/AccountSignUp')
        .then(response => console.log(response.text()))
        .then(data => console.log(data));*/

    // HTTP Post Request
    await fetch('http://localhost:49200/api/AccountSignUp', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        console.log(response.status); // returns 200;
    });

    alert('Successfully sent sign up request!');
}
