
$(function () {
    $('#Filter').click(function () {
        window.location.href = "/Home/Filter/" + $('#Breed').val();
    });
});

$(function () {
    $('.close').click(function () {
        var yourArray = document.getElementsByClassName('modal');

        for (let arrayItem of yourArray) {
            arrayItem.style.display = "none";
            $(arrayItem).find('input[type=text]').val("");
        }
    });
});


