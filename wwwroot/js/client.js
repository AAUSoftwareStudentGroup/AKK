function RouteClient(url)
{
    this.getRoutes = function(grade, sectionId, sortBy, success)
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            data: 
            {  
                grade: grade,
                sectionId: sectionId,
                sortBy: sortBy
            },
            success: success
        });
    };

    this.getRoute = function(id, success)
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url + "/" + id,
            data:
            {
                id: id
            },
            success: success
        });
    };

    this.addRoute = function(sectionId, name, author, holdColor, grade, success)
    {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: url,
            data:
            {
                sectionId: sectionId,
                name: name,
                author: author,
                grade: grade,
                colorOfHolds: holdColor
            },
            success: success
        });
    };

    this.updateRoute = function(routeId, sectionId, name, author, holdColor, grade, success)
    {
        $.ajax({
            type: "PATCH",
            dataType: "json",
            url: url + "/" + routeId,
            data:
            {
                routeId: routeId,
            //    sectionName: sectionName,
                sectionId: sectionId,
                name: name,
                author: author,
                colorOfHolds: holdColor,
                grade: {difficulty: grade.value}
            },
            success: success
        });
    };

    this.deleteRoute = function(id, success)
    {
        $.ajax({
            type: "DELETE", 
            dataType: "json",
            url: url + "/" + id,
            data:
            {
                id: id
            },
            success: success
        });
    };
}

function SectionClient(url)
{
    this.getAllSections = function(success)
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            success: success
        });
    };

    this.getSection = function(name, success)
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url + "/" + name,
            data:
            {
                name: name
            },
            success: success
        });
    };


    this.addSection = function(name, success)
    {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: url,
            data:
            {
                name: name
            },
            success: success
        });
    };

    this.deleteSection = function(name, success)
    {
        $.ajax({
            type: "DELETE", 
            dataType: "json",
            url: url + "/" + name,
            data:
            {
                name: name
            },
            success: success
        });
    };

    this.deleteSectionRoutes = function(name, success)
    {
        $.ajax({
            type: "DELETE",
            dataType: "json",
            url: url + "/" + name,
            data:
            {
                name: name
            },
            success: success
        });
    };
}

function Client(routeUrl, sectionUrl)
{
    this.routes = new RouteClient(routeUrl);
    this.sections = new SectionClient(sectionUrl);
}