function showSearch() {

    if ($("#navbarSearch").css('display') == 'none') {
        $("#navbarSearch").css({ 'display': "visible" });
        $("#navbarSearch").fadeIn(250);
        
        $(".navbar-form").css({
            "border-color": "#C1D6D8",
            "border-width": "1px",
            "border-style": "solid"
        });
        $("#navbarSeachField").focus();
    } else {
        $("#navbarSearch").fadeOut(250);
        $("#navbarSearch").css({ 'display': "none" });
        $(".navbar-form").css({
            "border-color": "#C1D6D8",
            "border-width": "0px",
            "border-style": "solid"
        });
        $("#navbarSeachField").focus();
    }
}
function showProfileDropMenu() {
    if ($(".dropdownProfile").css('display') == 'none') {
        $(".dropdownProfile").css({ 'display': "visible" });
        $(".dropdownProfile").fadeIn(200);
    } else {
        $(".dropdownProfile").fadeOut(200);
        $(".dropdownProfile").css({ 'display': "none" });
    }
}