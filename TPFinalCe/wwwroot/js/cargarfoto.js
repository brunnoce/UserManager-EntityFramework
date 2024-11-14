//$(document).ready(function () {
//    $("#seleccionImg").change(function () {
//        var tam = this.files[0].size;
//        if (tam > 5000000) {
//            alert("El tamaño del archivo no debe ser mayor a 5 mb");
//        }
//        else {
//            readURL(this);
//        }
//    });
//});
//function readURL(input) {
//    if (input.files && input.files[0]) {
//        var reader = new FileReader();
//        reader.onload = function (e) {
//            $("#imagen").attr("src", e.target.result);
//        }
//        reader.readAsDataURL(input.files[0]);
//    }
//}

$("#seleccionImg").change(function () {
    var fileName = this.files[0].name;
    var fileSize = this.files[0].size;
    var esValido = 0;

    if (fileSize > 5000000) {
        alert('El archivo no debe superar los 5MB');
        this.value = '';
        this.files[0].name = '';
        esValido = 1;
    } else {
        var ext = fileName.split('.').pop();

        ext = ext.toLowerCase();

        switch (ext) {
            case 'jpg':
            case 'jpeg':
            case 'png': break;
            default:
                alert('El archivo no tiene la extensión adecuada');
                this.value = '';
                this.files[0].name = '';
                esValido = 1;
        }
    }
    if (esValido == 0) {
        readURL(this);
    }
});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $("#imagen").attr("src", e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}


