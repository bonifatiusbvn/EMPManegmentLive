function getCommonDateformat(date) {
    return moment(date).format('DD MMM YY');
}

function getCommonDatetime(date, time) {

    var datePart = moment(date).format('YYYY-MM-DD');
    var timePart = moment(time).format('HH:mm:ss');
    var datetimeString = `${datePart} ${timePart}`;
    var formattedDateTime = moment(datetimeString, 'YYYY-MM-DD HH:mm:ss').format('YYYY-MM-DDTHH:mm');

    return formattedDateTime;
}


