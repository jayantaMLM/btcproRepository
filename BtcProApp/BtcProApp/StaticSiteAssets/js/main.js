
$(window).on("load", function () {
    'use strict';
    // Class Of Body 
    $(".body").fadeOut();
    // Element Body For Page .
    $("body").removeClass("loading-scroll-hiden");
    $(window).scrollTop(0);
    /* Call wow.js */
    new WOW().init();
});//End Loading
$(function () {
    'use strict';
    var $home         = $("#home"),   // Cached Id Form Homepage
        $about        = $("#about"), // Cached Id Form Homepage
        $container    = $("#home .container"), // Cached Id Form Container In Homepage
        $height       = $(window).height(), // Calc Height For Window
        $pad          = $height / 3, // padding of container home page.
        $padPhon      = $height / 3.3, // padding of container home page.
        $padPhonrate  = $height / 5.5, // padding of container home page.                
        $navscroll    = $height / 1.2, // when scroll Change navbar To Black 
        $width        = $(window).width(), // width for window
        $navId        = $("#nav"), // Cached Id Form Nav 
        $buttonTop    = $("#top-button"),
        $licknavbar   = $("#navbar li a"),// Cached Id Form Button Top 
        $navbarToggle = $(".navbar-toggle"),// Cached Id Form Navbar Toggle
        $NavLink      = $(".nav a"), // Cached Class Link .
        $navbarBrand  = $(".navbar-brand"),
        $iconBar      = $(".icon-bar"),
        $simplefilter  = $('.simplefilter li'),
        $navbarNav    = $(".navbar-nav");
    /*
     *-----------------------------------------------
     *         Height & Padding TopFor Home Page
     *-----------------------------------------------
     */
    // padding container 
    $container.css("padding-top", $pad);
    // when width < 700 -> height auto
    if ($width < 700) {
        
        // padding container 
        $container.css("padding-top", $padPhon);
    }
    // Rotation In Mobile 
    if ($height < 300) {
        // padding container 
        $container.css("padding-top", $padPhonrate);
    }
    /*
     *-----------------------------------------------
     *   On Scroll Home Page Change navbar To Black
     *-----------------------------------------------
     */
}); // Close Function  


    /* ---------------------------------------------------------------------- */
    /* ------------------------- accordion & toggles ------------------------ */
    /* ---------------------------------------------------------------------- */
    function toggleChevron(e) {
        $(e.target)
            .prev('.panel-heading')
            .find("i.indicator")
            .toggleClass('glyphicon-minus glyphicon-plus');
    }
    $('#accordion').on('hidden.bs.collapse', toggleChevron);
    $('#accordion').on('shown.bs.collapse', toggleChevron);


    /* ---------------------------------------------------------------------- */
    /* ------------------------- scroll part ------------------------ */
    /* ---------------------------------------------------------------------- */

    $(document).ready(function(){
    $(function(){
        $(document).on( 'scroll', function(){
            if ($(window).scrollTop() > 100) {
                $('.scroll-top-wrapper').addClass('show');
            } else {
                $('.scroll-top-wrapper').removeClass('show');
            }
        });
        $('.scroll-top-wrapper').on('click', scrollToTop);
    });
    function scrollToTop() {
        verticalOffset = typeof(verticalOffset) != 'undefined' ? verticalOffset : 0;
        element = $('body');
        offset = element.offset();
        offsetTop = offset.top;
        $('html, body').animate({scrollTop: offsetTop}, 500, 'linear');
    }
    });
