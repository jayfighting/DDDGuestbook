﻿using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers
{
    [Route("api/[controller]")]
    [VerifyGuestbookExists]
    [ApiController]
    public class GuestbookController : ControllerBase
    {
        private readonly IRepository<Guestbook> _guestbookRepository;

        public GuestbookController(IRepository<Guestbook> guestbookRepository)
        {
            _guestbookRepository = guestbookRepository;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var guestbook = _guestbookRepository.GetById(id);
            return Ok(guestbook);
        }

        [HttpPost("{id:int}/NewEntry")]
        public IActionResult NewEntry(int id, [FromBody] GuestbookEntry entry)
        {
            var guestbook = _guestbookRepository.GetById(id);
            guestbook.AddEntry(entry);
            _guestbookRepository.Update(guestbook);

            return Ok(guestbook);
        }
    }
}
