console.log("TOP OF FILE");

async function sendAmrData(e) {
    e.preventDefault();
    console.log('Sending AMR Info');

    
    var gender = document.getElementById('gender').value;
    var weight = document.getElementById('weight').value;
    var height = document.getElementById('height').value;
    var age = document.getElementById('age').value;
    var activity = document.getElementById('activity').value;
    
    /**
    let gender = document.getElementById('gender').value;
    let weight = document.getElementById('weight').value;
    let height = document.getElementById('height').value;
    let age = document.getElementById('age').value;
    let activity = document.getElementById('activity').value;
    **/

    const formData = new FormData();
    formData.append('gender', gender);
    formData.append('weight', weight);
    formData.append('height', height);
    formData.append('age', age);
    formData.append('activity', activity);

    console.log(gender);
    console.log(weight);
    console.log(height);
    console.log(age);
    console.log(activity);

    /**
    await fetch('http://47.151.24.23:49202/api/AddAMR', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        console.log(response.status); // returns 200;
    });
    **/
    calculate(gender, weight, height, age, activity);
}

//function checks to see what activity was selecte and returns float value
async function activityCheck(activity) {

    if (activity == "None") {
        return 1.2;
    }
    else if (activity == "Light") {
        return 1.375;
    }
    else if (activity == "Moderate") {
        return 1.55;
    }
    else if (activity == "Daily") {
        return 1.725;
    }
    else if (activity == "Heavy") {
        return 1.9;
    }
    else {
        return 1.2;
    }


}


/// Enum that reflects the five levels of activity used in calculating AMR according to https://www.verywellfit.com/how-many-calories-do-i-need-each-day-2506873.
/// kg and cm conversions are referenced to https://www.metric-conversions.org/weight/pounds-to-kilograms.htm

async function calculate(gender, weight, height, age, activity) {
    
    //converts to float

    var userWeight = parseFloat(weight) / 2.2; //coverts lb to kg
    var userHeight = parseFloat(height) * 2.54; //converts inches to centimeters
    var userAge = parseFloat(age);

    //await changes promise 
    var userActivity = await activityCheck(activity);




    //Calculations for Male User
    if (gender == "Male") {
        bmr = 66.47 + (userWeight * 13.75) + (userHeight * 5.003) - (userAge * 6.755);
    }

    //Calculations of Female Users
    else {
        bmr = 65.51 + (userWeight * 9.563) + (userHeight * 1.850) - (userAge * 4.676);
    }

    var amr = bmr * userActivity;


    console.log("-------------------------");
    console.log('User AMR Value Calculated');
    console.log(amr.toFixed(2) + " kcal/day");


}


