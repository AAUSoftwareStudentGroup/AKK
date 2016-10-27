$(document).ready(function() {

    sendApiRequest('GET', null);

    $('form').change(function(e) {
        var parameters = {
            grade: $( this ).find( "select[name='grade']" ).val(),
            section: $( this ).find( "select[name='section']" ).val(),
            sortBy: $( this ).find( "select[name='sortBy']" ).val()
        };
        
        sendApiRequest('GET', parameters);
    });
});

function sendApiRequest(type, parameters) {
    $.ajax({
      type: type,
      url: 'api/route',
      data: parameters,
      success: createRoutes,
      dataType: 'json'
    });
}

function createRoutes(data) {
    var routeList = $('.list');
    $('.route').remove();
    
    data.forEach(function(data) {
        var nextRoute = new Route(data['grade'], data['name'], data['sectionID'], data['author'], data['colorOfHolds'], data['date']);
        nextRoute.route.appendTo(routeList);
    });
}
