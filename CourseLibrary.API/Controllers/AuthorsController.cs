using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository courseLibraryRepository;
        private readonly IMapper mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            this.courseLibraryRepository = courseLibraryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery] AuthorsResourceParameters resourceParameters)
        {
            var authors = courseLibraryRepository.GetAuthors(resourceParameters);
            return Ok(mapper.Map<IEnumerable<AuthorDto>>(authors));
        }

        [HttpGet("{authorId}", Name="GetAuthor")]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        {
            var author = courseLibraryRepository.GetAuthor(authorId);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<AuthorDto>(author));
        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto author)
        {
            if (author == null)
            {
                return BadRequest();
            }

            var newAuthor = mapper.Map<Author>(author);
            courseLibraryRepository.AddAuthor(newAuthor);
            courseLibraryRepository.Save();
            var authorToReturn = mapper.Map<AuthorDto>(newAuthor);

            return CreatedAtRoute("GetAuthor", new { authorId = authorToReturn.Id }, authorToReturn);
        }
    }
}