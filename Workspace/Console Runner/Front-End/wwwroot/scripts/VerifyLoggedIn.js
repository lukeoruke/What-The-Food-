

(async () => {

    var jsonData = JSON.parse(localStorage.getItem('JWT'));
    var jwt = JSON.stringify(jsonData.token);
    var isValid = await fetch('http://localhost:49202/api/ValidateLoggedIn?' + new URLSearchParams({ token: jwt }))
    console.log("In verify loggin in .js");
    window.location.replace("http://www.login.html");
})();
