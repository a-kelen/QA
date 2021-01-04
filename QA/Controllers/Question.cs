using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.QuestionsCQ.Commands;
using Application.QuestionsCQ.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QA.Controllers
{
    [Route("api/[controller]")]
    public class Question : BaseController
    {
        // GET: api/<controller>
        [Authorize]
        [HttpGet("own")]
        public async Task<IEnumerable<QuestionDTO>> GetOwn()
        {
            return await Mediator.Send(new Application.QuestionsCQ.Queries.OwnList.Query());
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [Authorize]
        [HttpPost]
        public async Task<QuestionDTO> Post(Create.Command request)
        {
            return await Mediator.Send(request);
        }
        [Authorize]
        [HttpPost("support")]
        public async Task<bool> Support(Application.LikesCQ.Commands.Switch.Command request)
        {
            return await Mediator.Send(request);
        }
        [Authorize]
        [HttpPost("ignore")]
        public async Task<bool> Ignore(Ignore.Command request)
        {
            return await Mediator.Send(request);
        }
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
