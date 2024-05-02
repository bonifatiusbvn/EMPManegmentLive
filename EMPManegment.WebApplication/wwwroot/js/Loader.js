function siteloadershow() {
    $('.cover-spin').show();

}

function siteloaderhide() {
    $('.cover-spin').hide();

}
$(window).on('beforeunload', function () {
    siteloadershow();
})

$(document).on('submit', 'form', function () {
    siteloadershow();
})
window.setTimeout(function () {
    siteloaderhide();
}, 2000)