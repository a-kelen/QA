using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.UserCQ.Commands;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Application.UserCQ.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QA.Controllers
{
    [Route("api/[controller]")]
    public class User : BaseController
    {
        public User(UserManager<Domain.Entities.User> userManager)
        {
            this.userManager = userManager;
        }
        UserManager<Domain.Entities.User> userManager;
        // GET: api/<controller>
        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [Authorize]
        [HttpGet("current")]
        public async Task<Application.UserCQ.Data.UserDTO> GetCurrentUser()
        {
            return await Mediator.Send(new Application.UserQA.Queries.Get.Query());
        }
        // POST api/<controller>
        [HttpPost]
        public async Task<Application.UserCQ.Data.UserDTO> Post(Register.Command request)
        {
           return await Mediator.Send(request);   
        }
        [HttpPost("login")]
        public async Task<Application.UserCQ.Data.UserDTO> Login(Login.Command request)
        {
            return await Mediator.Send(request);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        [Authorize]
        [HttpPut]
        public async Task<UserDTO> Put(ChangeProfile.Command request)
        {
            return await Mediator.Send(request);
        }
        [HttpPost("password")]
        public async Task<bool> ChangePass(ChangePass.Command request)
        {
            return await Mediator.Send(request);
        }
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
