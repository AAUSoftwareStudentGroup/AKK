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

    this.getImage = function(id, success) {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url + "/" + id + "/image",
            success: success
        });
    }

    this.addRoute = function(sectionId, name, author, holdColor, grade, tape, success)
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
                colorOfHolds: holdColor,
                ColorOfTape: tape
            },
            success: success
        });
    };
    this.updateRoute = function(routeId, sectionId, name, author, holdColor, grade, tape, image, success)
    {
        $.ajax({
            type: "PATCH",
            dataType: "json",
            url: url + "/" + routeId,
            data:
            {
                id: routeId,
            //    sectionName: sectionName,
                sectionId: sectionId,
                name: name,
                author: author,
                colorOfHolds: holdColor,
                grade: grade,
                colorOfTape: tape,
                image: image
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
            url: url + "/" + name + "/routes",
            data:
            {
                name: name
            },
            success: success
        });
    };

    this.renameSection = function(sectionId, newName, success)
    {
        $.ajax({
            type: "PATCH",
            dataType: "json",
            url: url,
            data:
            {
                sectionId: sectionId,
                newName: newName
            },
            success: success
        });
    };
}

function GradeClient(url)
{
    this.getAllGrades = function(success)
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            success: success
        });
    };

    this.addGrade = function(grade, success)
    {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: url,
            data:
            {
                grade: grade
            },
            success: success
        });
    };

    this.getGrade = function(gradeId, success)
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            data:
            {
                id: gradeId
            },
            success: success
        });
    };

    this.deleteGrade = function(gradeId, success)
    {
        $.ajax({
            type: "DELETE",
            dataType: "json",
            url: url + "/" + gradeId,
            data:
            {
                id: gradeId
            },
            success: success
        });
    };
}

function Client(routeUrl, sectionUrl, gradeUrl)
{
    this.routes = new RouteClient(routeUrl);
    this.sections = new SectionClient(sectionUrl);
    this.grades = new GradeClient(gradeUrl);
}