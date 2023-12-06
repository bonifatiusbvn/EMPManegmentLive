$(document).ready(function () {
    
    let currentTime = new Date();
    let morningStart = new Date();
    morningStart.setHours(10, 0, 0); // 10:00 AM

    let morningEnd = new Date();
    morningEnd.setHours(11, 59, 59); // 11:59 AM

    let afternoonStart = new Date();
    afternoonStart.setHours(12, 0, 0); // 12:00 PM

    let afternoonEnd = new Date();
    afternoonEnd.setHours(17, 59, 59); // 5:59 PM

   
    if (currentTime >= morningStart && currentTime <= morningEnd)
    {
        $("#txtdaywish").text("Good Morning..!!");

    }
    else if (currentTime >= afternoonStart && currentTime <= afternoonEnd)
    {
        $("#txtdaywish").text("Good Afternoon..!!");
    }
    else
    {
        $("#txtdaywish").text("Good Evening..!!");
    }
});