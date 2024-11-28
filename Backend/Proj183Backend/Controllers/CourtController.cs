using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Proj183Backend.Controllers.Models;
using Proj183Backend.Data;


namespace Proj183Backend.Controllers;

[Route("courts/")]
[ApiController]
public class CourtController : ControllerBase
{
    private readonly DataContext _dataContext;

    public CourtController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllAsync()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var courts = await _dataContext.Court.AsNoTracking()
                .Include(c => c.Address)
                .Include(c => c.User)
                .OrderByDescending(c => c.CourtId)
                .ToListAsync().ConfigureAwait(false);

            var courtViewModels = new List<CourtViewModel>();

            foreach (var court in courts)
            {
                courtViewModels.Add(new CourtViewModel() 
                { 
                    CourtId = court.CourtId, 
                    AddressId = court.AddressId, 
                    UserId = court.UserId, 
                    UserEmail = court.User!.Email!, 
                    Street = court.Address!.Street, 
                    City = court.Address.City, 
                    Canton = court.Address.Canton, 
                    Description = court.Description, 
                    Title = court.Title 
                });
            }

            return Ok(courtViewModels);
        }
        catch (Exception exc)
        {

            return BadRequest(exc.Message);
        }
    }

    [HttpGet("{courtId:int}"), Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync([FromRoute, BindRequired] int courtId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var court = await _dataContext.Court.AsNoTracking()
                .Include(c => c.Address)
                .Include(c => c.User)
                .Where(c => c.CourtId == courtId)
                .FirstOrDefaultAsync().ConfigureAwait(false);

            if (court == null) return NotFound("The desired court was not found.");

            var courtViewModels = new CourtViewModel()
            {
                CourtId = court.CourtId,
                AddressId = court.AddressId,
                UserId = court.UserId,
                UserEmail = court.User!.Email!,
                Street = court.Address!.Street,
                City = court.Address.City,
                Canton = court.Address.Canton,
                Description = court.Description,
                Title = court.Title
            };

            return Ok(courtViewModels);
        }
        catch (Exception exc)
        {

            return BadRequest(exc.Message);
        }
    }

    [HttpPost, Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)] 
    public async Task<IActionResult> PostAsync([FromBody] CourtModel courtRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        courtRequest.UserId = await _dataContext.Users.AsNoTracking()
            .Where(u => u.Email == courtRequest.UserEmail)
            .Select(foundUser => foundUser.Id).FirstOrDefaultAsync().ConfigureAwait(false);

        if (courtRequest.UserId == 0) return NotFound("The logged in user was not found. Try loging out and in again.");

        var reuseableAddressId = await _dataContext.Address.AsNoTracking()
            .Where(a => 
                a.Street == courtRequest.Street 
                && a.Number == courtRequest.Number
                && a.City == courtRequest.City 
                && a.Canton == courtRequest.Canton
                && a.PLZ == courtRequest.PLZ)
            .Select(a => a.AddressId)
            .FirstOrDefaultAsync().ConfigureAwait(false);

        try
        {

            var newCourt = reuseableAddressId > 0 
                ? new Court()
                    {
                        UserId = courtRequest.UserId,
                        AddressId = reuseableAddressId,
                        Description = courtRequest.Description,
                        Title = courtRequest.Title,
                    }
                : new Court()
                    {
                        UserId = courtRequest.UserId,
                        Address = new Address()
                        {
                            Street = courtRequest.Street.Trim(),
                            City = courtRequest.City.Trim(),
                            Canton = courtRequest.Canton.Trim()
                        },
                        Description = courtRequest.Description,
                        Title = courtRequest.Title,
                    };

            await _dataContext.Court.AddAsync(newCourt).ConfigureAwait(false);
            await _dataContext.SaveChangesAsync().ConfigureAwait(false);

            return Created(new Uri($"{Request.Path}/{newCourt.CourtId}", UriKind.Relative), newCourt.CourtId);
        }
        catch (Exception exc)
        {
            return BadRequest(exc.Message);
        }
    }

    [HttpPut("{courtId:int}"), Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostAsync([FromBody] CourtModel courtRequest, [FromRoute, BindRequired] int courtId)
    {
        if (courtId != courtRequest.CourtId)
            return BadRequest("Id from route is not matching with the id from body.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        courtRequest.UserId = await _dataContext.Users.AsNoTracking()
            .Where(u => u.Email == courtRequest.UserEmail)
            .Select(foundUser => foundUser.Id).FirstOrDefaultAsync().ConfigureAwait(false);

        if (courtRequest.UserId == 0) return NotFound("The logged in user was not found. Try loging out and in again.");

        var reuseableAddressId = await _dataContext.Address.AsNoTracking()
            .Where(a =>
                a.Street == courtRequest.Street
                && a.Number == courtRequest.Number
                && a.City == courtRequest.City
                && a.Canton == courtRequest.Canton
                && a.PLZ == courtRequest.PLZ)
            .Select(a => a.AddressId)
            .FirstOrDefaultAsync().ConfigureAwait(false);

        try
        {
            // read court with address 
            var originalCourt = await _dataContext.Court.AsNoTracking()
                .Include(oc => oc.Address)
                .Where(c => c.CourtId == courtId)
                .FirstOrDefaultAsync().ConfigureAwait(false);

            if (originalCourt == null) return NotFound("The desired court was not found.");

            // overwrite original court
            originalCourt = reuseableAddressId > 0
                ? new Court()
                {
                    CourtId = courtId,
                    UserId = courtRequest.UserId,
                    AddressId = reuseableAddressId,
                    Description = courtRequest.Description,
                    Title = courtRequest.Title,
                }
                : new Court()
                {
                    CourtId = courtId,
                    UserId = courtRequest.UserId,
                    Address = new Address()
                    {
                        Street = courtRequest.Street.Trim(),
                        City = courtRequest.City.Trim(),
                        Canton = courtRequest.Canton.Trim()
                    },
                    Description = courtRequest.Description,
                    Title = courtRequest.Title,
                };

            // update the court and save the changes
            _dataContext.Court.Update(originalCourt);
            await _dataContext.SaveChangesAsync().ConfigureAwait(false);

            return NoContent();
        }
        catch (Exception exc)
        {
            return BadRequest(exc.Message);
        }
    }

    [HttpDelete("{courtId:int}"), Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute, BindRequired] int courtId, [FromQuery, BindRequired] string userEmail)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var originalUserId = await _dataContext.Users.AsNoTracking()
            .Where(u => u.Email == userEmail).Select(foundUser => foundUser.Id).FirstOrDefaultAsync().ConfigureAwait(false);

        if (originalUserId == 0) return NotFound("The logged in user was not found. Try loging out and in again.");

        try
        {
            var originalCourt = await _dataContext.Court.AsNoTracking()
                .Where(c => c.CourtId == courtId)
                .FirstOrDefaultAsync().ConfigureAwait(false);

            if (originalCourt == null) return NotFound("The desired court was not found.");

            _dataContext.Court.Remove(originalCourt);
            await _dataContext.SaveChangesAsync().ConfigureAwait(false);

            return NoContent();
        }
        catch (Exception exc)
        {
            return BadRequest(exc.Message);
        }
    }
}
