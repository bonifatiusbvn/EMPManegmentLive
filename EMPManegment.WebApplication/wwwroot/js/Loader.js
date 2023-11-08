function siteloadershow() {
    $('.loader').show();

}

function siteloaderhide() {
    $('.loader').hide();

}
$(window).on('beforeunload', function () {
    siteloadershow();
})

$(document).on('submit', 'form', function () {
    siteloadershow();
})
window.setTimeout(function () {
    siteloaderhide();
}, 500)
