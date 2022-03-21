async function sendSignup(e) {
    e.preventDefault();

    console.log('Attempting to sign up...');

    let email = document.getElementById('email').value;
    let password = document.getElementById('password').value;
    let confirmPassword = document.getElementById('confirmPassword').value;

    const formData = new FormData();
    formData.append('email', email);
    formData.append('password', password);

    console.log(email);
    console.log(password);
    console.log(confirmPassword);

    if (password === confirmPassword) {
        console.log('Matched');
    }
    if (password !== confirmPassword || confirmPassword === undefined) {
        console.log('Doesn\'t Match');
        document.getElementById('confirmPasswordLabel').innerText = 'Passwords must match.';
        alert('Passwords must match!');
        return;
    }

    // HTTP Get Request
    await fetch('https://localhost:49201/api/AccountSignUp')
        .then(response => console.log(response.text()))
        .then(data => console.log(data));

    // HTTP Post Request
    await fetch('https://localhost:49201/api/AccountSignUp', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        console.log(response.status); // returns 200;
    });

    alert('Successfully sent sign up request!');
}
