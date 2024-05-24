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


    if (currentTime >= morningStart && currentTime <= morningEnd) {
        $("#txtdaywish").text("Good Morning..!!");

    }
    else if (currentTime >= afternoonStart && currentTime <= afternoonEnd) {
        $("#txtdaywish").text("Good Afternoon..!!");
    }
    else {
        $("#txtdaywish").text("Good Evening..!!");
    }
});


$(document).ready(function () {
    debugger
    function handleNavClick(e) {
        var $this = $(this);
        var $submenu = $this.siblings('.nav-treeview');
    }


    $('.nav-item > .nav-link').on('click', handleNavClick);
    $('.nav-treeview .nav-link').on('click', function () {
        var $this = $(this);
        $('.nav-treeview .nav-link').removeClass('active');
        $this.addClass('active');
        $this.parents('.nav-item').addClass('menu-open');
        $this.children('.nav-link').addClass('active');
        $this.parents('.nav-treeview').slideDown().css('display', '');

    });


    var currentPath = window.location.pathname;
    $('.nav-link').each(function () {
        if ($(this).attr('href') === currentPath) {
            $(this).addClass('active');
            $(this).parents('.nav-item').addClass('menu-open');
            $('.menu-open').children('a.nav-link').addClass('active');
            $(this).parents('.nav-treeview').slideDown().css('display', '');

        }
    });

});







