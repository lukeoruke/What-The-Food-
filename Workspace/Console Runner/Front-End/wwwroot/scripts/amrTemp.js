console.log("TOP OF FILE");

async function sendAmrData() {
    console.log('Sending AMR Info');
    let gender = document.getElementById('gender').value;
    let weight = document.getElementById('weight').value;
    let height = document.getElementById('height').value;
    let age = document.getElementById('age').value;
    let activity = document.getElementById('activity').value;

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
    

    await fetch('http://localhost:49201/api/AddAMR', {
        method: 'POST',
        body: formData,
    }).then(function (response) {
        console.log(response.status); // returns 200;
    });
}


