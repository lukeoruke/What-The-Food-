//Search localstorage for time and name of view to fetch call
//Save current time and name of new view to localstorage
function placeLocalStorage(newViewName, time) {
    let prevView = sessionStorage.getItem("currentViewName");
    let prevTime = parseInt(sessionStorage.getItem("viewTimestamp"));
    let timeDiffInSecs = Math.floor((time - prevTime) / 1000);
    sessionStorage.setItem("currentViewName", newViewName);
    sessionStorage.setItem("viewTimestamp", time);
    return { prevView: prevView, timeViewed: timeDiffInSecs };
};


(async () => {
    var jwt = localStorage.getItem('JWT');
    let resourceName = window.location.pathname.split("/").slice(-1)[0];
    let htmlName = resourceName.split("?")[0];
    let { prevView, timeViewed } = placeLocalStorage(htmlName, Date.now());
    if (prevView === null) {
        prevView = "";
    }
    if (timeViewed === null) {
        timeViewed = 0;
    }
    await fetch('http://47.151.24.23:49202/api/ValidateLoggedIn?' + new URLSearchParams({ token: jwt, previousViewName: prevView, currentViewName: htmlName, time: timeViewed }))
        .then(response => response.text())
        .then((response) => {
            if (response === "False") {
                //console.log("Not verified");
                window.location.replace("http://whatthefood.xyz/login.html");
            }
        })
})();
