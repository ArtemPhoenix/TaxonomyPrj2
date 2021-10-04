$(document).ready(function () {

    $('#startSeach').on('click', function () {
       
        var CountFromOrganism = $('#SearchCountFromOrganism').val();
        var CountToOrganism = $('#SearchCountToOrganism').val();
        var DateFromOrganism = $('#SearchDateFromOrganism').val();
        var DateToOrganism = $('#SearchDateToOrganism').val();
       
        $.get('/Information/PartialSearch', { CountFromOrganism, CountToOrganism, DateFromOrganism, DateToOrganism }, function (data) {
            
            $('#resultTable').html(data);
           
        });

    });

});