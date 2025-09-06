using Microsoft.AspNetCore.Mvc;
using TrackSense.API.AlertService.Services.CRUD;
using AutoMapper;
using TrackSense.API.AlertService.Models.Interfaces;

namespace TrackSense.API.AlertService.Controllers.CRUD;

//T1 is entity
//T2 is Dto
public abstract class BaseCrudController<T1, T2> : ControllerBase 
    where T1: Identifiable 
    where T2: Identifiable
{
    protected readonly ILogger _logger;
    protected readonly ICrudService<T1> _crudService;
    protected readonly IMapper _mapper;
 
    protected BaseCrudController(ILogger logger, ICrudService<T1> crudService, IMapper mapper)
    {
        _logger = logger;
        _crudService = crudService;
        _mapper = mapper;
    }

    protected async Task<ActionResult<List<T1>>> GetEntitiesAsync()
    {
        var results = await _crudService.GetAllAsync();

        if(results == null)
        {
            return NotFound();
        }

        var resultsDto = _mapper.Map<List<T2>>(results);

        return Ok(resultsDto);
    }

    protected async Task<ActionResult<T1>> GetEntityAsync(int id)
    {
        var result = await _crudService.GetByIdOrDefaultAsync(id);

        if(result == null)
        {
            return NotFound();
        }

        var resultDto = _mapper.Map<T2>(result);

        return Ok(resultDto);
    }

    protected async Task<ActionResult<T1>> PostEntityAsync([FromBody] T2 dto)
    {
        if(dto == null || !ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = await _crudService.GetByIdOrDefaultAsync(dto.ID);
        if(entity != null)
        {
            return StatusCode(422);
        }

        var map = _mapper.Map<T1>(dto);
        T1 result = await _crudService.AddAsync(map);

        return Ok(result);
    }

    protected async Task<ActionResult<T1>> UpdateEntityAsync([FromBody] T2 dto)
    {
        if(dto == null || !ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = _mapper.Map<T1>(dto);
        T1 result = await _crudService.UpdateAsync(entity);

        if(result == null)
        {
            return BadRequest("Entity does not exist");
        }

        return Ok(result);
    }

    protected async Task<ActionResult> DeleteEntityAsync(int id)
    {
        var group = await _crudService.GetByIdOrDefaultAsync(id);
        
        if(group == null)
            return NotFound();

        var saved = await _crudService.DeleteAsync(id);
        
        return saved > 0 ? Ok() : Accepted("Could not save changes");
    }
}