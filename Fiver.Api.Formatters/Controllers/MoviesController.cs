using Fiver.Api.Formatters.Models.Movies;
using Fiver.Api.Formatters.OtherLayers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fiver.Api.Formatters.Controllers
{
    [Route("movies")]
    public class MoviesController : Controller
    {
        private readonly IMovieService service;

        public MoviesController(IMovieService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var model = service.GetMovies();

            var outputModel = ToOutputModel(model);
            return Ok(outputModel);
        }

        [HttpGet("{id}", Name = "GetMovie")]
        public IActionResult Get(int id)
        {
            var model = service.GetMovie(id);

            var outputModel = ToOutputModel(model);
            return Ok(outputModel);
        }

        [HttpPost]
        public IActionResult Create([FromBody]MovieInputModel inputModel)
        {
            var model = ToDomainModel(inputModel);
            service.AddMovie(model);

            var outputModel = ToOutputModel(model);
            return CreatedAtRoute("GetMovie", new { id = outputModel.Id }, outputModel);
        }
        
        #region " Mappings "

        private MovieOutputModel ToOutputModel(Movie model)
        {
            if (model == null) return null;
            return new MovieOutputModel
            {
                Id = model.Id,
                Title = model.Title,
                ReleaseYear = model.ReleaseYear,
                Summary = model.Summary,
                LastReadAt = DateTime.Now
            };
        }

        private List<MovieOutputModel> ToOutputModel(List<Movie> model)
        {
            return model.Select(item => ToOutputModel(item))
                        .ToList();
        }

        private Movie ToDomainModel(MovieInputModel inputModel)
        {
            return new Movie
            {
                Id = inputModel.Id,
                Title = inputModel.Title,
                ReleaseYear = inputModel.ReleaseYear,
                Summary = inputModel.Summary
            };
        }
        
        #endregion
    }
}
