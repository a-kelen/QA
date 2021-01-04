using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.AnswersCQ.Commands;
using Application.AnswersCQ.Data;
using Application.AnswersCQ.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QA.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class Answer : BaseController
    {
        // GET: api/<controller>
        [HttpGet("lastest")]
        public async Task<IEnumerable<AnswerDTO>> Get()
        {
            return await Mediator.Send(new LastestAnswerList.Query());
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
        public async Task<AnswerDTO> Post(Create.Command request) 
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
