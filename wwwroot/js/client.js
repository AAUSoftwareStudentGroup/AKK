function RouteClient(url)
{
    this.getAll = function(grade, section, sortBy, success)
    {
        $.ajax({
          dataType: "json",
          url: url,
          data: 
          {  
              grade : grade,
              section : section,
              sortBy : sortBy
          },
          success: success
        });
    }
}

function Client()
{
    this.routes = new RouteClient(API_ROUTE_URL);
}