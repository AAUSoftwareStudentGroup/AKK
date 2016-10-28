$(document).ready(function() {

    sendApiRequest('GET', null);

    $('form').change(function(e) {
        var parameters = {
            grade: $( this ).find( "select[name='grade']" ).val(),
            section: $( this ).find( "select[name='section']" ).val(),
            orderBy: $( this ).find( "select[name='sortBy']" ).val()
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

function createRoutes(response) {
    console.log(response);
    var routeList = $('.list');
    $('.route').remove();
    
    response.data.forEach(function(route) {
        var nextRoute = new Route(route['grade'], route['name'], route['sectionID'], route['author'], route['colorOfHolds'], route['date']);
        nextRoute.route.appendTo(routeList);
    });
}