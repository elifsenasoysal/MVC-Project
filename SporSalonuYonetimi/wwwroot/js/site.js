// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Sayfa aşağı kayınca Navbar'ın arka planını değiştir veya gölge ekle
$(window).scroll(function() {
    if ($(this).scrollTop() > 50) {
        $('.navbar').addClass('shadow-lg'); // Daha belirgin gölge
    } else {
        $('.navbar').removeClass('shadow-lg');
    }
});