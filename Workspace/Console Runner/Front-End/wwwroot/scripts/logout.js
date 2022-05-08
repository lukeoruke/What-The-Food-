
async function sendLogout(e) {
    var jsonData = JSON.parse(localStorage.getItem('JWT'));
    var jwt = JSON.stringify(jsonData.token);
    await fetch('http://localhost:49202/api/Logout?' + new URLSearchParams({ token: jwt }));

    window.location.replace('https://localhost:49199/login.html');
}
