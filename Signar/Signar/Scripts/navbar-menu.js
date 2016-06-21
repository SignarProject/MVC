function showSearch() {

    if ($("#searchText").css('display') == 'none') {
        $("#searchText").css({ 'display': "visible" });
        $("#searchText").fadeIn(250);
        
        $(".navbar-form").css({
            "border-color": "#C1D6D8",
            "border-width": "1px",
            "border-style": "solid"
        });
        $(".form-control").focus();
    } else {
        $("#searchText").fadeOut(250);
        $("#searchText").css({ 'display': "none" });
        $(".navbar-form").css({
            "border-color": "#C1D6D8",
            "border-width": "0px",
            "border-style": "solid"
        })
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