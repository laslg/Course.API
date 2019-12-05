using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authorcollections")]
    public class AuthorCollectionsController : Controller
    {
        private readonly ICourseLibraryRepository courseLibraryRepository;
        private readonly IMapper mapper;

        public AuthorCollectionsController(ICourseLibraryRepository repository, IMapper mapper)
        {
            courseLibraryRepository = repository;
            this.mapper = mapper;
        }

        [HttpGet("({ids})", Name="GetAuthorCollection")]
        public IActionResult GetAuthorCollection([FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var authorsEntities = courseLibraryRepository.GetAuthors(ids);

            if (ids.Count() != authorsEntities.Count())
            {
                return NotFound();
            }

            var authorDtos = mapper.Map<IEnumerable<AuthorDto>>(authorsEntities);
            return Ok(authorDtos);
        }

        [HttpPost]
        public ActionResult<IEnumerable<AuthorDto>> CreateAuthorCollection(IEnumerable<AuthorForCreationDto> authors)
        {
            var newAuthors = mapper.Map<IEnumerable<Entities.Author>>(authors);
            foreach (var a in newAuthors)
            {
                courseLibraryRepository.AddAuthor(a);
            }
            courseLibraryRepository.Save();

            var authorCollectionToReturn = mapper.Map<IEnumerable<AuthorDto>>(newAuthors);
            var idsAsString = string.Join(",", authorCollectionToReturn.Select(a => a.Id));
            
            return CreatedAtRoute("GetAuthorCollection", new { ids = idsAsString }, authorCollectionToReturn);
        }
    }
}