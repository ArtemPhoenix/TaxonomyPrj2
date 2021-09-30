$(document).ready(function () {

    $('#createCategory').on('click', function () {
        var clikId = $(this).attr('data-id');
        
        $.get('/Category/PartialCreate', { id: clikId }, function (data) {
            $('#ModalViewC').html(data);
            $("#myModalC").modal('show');

            $.validator.unobtrusive.parse("#needsValidationCreate"); // прицепить валидацию к форме

            $('#exitPartialButton').on('click', function () {
                $("#myModalC").modal('hide');
            });

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
               

               

            });
        });
        
    });

    loadInfoTree();
   
});






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
                $.post('/Category/Delete', { id: clickId }, function (data) {
                    if (data.error != null && data.error.length > 0)
                    {
                        alert(data.error);
                    }
                    else
                    {
                        if (data.save != "успешно")
                        {
                            alert(data.save);
                        }

                    }
                   
                    $("#myModalC").modal('hide');
                    
                    loadInfoTree();
                }); 
            });
        });
        

    });

    $('.redactCategory').on('click', function (e) {
        var clickId = $(this).attr('data-id');
       
        //-----------------------------
        $.get('/Category/PartialEdit', { id: clickId }, function (data) {
            $('#ModalViewC').html(data);
            $("#myModalC").modal('show');

            $.validator.unobtrusive.parse("#needsValidationEdit"); // прицепить валидацию к форме

            $('#exitPartialButton').on('click', function () {
                $("#myModalC").modal('hide');
            });

            $('#editButton').on('click', function () {
                var id = $('#id').val();
                var parentId = $('#ParentId').val();
                var name = $('#Name').val();
                var nameCat = $('#NameCat').val();
                var description = $('#Description').val();

                if ($('#needsValidationEdit').valid()) {
                    $.post('/Category/PartialEdit', { id, parentId, nameCat, name, description }, function (data) {
                        $("#myModalC").modal('hide');
                        
                        loadInfoTree();
                    });
                }
              
            });
        });
        //-----------------------
        
    });

       
}



function loadInfoTree()
{
   

    $.get('/Category/PartialInfoTree', function (data) {
        $('#infoTree').html(data);
       
        addButton();
    });

    
}

function clickUL(selector)  // клик по категории
{
    var xFalse = $(selector).parent().children("ul").hasClass("hideChild");

    if (xFalse)
    {
        $(selector).parent().children("ul").removeClass("hideChild");
        $(selector).html("-");
    }
    else
    {
        $(selector).parent().children("ul").addClass("hideChild");
        $(selector).html("+");
    }
     
}

function clickDescr(selector) // клик по кнопке описание
{
    var xFalse = $(selector).parent().children("div").hasClass("hideChild");

    if (xFalse) {
        $(selector).parent().children("div").removeClass("hideChild");
    }
    else
    {
        $(selector).parent().children("div").addClass("hideChild");
    }
}