function RouteClient(url, cookieService)
{
    var self = this;
    this.cookieService = cookieService;

    //AKK.Controllers.RouteController.GetRoutes
    this.getRoutes = function(grade, sectionId, sortBy, success)
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            data: 
            {
                gradeId: grade,
                sectionId: sectionId,
                sortBy: sortBy
            },
            success: success
        });
    };

    //AKK.Controllers.RouteController.GetRoutes
    this.searchRoutes = function(searchstring, success) {
       return $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            data: 
            {
                searchstr: searchstring,
                maxresults: 0
            },
            success : success
        });
    }

    //AKK.Controllers.RouteController.GetRoute
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

    //AKK.Controllers.RouteController.GetImage
    this.getImage = function(id, success) {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url + "/" + id + "/image",
            success: success
        });
    }

    //AKK.Controllers.RouteController.AddComment
    this.addComment = function(formdata, routeId, success, error) {
        formdata.append('token', self.cookieService.getToken());
        formdata.append('id', routeId);

        $.ajax({
            url: url + "/comment", 
            type: 'POST',
            success: success,
            error: error,
            data: formdata,
            cache: false,
            contentType: false,
            processData: false
        });
    }

    //AKK.Controllers.RouteController.RemoveComment
    this.removeComment = function(id, routeId, success) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: url + "/comment/remove",
            data:
            {
                token: self.cookieService.getToken(),
                commentId: id,
                routeId: routeId,
            },
            success: success
        });
    }

    //AKK.Controllers.RouteController.AddRoute
    this.addRoute = function(sectionId, name, author, holdColor, gradeId, tape,note,image, success)
    {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: url,
            data:
            {
                token: self.cookieService.getToken(),
                sectionId: sectionId,
                author: author,
                name: name,
                gradeId: gradeId,
                colorOfHolds: holdColor,
                colorOfTape: tape,
                note: note,
                image: image
            },
            success: success
        });
    };

    //AKK.Controllers.RouteController.UpdateRoute
    this.updateRoute = function(routeId, sectionId, author, name, holdColor, gradeId, tape, note, image, success)
    {
        $.ajax({
            type: "PATCH",
            dataType: "json",
            url: url + "/" + routeId,
            data:
            {
                token: self.cookieService.getToken(),
                id: routeId,
                sectionId: sectionId,
                author: author,
                name: name,
                colorOfHolds: holdColor,
                gradeId: gradeId,
                colorOfTape: tape,
                note: note,
                image: image
            },
            success: success
        });
    };

    //AKK.Controllers.RouteController.DeleteRoute
    this.deleteRoute = function(id, success)
    {
        $.ajax({
            type: "DELETE", 
            dataType: "json",
            url: url + "/" + id,
            data:
            {
                token: self.cookieService.getToken(),
                id: id
            },
            success: success
        });
    };

    //AKK.Controllers.RouteController.SetRating
    this.setRating = function(routeId, ratingValue, success)
    {
        $.ajax({
            type: "PUT", 
            dataType: "json",
            url: url + "/" + routeId + "/rating",
            data:
            {
                routeId: routeId,
                token: self.cookieService.getToken(),
                ratingValue: ratingValue
            },
            success: success
        });
    }
}

function SectionClient(url, cookieService)
{
    var self = this;
    this.cookieService = cookieService;

    //AKK.Controllers.SectionController.GetAllSections
    this.getAllSections = function(success)
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            success: success
        });
    };

    //AKK.Controllers.SectionController.GetSection
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

    //AKK.Controllers.SectionController.AddSection
    this.addSection = function(name, success)
    {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: url,
            data:
            {
                token: self.cookieService.getToken(),
                name: name
            },
            success: success
        });
    };

    //AKK.Controllers.SectionController.DeleteSection
    this.deleteSection = function(name, success)
    {
        $.ajax({
            type: "DELETE", 
            dataType: "json",
            url: url + "/" + name,
            data:
            {
                token: self.cookieService.getToken(),
                name: name
            },
            success: success
        });
    };

    //AKK.Controllers.SectionController.DeleteSectionRoutes
    this.deleteSectionRoutes = function(name, success)
    {
        $.ajax({
            type: "DELETE",
            dataType: "json",
            url: url + "/" + name + "/routes",
            data:
            {
                token: self.cookieService.getToken(),
                name: name
            },
            success: success
        });
    };

    //AKK.Controllers.SectionController.UpdateSection
    this.renameSection = function(sectionId, newName, success)
    {
        $.ajax({
            type: "PATCH",
            dataType: "json",
            url: url + "/" + sectionId,
            data:
            {
                token: self.cookieService.getToken(),
                name: newName
            },
            success: success
        });
    };
}

function GradeClient(url, cookieService)
{
    var self = this;
    this.cookieService = cookieService;

    //AKK.Controllers.GradeController.GetAllGrades
    this.getAllGrades = function(success)
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            success: success
        });
    };

    //AKK.Controllers.GradeController.AddGrade
    this.addGrade = function(grade, success)
    {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: url,
            data:
            {
                token: self.cookieService.getToken(),
                grade: grade
            },
            success: success
        });
    };

    //AKK.Controllers.GradeController.GetGrade
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

    //AKK.Controllers.GradeController.UpdateGrade
    this.updateGrade = function(grade, success)
    {
        $.ajax({
            type: "PATCH",
            dataType: "json",
            url: url+"/"+grade.id,
            data:
            {
                token: self.cookieService.getToken(),
                grade: grade
            },
            success: success
        });
    };

    //AKK.Controllers.GradeController.SwapGrades
    this.swapGrades = function(gradeAId, gradeBId, success)
    {
        $.ajax({
            type: "PATCH",
            dataType: "json",
            url: url+`/${gradeAId}/swap/${gradeBId}`,
            data:
            {
                token: self.cookieService.getToken(),
            },
            success: success
        });
    };

    //AKK.Controllers.GradeController.DeleteGrade
    this.deleteGrade = function(gradeId, success)
    {
        $.ajax({
            type: "DELETE",
            dataType: "json",
            url: url + "/" + gradeId,
            data:
            {
                token: self.cookieService.getToken(),
                id: gradeId
            },
            success: success
        });
    };
}

function MemberClient(url, cookieService)
{
    var self = this;
    this.cookieService = cookieService;

    //AKK.Controllers.MemberController.Login
    this.logIn = function(username, password, success)
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url + "/login",
            data:
            {
                username: username,
                password: password
            },
            success: function(response) {
                if(response.success)
                    self.cookieService.setToken(response.data);
                success(response);
            }
        });
    };

    //AKK.Controllers.MemberController.Logout
    this.logOut = function(success)
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url + "/logout",
            data:
            {
                token: self.cookieService.getToken()
            },
            success: function(response) {
                if(response.success)
                    self.cookieService.expireToken();
                success(response);
            }
        });
    };

    //AKK.Controllers.MemberController.AddMember
    this.register = function(displayname, username, password, success)
    {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: url,
            data:
            {
                displayname: displayname,
                username: username,
                password: password
            },
            success: function(response) {
                if(response.success)
                    self.cookieService.setToken(response.data);
                success(response);
            }
        });
    };

    //AKK.Controllers.MemberController.GetMemberInfo
    this.getMemberInfo = function(success) 
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url + "/" + self.cookieService.getToken(),
            success: success
        });
    };

    //AKK.Controllers.MemberController.GetMemberRatings
    this.getMemberRatings = function(success)
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url + "/" + self.cookieService.getToken() + "/ratings",
            success: success
        });
    }

    //AKK.Controllers.MemberController.GetAllMembers
    this.getAllMembers = function(success)
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            data: 
            {
                token: self.cookieService.getToken()
            },
            success: success
        });
    };

    //AKK.Controllers.MemberController.ChangeRole
    this.changeRole = function(memberid, role, success)
    {
        $.ajax({
            type: "PATCH",
            dataType: "json",
            url: url + "/role",
            data:
            {
                token: self.cookieService.getToken(),
                memberid: memberid,
                role: role
            },
            success: success
        });
    }; 
}

function HoldClient(url, cookieService)
{
    var self = this;
    this.cookieService = cookieService;

    //AKK.Controllers.HoldColorController.GetAllHoldColors
    this.getAllHolds = function(success)
    {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            success: success
        });
    };

    //AKK.Controllers.HoldColorController.AddHoldColor
    this.addHold = function(hold, success)
    {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: url,
            data:
            {
                token: self.cookieService.getToken(),
                holdcolor: hold
            },
            success: success
        });
    };

    //AKK.Controllers.HoldColorController.DeleteHoldColor
    this.deleteHold = function(holdId, success)
    {
        $.ajax({
            type: "DELETE",
            dataType: "json",
            url: url + "/" + holdId,
            data:
            {
                token: self.cookieService.getToken(),
                id: holdId
            },
            success: success
        });
    };
}

function Client(routeUrl, sectionUrl, gradeUrl, memberUrl, holdUrl, cookieService)
{
    this.routes = new RouteClient(routeUrl, cookieService);
    this.sections = new SectionClient(sectionUrl, cookieService);
    this.grades = new GradeClient(gradeUrl, cookieService);
    this.members = new MemberClient(memberUrl, cookieService);
    this.holds = new HoldClient(holdUrl, cookieService);
}