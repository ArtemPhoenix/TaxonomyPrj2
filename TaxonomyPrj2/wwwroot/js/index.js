$(document).ready(function () {
    CheckingTheRoleChange();
  
   /* var role = $('#roleUser').val();
    if (role == "CommonUser")
    {
        $('#roleUserLayout').html("Права доступа: CommonUser");
    } 
   /* if (role == "CommonUser")
    {
        $('#workWithUser').html("");
    }
    if (role == "Admin")
    {
        $.get('/Home/PartialWorkWithUser', {}, function (data) {
            $('#workWithUser').html(data);
        });
    }*/

    //выделение категории в таблице
    /*$('.choiseCategory').on('click', function (e) {
        $('#tableCategories .table').removeClass('marked');
        $(this).addClass('marked');
    });*/
    
   // $('.choiseCategory').removeClass('choiseMarker');
   // $(this).addClass('choiseMarker');

    //запуск поиска
    StartSearch();
    $('#startButtonSeach').on('click', function () {
        $.get('/Home/PartialSearchStart', {}, function (data) {
            //alert(data);
            $('#startSearch').html(data);
           
            StartSearch();
            $('#exitStartSeach').on('click', function () {
                $('#searchResult1').html("");
                $('#startSearch').html("");
               
            });
        });
    });

    //small поиск
    StartSearchSmall();

   // обновление списка организмов по категории
    $('.choiseCategory').on('click', function () {
        var clickId = $(this).attr('data-id');
        $('#SearchIdCategiryOrganismSmall').val(clickId);
        $('.choiseCategory').removeClass('choiseMarker');
        $(this).addClass('choiseMarker');
        loadTableOrganisms(clickId);
       
    });

    addButton();

 
   // $('div:contains("Петр")')	вернет все div - элементы, внутри которых найдется строка Петр.

      

   // document.getElementById('message').innerHTML = a;
    //$("#message1").text("***********");
   

   
    
    //$("#message").fadeOut("slow");
});

function CheckingTheRoleChange()
{
    $(document).on('click', function () {
        var constRole = $('#constRole').val();

        $.get('/Home/returnRole', { role: constRole }, function (data) {
            //alert("проверка прошла");
            if (!data.result) {

                alert("Внимание! У пользователя сменилась роль");
                $('.hideNotAut').html("");
                window.location.replace("https://localhost:44333/Account/Login?ReturnUrl=%2F");

            }

        });
    });
}


function StartSearchSmall()
{
    $('#startSeachCategorySmall').on('click', function () {
        var searchNameOrganism = $('#SearchNameOrganismSmall').val();
        var searchCountFromtOrganism = $('#SearchCountFromOrganismSmall').val();
        //if (SearchCountFromtOrganism == "") { SearchCountFromtOrganism = "-1"; }
        var searchCountTotOrganism = $('#SearchCountToOrganismSmall').val();
        //if (SearchCountTotOrganism == "") { SearchCountTotOrganism = "-1"; }
        var searchDateFromOrganism = $('#SearchDateFromOrganismSmall').val();
        var searchDateToOrganism = $('#SearchDateToOrganismSmall').val();
        var searchIdCategiryOrganism = $('.choiseMarker').attr('data-id');
       
        $.get('/Home/PartialSearchResult', { searchNameOrganism, searchCountFromtOrganism, searchCountTotOrganism, searchDateFromOrganism, searchDateToOrganism, searchIdCategiryOrganism }, function (data) {
            //alert(data);
            $('#tableOrganisms').html(data);
            addButton();
            
        });

    });
}

function StartSearch()
{
   
    $('#startSeach').on('click', function () {
        var searchNameOrganism = $('#SearchNameOrganism').val();
        var searchCountFromtOrganism = $('#SearchCountFromtOrganism').val();
        //if (SearchCountFromtOrganism == "") { SearchCountFromtOrganism = "-1"; }
        var searchCountTotOrganism = $('#SearchCountTotOrganism').val();
        //if (SearchCountTotOrganism == "") { SearchCountTotOrganism = "-1"; }
        var searchDateFromOrganism = $('#SearchDateFromOrganism').val();
        var searchDateToOrganism = $('#SearchDateToOrganism').val();
        var searchIdCategiryOrganism = null;
        $.get('/Home/PartialSearchResult', { searchNameOrganism, searchCountFromtOrganism, searchCountTotOrganism, searchDateFromOrganism, searchDateToOrganism, searchIdCategiryOrganism }, function (data) {
            $('#searchResult1').html(data);
            $('#exitSeach').on('click', function () {
                $('#searchResult1').html("");
            });
        });

    });
}

function loadTableOrganisms(id)
{
    $.get('/Organism/PartialOrganismTable', { categoryId: id }, function (data) {
        
        $('#tableOrganisms').html(data);
        addButton();
    });
}

function addButton()
{
    
      // модальное окно редактирования организма
    $('.redactOrganism').on('click', function () {
        var clickId = $(this).attr('data-id');
        
        $.get('/Organism/PartialEdit', { id: clickId }, function (data) {
            $('#ModalView').html(data);
            $("#myModal").modal('show');

            $.validator.unobtrusive.parse("#needsValidationOrganism"); // прицепить валидацию к форме
            //-------------------------
            // закрытие м/окна по кнопке
            $('#exitPartialButton').on('click', function () {
                
                $("#myModal").modal('hide');

            });
            
            //-------------------------

            $('#redactOrganismButton').on('click', function () {
                var clickId = $(this).attr('data-id');
                var clickName = $('#Name').val();
                var clickCategoryId = $('#newCategoryId').val(); 
                var clickStartResearch = $('#StartResearch').val();
                var clickCountSample = $('#CountSample').val();
                var clickDescription = $('#Description').val();

              
                if ($('#needsValidationOrganism').valid()) {
                    $.post('/Organism/PartialEdit', { id: clickId, name: clickName, categoryId: clickCategoryId, startResearch: clickStartResearch, countSample: clickCountSample, description: clickDescription }, function (data) {
                        $('#ModalView').html(data);
                        $("#myModal").modal('show');
                        //-------------------------
                        // закрытие м/окна и обновление списка организмов по категории
                        //  exitPartialButton
                        $('#exitPartialButton').on('click', function () {
                            var fsd = 90;
                            //var clickId = $(this).attr('data-id');
                            $("#myModal").modal('hide');

                            var clickId = $(this).attr('data-id');

                            loadTableOrganisms(clickId);

                        });
                        //-------------------------
                    });
                }
                
               
            });

            
        })

        
       
    });

     // модальное окно создания организма
    $('.createOrganism').on('click', function () {
        $.get('/Organism/PartialCreate', function (data) {
            $('#ModalView').html(data);
            $("#myModal").modal('show');
           
            //-------------------------
            // закрытие м/окна по кнопке
            $('#exitPartialButton').on('click', function () {
                $("#myModal").modal('hide');

            });
            //-------------------------
            $.validator.unobtrusive.parse("#needsValidationOrganism"); // прицепить валидацию к форме

            $('#createOrganismButton').on('click', function () {
                var clickName = $('#Name').val();
                var clickCategoryId = $('#newCategoryId').val(); 
                var clickStartResearch = $('#StartResearch').val();
                var clickCountSample = $('#CountSample').val();
                var clickDescription = $('#Description').val();
                
                
                if ($('#needsValidationOrganism').valid()) {
                     
                    $.post('/Organism/PartialEdit', { id: '0', name: clickName, categoryId: clickCategoryId, startResearch: clickStartResearch, countSample: clickCountSample, description: clickDescription }, function (data) {
                        $('#ModalView').html(data);
                        $("#myModal").modal('show');
                        //-------------------------
                        // закрытие м/окна и обновление списка организмов по категории
                        $('#exitPartialButton').on('click', function () {
                            $("#myModal").modal('hide');

                            var clickId = $(this).attr('data-id');

                            loadTableOrganisms(clickId);
                            /* $.get('/Organism/PartialOrganismTable', { categoryId: clickId }, function (data) {
                                 alert("******");
                                 $('#tableOrganisms').html(data);*/
                        });
                        //-------------------------
                        
                    });
                }
               
               
                
            });
        });
    });

    // модальное окно удаления организма
    $('.deleteOrganism').on('click', function () {
        var clickId = $(this).attr('data-id');

        $.get('/Organism/PartialQuestion', { id: clickId }, function (data) {
            $('#ModalView').html(data);
            $("#myModal").modal('show');

            // закрытие м/окна по кнопке
            $('#NoQuestionButton').on('click', function () {
                $("#myModal").modal('hide');
            });
            //-------------------------

            $('#YesQuestionButton').on('click', function () {
                var clickCategoryId = $(this).attr('data-Categoryid');
                $.post('/Organism/PartialQuestion', { id: clickId }, function (data) {
                    
                    if (data.save == true) {
                        $("#myModal").modal('hide');

                        //обновление списка организмов по категории
                        loadTableOrganisms(clickCategoryId);
                        
                        
                    }
                });
            });
        });
    });

}
