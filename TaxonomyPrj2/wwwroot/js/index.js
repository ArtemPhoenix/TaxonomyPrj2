$(document).ready(function () {
    
    //запуск поиска
    StartSearch();
    $('#startButtonSeach').on('click', function () {
        $.get('/Home/PartialSearchStart', {}, function (data) {
           
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

});


function StartSearchSmall()
{
    $('#startSeachCategorySmall').on('click', function () {
        var NameOrganism = $('#SearchNameOrganismSmall').val();
        var CountFromtOrganism = $('#SearchCountFromOrganismSmall').val();
        var CountTotOrganism = $('#SearchCountToOrganismSmall').val();
        var DateFromOrganism = $('#SearchDateFromOrganismSmall').val();
        var DateToOrganism = $('#SearchDateToOrganismSmall').val();
        var IdCategiryOrganism = $('.choiseMarker').attr('data-id');
       
        $.get('/Home/PartialSearchResult', { NameOrganism, CountFromtOrganism, CountTotOrganism, DateFromOrganism, DateToOrganism, IdCategiryOrganism }, function (data) {
            
            $('#tableOrganisms').html(data);
            addButton();
            
        });

    });
}

function StartSearch()
{
   
    $('#startSeach').on('click', function () {
        var NameOrganism = $('#SearchNameOrganism').val();
        var CountFromtOrganism = $('#SearchCountFromtOrganism').val();        
        var CountTotOrganism = $('#SearchCountTotOrganism').val();       
        var DateFromOrganism = $('#SearchDateFromOrganism').val();
        var DateToOrganism = $('#SearchDateToOrganism').val();
        var IdCategiryOrganism = null;
        $.get('/Home/PartialSearchResult', { NameOrganism, CountFromtOrganism, CountTotOrganism, DateFromOrganism, DateToOrganism, IdCategiryOrganism }, function (data) {
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
                $.post('/Organism/Delete', { id: clickId }, function (data) {

                    if (data.error != null && data.error.length > 0) {
                        alert(data.error);
                    }
                    else {
                        if (data.save != "успешно") {
                            alert(data.save);
                        }

                    }
                  
                        $("#myModal").modal('hide');

                        //обновление списка организмов по категории
                        loadTableOrganisms(clickCategoryId);
                        
                        
                  
                });
            });
        });
    });

}
