using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.RoomsCQ.Commands;
using Application.RoomsCQ.Data;
using Application.RoomsCQ.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QA.Controllers
{
    [Route("api/[controller]")]
    public class Room : BaseController
    {
        // GET: api/<controller>
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<RoomDTO>> GetOwnList()
        {
            return await Mediator.Send(new OwnList.Query());
        }
        [Authorize]
        [HttpGet("subscribes")]
        public async Task<IEnumerable<RoomDTO>> GetSubscribesList()
        {
            return await Mediator.Send(new SubscribesList.Query());
        }

        [Authorize]
        [HttpGet("search/{name}")]
        public async Task<IEnumerable<RoomDTO>> Search(string name)
        {
            return await Mediator.Send(new Search.Query { Name = name });
        }

        // GET api/<controller>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<LargeRoomDTO> Get(Guid id)
        {
            return await Mediator.Send(new GetById.Query { RoomId = id });
        }

        // POST api/<controller>
        [Authorize]
        [HttpPost]
        public async Task<Application.RoomsCQ.Data.RoomDTO> Post(Application.RoomsCQ.Commands.Create.Command request)
        {
            return await Mediator.Send(request);
        }
        [Authorize]
        [HttpPost("switch")]
        public async Task<bool> Post(Switch.Command request)
        {
            return await Mediator.Send(request);
        }
        // PUT api/<controller>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<RoomDTO> Put(Guid id, Edit.Command request)
        {
            request.RoomId = id;
            return await Mediator.Send(request);
        }

        // DELETE api/<controller>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<bool> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Command { RoomId = id });
        }
    }
}
