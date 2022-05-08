async function sendLogout(e) {
    localStorage.clear();
    sessionStorage.clear();
    //await fetch('http://localhost:49202/api/Logout?' + new URLSearchParams({token: jwt}));
    window.location.replace('https://localhost:49199/login.html');
}
