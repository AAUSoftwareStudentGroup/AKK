using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using AKK.Controllers.ApiResponses;
using AKK.Models;
using AKK.Models.Repositories;
using AKK.Services;


namespace AKK.Controllers
{
    [Route("api/holdColor")]
    public class HoldColorController
    {
        private readonly IRepository<HoldColor> _holdColorRepository;
        private readonly IAuthenticationService _authenticationService;

        public HoldColorController(IRepository<HoldColor> holdColorRepository,
            IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _holdColorRepository = holdColorRepository;
        }

        [HttpGet]
        public ApiResponse<IEnumerable<HoldColor>> GetAllHoldColors()
        {
            var holdsColors = _holdColorRepository.GetAll();

            if (!holdsColors.Any())
            {
                return new ApiErrorResponse<IEnumerable<HoldColor>>("No colors found");
            }
            return new ApiSuccessResponse<IEnumerable<HoldColor>>(holdsColors);
        }


        [HttpPost]
        public ApiResponse<HoldColor> AddHoldColor(string token, HoldColor holdcolor)
        {
            if (!_authenticationService.HasRole(token, Role.Admin))
            {
                return new ApiErrorResponse<HoldColor>("You need to be logged in as an administrator to add a new grade");
            }
            if (holdcolor.ColorOfHolds == default(Color))
            {
                return new ApiErrorResponse<HoldColor>("A color must be selected");
            }

            try
            {
                _holdColorRepository.Add(holdcolor);
                _holdColorRepository.Save();
                return new ApiSuccessResponse<HoldColor>(holdcolor);
            }
            catch
            {
                return new ApiErrorResponse<HoldColor>("Failed to add holdcolor");
            }
        }

        [HttpDelete("{id}")]
        public ApiResponse<HoldColor> DeleteHoldColor(string token, Guid id)
        {
            if (!_authenticationService.HasRole(token, Role.Admin))
            {
                return new ApiErrorResponse<HoldColor>("You need to be logged in as an administrator to add a new grade");
            }
            var holdColor = _holdColorRepository.Find(id);
            if (holdColor == null)
            {
                return new ApiErrorResponse<HoldColor>("Could not find holdColor");
            }
            HoldColor holdColorCopy = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(holdColor)) as HoldColor;
            _holdColorRepository.Delete(id);
            try
            {
                _holdColorRepository.Save();
                return new ApiSuccessResponse<HoldColor>(holdColorCopy);
            }
            catch
            {            
               return new ApiErrorResponse<HoldColor>("Failed to delete Holdcolor");
            }
        }
    }

}