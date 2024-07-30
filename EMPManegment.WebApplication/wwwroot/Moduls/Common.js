function getCommonDateformat(date) {
    return moment(date).format('DD MMM YY');
}

function getCommonDatetime(date, time) {
    var datetimeString = time ? (date + ' ' + time) : date;
    return moment(datetimeString, 'YYYY-MM-DD HH:mm:ss').format('YYYY-MM-DDTHH:mm');
}