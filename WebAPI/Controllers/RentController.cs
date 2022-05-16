﻿using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/rent")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly RentService rentService;

        public RentController(RentService rentService)
        {
            this.rentService = rentService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("games")]
        public async Task<ActionResult<List<Game>>> GetAvailableGames([FromQuery] string dateFrom, [FromQuery] string dateTo)
        {
            var games = await rentService.GetGamesInTimespan(DateTime.Parse(dateFrom), DateTime.Parse(dateTo));
            return Ok(games);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ActionResult<Rent>> NewRent([FromBody] NewRentDto newRent)
        {
            // create new Rent (status = Pending)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var rent = await rentService.CreateNewRent(userId, DateTime.Parse(newRent.DateFrom), DateTime.Parse(newRent.DateTo), newRent.Games);
            // create new Rent Comment
            var comment = await rentService.AddCommentToRent(userId, newRent.Comment, rent);
            // return new Rent
            return Ok(rent);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("all")]
        public async Task<ActionResult<RentDto>> ListRents()
        {
            var dbRents = await rentService.ListRents();
            var rents = dbRents.Select(r =>
            {
                return new RentDto()
                {
                    Id = r.ID,
                    DateFrom = r.DateFrom,
                    DateTo = r.DateTo,
                    Renter = r.Renter,
                    Status = r.Status.ToString(),
                    // todo: játékok hozzáadása
                    Games = rentService.GetGamesForRent(r)      //nem async :(
                };
            }).ToList();
            
            return Ok(rents);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RentDetailsDto>> GetRentDetails(int id)
        {
            var rent = await rentService.GetRentById(id);
            var games = rentService.GetGamesForRent(rent);
            var comments = await rentService.GetCommentsForRent(rent);
            return Ok(new RentDetailsDto
            {
                Id = rent.ID,
                DateFrom = rent.DateFrom,
                DateTo = rent.DateTo,
                Status = rent.Status.ToString(),
                Renter = rent.Renter,
                Games = games,
                Comments = comments,
            });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("comment/{rentId}")]
        public async Task<IActionResult> PostComment([FromBody] NewCommentDto message, int rentId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var newComment = await rentService.AddCommentToRent(userId, message.Message, await rentService.GetRentById(rentId));
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPost("status/{rentId}")]
        public async Task<IActionResult> ChangeStatus([FromBody] NewStatusDto status, int rentId)
        {
            rentService.ChangeStatusOfRent(status.Status, await rentService.GetRentById(rentId));
            return Ok();
        }
    }

    public class NewCommentDto
    {
        public string Message { get; set; } = "";
    }

    public class NewStatusDto
    {
        public string Status { get; set; }
    }
}
