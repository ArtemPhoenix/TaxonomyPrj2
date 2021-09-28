$(document).ready(function () {

  


    addTableUsers();
   // redactUsers();
   // deleteUsers();

});

function addTableUsers()
{
    $.get('/Account/PartialUsers', {}, function (data) {
        $('#allUsers').html(data);
        redactUsers();
        deleteUsers();
    });
}

function redactUsers()
{
    $('.redactUser').on('click', function () {
        var clickLogin = $(this).attr('data-id');
        
        $.get('/Account/PartialEdit', { login: clickLogin }, function (data) {
            $('#ModalViewC').html(data);
            $("#myModalC").modal('show');
            $.validator.unobtrusive.parse("#needsValidationEdit");

            $('#editButton').on('click', function () {
                var login = $('#Login').val();
                var eMail = $('#EMail').val();
                var year = $('#Year').val();
                var role = $('#Role').val();

                if ($('#needsValidationEdit').valid())
                {
                    
                    $.post('/Account/PartialEdit', { login, eMail, year, role }).done(function (data) {
                        if (data.save == true) {
                            $("#myModalC").modal('hide');
                            addTableUsers();
                        }
                        else {
                            $('#errInfo').html(data.error);
                        }

                    }).fail();
                }
               

                
            });
        });
    });
}

function deleteUsers()
{
    $('.deleteUser').on('click', function () {
        var login = $(this).attr('data-id');
       /* var activeUser = $(this).attr('data-active');
        if (login == activeUser) {
            //$('#question').addClass('redText');
            // $('#question').html("Error");
            alert('Error');
        }
        else
        {
            alert('All OK');
        }*/
        $.get('/Account/PartialDelete', { login }, function (data) {
            $('#ModalViewC').html(data);
            $("#myModalC").modal('show');

            $('#NoQuestionButton').on('click', function () {
                $("#myModalC").modal('hide');
            });

            $('#YesQuestionButton').on('click', function () {
                $.post('/Account/DeleteUser', { login }).done( function (data) {
                    
                    if (data.save == true) {
                        $("#myModalC").modal('hide');
                        addTableUsers();
                    }
                    else {
                        $('#errInfo').html(data.error);
                    }
                }).fail();
            });
        });
    });
}