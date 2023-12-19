//function siteloadershow() {
//    $('.loader').show();
//}
//function siteloaderhide() {
//    $('.loader').hide(); 
//}
//$(window).on('beforeunload', function () {
//    siteloadershow();
//})

//$(document).on('submit', 'form', function () {
//    siteloadershow();
//})
//window.setTimeout(function () {
//    siteloaderhide();
//}, 500)


function DisplayLoader() {
    $('#Loader').show();
}
function HideLoader() {
    $('#Loader').hide();
}
$('#Loader').hide();
$(document).on('submit', 'form', function () {
    DisplayLoader();
})
$(window).on('beforeunload', function () {
    DisplayLoader();
});
/*$('#Loader').show();*/

