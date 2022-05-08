
async function sendLogout(e) {
    var jwt = localStorage.getItem("JWT");
    await fetch('http://localhost:49202/api/Logout?' + new URLSearchParams({ token: jwt }));
    localStorage.clear();
    sessionStorage.clear();
    window.location.replace('https://localhost:49199/login.html');
}
