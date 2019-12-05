using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository repository;
        private readonly IMapper mapper;

        public CoursesController(ICourseLibraryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            if (!repository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var coursesForAuthor = repository.GetCourses(authorId);
            return Ok(mapper.Map<IEnumerable<CourseDto>>(coursesForAuthor));
        }
        
        [HttpGet("{courseId}", Name="GetCourseForAuthor")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!repository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var course = repository.GetCourse(authorId, courseId);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CourseDto>(course));
        }

        [HttpPost]
        public ActionResult<CourseDto> CreateCourseForAuthor(Guid authorId, CourseForCreationDto course)
        {
            if (!repository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var newCourse = mapper.Map<Course>(course);
            repository.AddCourse(authorId, newCourse);
            repository.Save();

            var courseToReturn = mapper.Map<CourseDto>(newCourse);

            return CreatedAtRoute("GetCourseForAuthor", new { authorId = authorId, courseId = courseToReturn.Id }, courseToReturn);
        }

    }
}
