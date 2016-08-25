$(document).ready(function () {
    var readURL = function (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var imgpreview = $(input).data("preview");
                var img = document.getElementById(imgpreview);
                console.log("load img");
                console.log(imgpreview);
                img.setAttribute("src",e.target.result);
                //$("#"+imgpreview).attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    $(".preview").change(function () {
        console.log("change");
        readURL(this);
    });

});