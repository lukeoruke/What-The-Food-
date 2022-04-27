/**
 * Define API from NY Times that will be used
 */
const APIFOOD = 'https://api.nytimes.com/svc/search/v2/articlesearch.json?fq=section_name:(%22Food%22)&sort=newest&api-key=TK2GSBMDBO2kHDUABQFh4tlpE0Bu8cuf'
const APIHEALTH = 'https://api.nytimes.com/svc/search/v2/articlesearch.json?fq=section_name:(%22Health%22)&sort=newest&api-key=TK2GSBMDBO2kHDUABQFh4tlpE0Bu8cuf'

//Create a global variable which will store all News
let array = [];
//the level of which we want to display health realted news
healthFilterLevel = 0;
getHealthFilter().then(pickCategory);
/**
 * Select the category of news that will be catered towards the user
 */
async function pickCategory() {
    console.log('inside pickCategory, healthFilterlevel is: ' + healthFilterLevel);
    //if they have no personalization, display food news as default
    //else display personzalized news based on user preference
    if (healthFilterLevel == 0 ) {
        await storeFoodNews(0);
    }
    else {
        await storeHealthNews(healthFilterLevel);
        await storeFoodNews(healthFilterLevel)
    }
    //display news to homepage accordingly
    displayNews();
}
/**
 * Store food related news from API into array
 * @param {Number} displayAmnt The number of Food Articles that will be displayed
 */
async function storeFoodNews(displayAmnt) {
    //make a promise through fetch on APIFOOD, appply await as well
    await fetch(APIFOOD)
        .then(response => response.json())
        .then(data => {
            for (; displayAmnt < 5; displayAmnt++) {
                array.push(data.response.docs[displayAmnt]);
            }
        });
}
/**
 * Store health related news from API into array
 * @param {Number} displayAmnt The number of Food Articles that will be displayed
 */
async function storeHealthNews(displayAmnt) {
    //fetch returns a promise that resolves into a response object
    //which in this case is a JSON
    await fetch(APIHEALTH)
        .then(response => response.json())
        .then(data => {
            for (let x = 0; x < displayAmnt; x++) {
                array.push(data.response.docs[x]);
            }
        });
}
/**
 *  Populates and assigns news relative to html position
 */
async function displayNews() {
    for (let index = 0; index < 5; index++) {
        //create an anchor element to create hyperlinked html
        let a = document.createElement("a");
        //href specifies the url of the page
        await a.setAttribute('href', array[index].web_url);
        //manipulates the title of html/url
        a.innerText = array[index].headline.main;
        a.title = array[index].section_name;
        //p stores a paragraph from document which is a JSON
        let p = document.createElement("p");
        //assign p to have the lead paragraph from JSON object
        p.innerHTML = array[index].lead_paragraph;

        //store the image of article into assigned location specified in html
        document.getElementById("imgID" + index).src =
            ("https://static01.nyt.com/" + array[index].multimedia[0].url);

        //display title and text in proper position by ID in html
        let headlineText = document.getElementById("headlineText" + index);
        let text = document.getElementById("text" + index);

        //append the hyperlink a made earlier
        headlineText.appendChild(a);
        //append the text paragraph p made earlier
        text.appendChild(p);
    }
}

//async function test() {
//    console.log("ran in test");
//    console.log(array[0]);
//    for (let x = 0; x < 5; x++) {
//        console.log(array[x].headline);
//        console.log("INDEX" + x + ": " + array[x].section_name);
//    }
//}
//async function getFoodNews() {
//    let counter = 0;
//    fetch(APIFOOD)
//        .then(response => response.json())
//        .then(data => {
//            console.log(data);
//            data.response.docs.slice(0, 5).map(article => {
//                console.log(article.headline.main);
//                console.log(article.section_name);
//                //a stand for hyperlink
//                let a = document.createElement("a");
//                //href specifies the url of the page
//                a.setAttribute('href', article.web_url);
//                //manipulates the title of html/url
//                //used to use innerHTML but has risks
//                a.innerText = article.headline.main;
//                a.title = "ALKKJFALDKFJ";
//                console.log(a.title);
//                console.log(a);

//                //p stand for paragraph
//                let p = document.createElement("p");
//                p.innerHTML = article.lead_paragraph;

//                let img = document.getElementById("imgID" + counter).src =
//                    ("https://static01.nyt.com/" + article.multimedia[0].url);
//                console.log(img);

//                let headlineText = document.getElementById("headlineText" + counter);
//                let text = document.getElementById("text" + counter);

//                headlineText.appendChild(a);
//                text.appendChild(p);

//                counter++;
//                console.log(headlineText);
//                console.log(counter);
//                array.push(article.section_name);
//            });
//        })
//}

/**
 * Personalize news depending on the article that is selected by user
 */
headlineText0.addEventListener("click", () => {
    getCategory(0)
    alert();
});

headlineText1.addEventListener("click", () => {
    getCategory(1)
    alert();
});
headlineText2.addEventListener("click", () => {
    getCategory(2)
    alert();
});

headlineText3.addEventListener("click", () => {
    getCategory(3)
    alert();
});
headlineText4.addEventListener("click", () => {
    getCategory(4)
    alert();
});

/**
 * Get the category of article clicked by user
 * @param {Number} index The index of article relative to homepage
 */
async function getCategory(index) {
    var category = array[index].section_name;;
    console.log(category);
    if (category == "Food") {
        console.log("this is a food");
        decrementHealthFilter();
    }
    if (category == "Health") {
        console.log("This is Health");
        incrementHealthFilter();
    }
}
/**
 * Get the number of Health news to display
 */
async function getHealthFilter() {
    //access the backend and increment
    //if the value is set at 4 already, then we do not need to increment it no more
    const response = await fetch('http://localhost:49200/api/News', {
        method: 'GET'
    })
    //get response body as text
    const data = await response.text();
    healthFilterLevel = data;
    console.log("healthFilterLevel is now " + healthFilterLevel);
}
/**
 * Change user preference to have more Health News
 */
async function incrementHealthFilter() {
    //access the backend and increment
    //if the value is set at 4 already, then we do not need to increment it no more
    let data = await fetch('http://localhost:49200/api/NewsIncrement', {
        method: 'POST',
    })
    console.log("INCREMENTED");
}
/**
 * Change user preference to have less Health News
 */
async function decrementHealthFilter() {
    //access the backend and decrement
    //if the value is set at 0 already, then we do not need to increment it no more
    await fetch('http://localhost:49200/api/NewsDecrement', {
        method: 'POST',
    })
    console.log("DECREMENTED");
}

