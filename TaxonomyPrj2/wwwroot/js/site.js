
$(document).ready(function () {

    $(document).on('click', function () {  // проверка на смену роли во время работы
        var constRole = $('#constRole').val();
       
        $.get('/Account/returnRole', { role: constRole }, function (data) {
            
            if (!data.result) {

                alert("Внимание! У пользователя сменилась роль");
                $('.hideNotAut').html("");
                window.location.replace("/"); 

            }
           

        });
    });
    
});