﻿var jsonData = JSON.parse(localStorage.getItem('JWT'));
var jwt = JSON.stringify(jsonData.token);

await fetch('http://localhost:49202/api/ValidateLoggedIn?' + new URLSearchParams({token: jwt, search: search}))