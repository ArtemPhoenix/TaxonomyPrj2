$(document).ready(function () {

    $(document).on('click', function () {
        var constRole = $('#constRole').val();
        
        $.get('/Category/returnRole', { role: constRole }, function (data) {
            //alert("проверка прошла");
            if (!data.result) {
               
                alert("Внимание! У пользователя сменилась роль");
                $('.hideNotAut').html("");
                window.location.replace("https://localhost:44333/Account/Login?ReturnUrl=%2F");
              
            }
           
        });
    });
  //  $('#roleUserLayout').html("Admin");
   /* $.get('/Home/PartialWorkWithUser', {}, function (data) {
        $('#workWithUser').html(data);
    });
*/


    $('#createCategory').on('click', function () {
        var clikId = $(this).attr('data-id');
        //CreateModal(clikId);
        $.get('/Category/PartialCreate', { id: clikId }, function (data) {
            $('#ModalViewC').html(data);
            $("#myModalC").modal('show');

            $.validator.unobtrusive.parse("#needsValidationCreate"); // прицепить валидацию к форме



            $('#createButton').on('click', function () {
                var parentId = $('#ParentId').val();
                var name = $('#Name').val();
                var nameCat = $('#NameCat').val();
                var description = $('#Description').val();
                

                if ($('#needsValidationCreate').valid()) {
                   
                    $.post('/Category/PartialCreate', { parentId, name, nameCat, description })
                        .done(function (data) {
                            alert("defsedf");
                            if (data.save) {
                                $("#myModalC").modal('hide');

                                loadInfoTree();

                            }
                        })
                        .fail();
                }
               

                //  $('#needsValidationCreate').validate();

            });
        });
        
    });

    loadInfoTree();
        
    

});




function addButtonTree()  // descript
{
    $('.descriptionCategory').on('click', function (e) {
        var clickId = $(this).attr('data-id');

         alert(clickId);
        // prompt(clickId);
        // confirm(clickId);

        $.get('/Category/PartialDescription', { id: clickId }, function (data) {
            
            $('#ModalViewC').html(data);
            $("#myModalC").modal('show');
            //-------------------------
            // закрытие м/окна по кнопке
            $('#exitPartialButtonCategory').on('click', function () {
               
                $("#myModalC").modal('hide');
            });
            //-------------------------

        });

    });

    $('.choiseCategory').on('click', function (e) {
        var clickId = $(this).attr('data-id');
      
        $('.choiseCategory').removeClass('choiseMarker');
        $(this).addClass('choiseMarker');

        loadInfoCategory(clickId);
    });
}

function addButton()  // delete & redact
{
    $('.deleteCategory').on('click', function (e) {
        var clickId = $(this).attr('data-id');
        $.get('/Category/PartialDelete', { id: clickId }, function (data) {
            $('#ModalViewC').html(data);
            $("#myModalC").modal('show');
            // закрытие м/окна по кнопке
            $('#NoQuestionButton').on('click', function () {
                $("#myModalC").modal('hide');
            });
            //-------------------------
            $('#YesQuestionButton').on('click', function () {
                var clickId = $(this).attr('data-id');
                $.post('/Category/PartialDelete', { id: clickId }, function (data) {
                    if (data.save != true) { alert("Данная подкатегория имеет дочерние подкатегории, удаление невозможно!"); }
                    $("#myModalC").modal('hide');
                    //loadInfoCategory(1);
                    loadInfoTree();
                }); 
            });
        });
        

    });

    $('.redactCategory').on('click', function (e) {
        var clickId = $(this).attr('data-id');
        //alert(clickId);
        //-----------------------------
        $.get('/Category/PartialEdit', { id: clickId }, function (data) {
            $('#ModalViewC').html(data);
            $("#myModalC").modal('show');

            $.validator.unobtrusive.parse("#needsValidationEdit"); // прицепить валидацию к форме
            
            $('#editButton').on('click', function () {
                var id = $('#id').val();
                var parentId = $('#ParentId').val();
                var name = $('#Name').val();
                var nameCat = $('#NameCat').val();
                var description = $('#Description').val();

             
                if ($('#needsValidationEdit').valid()) {
                    $.post('/Category/PartialEdit', { id, parentId, nameCat, name, description }, function (data) {
                        $("#myModalC").modal('hide');
                        //loadInfoCategory(id);
                        loadInfoTree();
                    });
                }
              
               
               
                    

            });
        });
        //-----------------------
        
    });

       
}

function loadInfoCategory(id)
{

    //alert("info");
    $.get('/Category/PartialInfoCategory', { id }, function (data) {
        $('#infoCategory').html(data);
       
        addButton();
    });

    
}

function loadInfoTree()
{
    $.get('/Category/PartialInfoTree', function (data) {
        //alert(data);
        $('#infoTree').html(data);
        addButtonTree();
        addButton();
    });
}