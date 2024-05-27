function getCommonDateformat(date) {
    var formattedDate = moment(date).format('DD MMM YY');
    return formattedDate;
}