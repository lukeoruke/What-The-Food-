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

    // Check if email is valid
    for (const c of email) {
        let code = c.charCodeAt(0);
        if (!(c === '.' || c === ',' || c === '@' || c === '!') &&
            !((code > 47 && code < 58) || (code > 64 && code < 91) || (code > 96 && code < 123))) {
            alert('Invalid character in email!');
            return;
        }
    }

    // Check if password is valid
    if (password === confirmPassword) {
        console.log('Matched');

        if (password.length < 8) {
            alert('Password is too short!');
            return;
        } else {
            for (const c of password) {
                let code = c.charCodeAt(0);
                if (!(c === ' ' || c === '.' || c === ',' || c === '@' || c === '!') &&
                    !((code > 47 && code < 58) || (code > 64 && code < 91) || (code > 96 && code < 123))) {
                    alert('Invalid character in password!');
                    return;
                }
            }
        }
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
    /*await fetch('http://47.151.24.23:49202/api/AccountSignUp')
        .then(response => console.log(response.text()))
        .then(data => console.log(data));*/

    console.log("this is right above the fetch");
    // HTTP Post Request
    await fetch('http://47.151.24.23:49202/api/AccountSignUp', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        console.log(response.status); // returns 200;
    });
    console.log("this is right UNDER the fetch");
    alert('Successfully sent sign up request!');
}
