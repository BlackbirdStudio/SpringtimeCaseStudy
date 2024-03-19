using AutoMapper;
using CaseStudy.EFCore;
using CaseStudy.EFCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace CaseStudy.Controllers
{
    [ApiController, SpringtimeRoute, AuthorizeRoleManageData]
    public class CrudController<TEFEntity, TDtoEntity>(Repository repository, IMapper mapper) : Controller where TEFEntity : BaseEntity
    {
        private readonly Repository _repository = repository;
        private readonly IMapper _mapper = mapper;

        [HttpPost]
        public async Task Create(TDtoEntity entity)
        {
            var efEntity = _mapper.Map<TEFEntity>(entity);
            await _repository.Create(efEntity);
        }

        [HttpGet(ControllerPath.All)]
        public async Task<IEnumerable<TEFEntity>> GetAll()
        {
            return await _repository.GetAllNoTracking<TEFEntity>();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TEFEntity>> Get(int id)
        {
            return await _repository.Get<TEFEntity>(id);
        }

        [HttpPatch("{id}")]
        public async Task Update(int id, TDtoEntity entity)
        {
            await _repository.Update<TEFEntity, TDtoEntity>(id, entity);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _repository.Delete<TEFEntity>(id);
        }
    }
}
