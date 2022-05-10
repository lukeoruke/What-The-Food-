
async function sendLogout(e) {
    var jwt = localStorage.getItem("JWT");
    await fetch('http://47.151.24.23:49202/api/Logout?' + new URLSearchParams({ token: jwt }));
    localStorage.clear();
    sessionStorage.clear();
    window.location.replace('http://whatthefood.xyz/login.html');
}
async function deleteUserData() {
    var jwt = localStorage.getItem("JWT");
    await fetch('http://47.151.24.23:49202/api/DeleteUserData?' + new URLSearchParams({ token: jwt
    }), {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Origin': '*',
        },
    })
    localStorage.clear();
    sessionStorage.clear();
    window.location.replace('http://whatthefood.xyz/login.html');
}
async function toggleDataCollection() {
    var jwt = localStorage.getItem("JWT");
    await fetch('http://47.151.24.23:49202/api/ToggleDataCollection?' + new URLSearchParams({
        token: jwt
    }), {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Origin': '*',
        },
    })
}