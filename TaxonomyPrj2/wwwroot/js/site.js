﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    $(document).on('click', function () {  // проверка на смену роли во время работы
        var constRole = $('#constRole').val();
       
        $.get('/Account/returnRole', { role: constRole }, function (data) {
            
            if (!data.result) {

                alert("Внимание! У пользователя сменилась роль");
                $('.hideNotAut').html("");
                window.location.replace("/");  // узнать, как сделать универсально

            }
            else { alert("!!"); }

        });
    });
    
});