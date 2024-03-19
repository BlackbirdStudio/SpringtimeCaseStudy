using AutoMapper;
using CaseStudy.EFCore.Exceptions;
using CaseStudy.EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseStudy.EFCore
{
    public class Repository(CaseStudyContext _caseStudyContext, IMapper _mapper)
    {
        public async Task<TEFEntity> Create<TEFEntity>(TEFEntity entity) where TEFEntity : BaseEntity
        {
            await _caseStudyContext.Set<TEFEntity>().AddAsync(entity);
            await _caseStudyContext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<TEFEntity>> GetAllNoTracking<TEFEntity>() where TEFEntity : BaseEntity =>
            await _caseStudyContext.Set<TEFEntity>().AsNoTracking().ToListAsync();

        public async Task<TEFEntity> GetNoTracking<TEFEntity>(int id) where TEFEntity : BaseEntity =>
            await _caseStudyContext.Set<TEFEntity>().AsNoTracking().FirstAsync(o => o.Id == id);

        public async Task<TEFEntity> Get<TEFEntity>(int id) where TEFEntity : BaseEntity =>
            (await _caseStudyContext.Set<TEFEntity>().FirstOrDefaultAsync(o => o.Id == id))
            ?? throw new EntityNotFoundException(id, typeof(TEFEntity));

        public async Task Update<TEFEnitity, TDtoEntity>(int id, TDtoEntity entity) where TEFEnitity: BaseEntity
        {
            var existingEntry = await Get<TEFEnitity>(id);
            _mapper.Map(entity, existingEntry);
            await _caseStudyContext.SaveChangesAsync();
        }

        public async Task Delete<TEFEntity>(int id) where TEFEntity: BaseEntity
        {
            var existingEntry = await Get<TEFEntity>(id) ?? throw new EntityNotFoundException(id, typeof(TEFEntity));
            _caseStudyContext.Set<TEFEntity>().Remove(existingEntry);
            await _caseStudyContext.SaveChangesAsync();
        }
    }
}
