using ApplicationCore.Services;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestServices _ITestServices;
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;


        public TestController(ITestServices testServices, IUnitOfWork uof, IMapper mapper)
        {
            _ITestServices = testServices;
            _uof = uof ?? throw new ArgumentNullException(nameof(uof));
            _mapper = mapper;
        }
        [HttpGet("GetAllTest")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IReadOnlyList<Test> test = await _ITestServices.GetAllAsync();
                return Ok(new ApiResponse<IReadOnlyList<Test>>("GetAll Successfully", test));
            }
            catch (Exception ex)
            {
                _uof.RollbackTransaction();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var test = await _ITestServices.GetByIdAsync(id);
            if (test == null)
                return NotFound($"Aucune test trouvée avec l'identifiant {id}.");
                var testtdo = _mapper.Map<TestDTO>(test);
                return Ok(new ApiResponse<TestDTO>("GetAll Successfully", testtdo));
            }
            catch (Exception ex)
            {
                _uof.RollbackTransaction();
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] TestDTO entity)
        {

            try
            {

                _uof.BeginTransaction();
                var testfromfront = _mapper.Map<Test>(entity);

                await _ITestServices.AddAsync(testfromfront);
                _uof.CommitTransaction();

                var testdto = _mapper.Map<TestDTO>(testfromfront);
                return Ok(new ApiResponse<TestDTO>("test created Successfully", testdto));
            }

            catch (Exception ex)
            {
                _uof.RollbackTransaction();
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TestDTO testfromfront)
        {
          
            // Validate that only allowed fields are provided
            var allowedFields = new List<string> { "designation" };



            try
            {
                _uof.BeginTransaction();
                var entity = _mapper.Map<Test>(testfromfront);

                var updatedAdresse = await _ITestServices.UpdateAsync(id, entity, allowedFields);
                if (updatedAdresse == null)
                {
                    _uof.RollbackTransaction();
                    return NotFound($"Aucune adresse trouvée avec l'identifiant {id}.");
                }
                _uof.CommitTransaction();
                var testdto= _mapper.Map<TestDTO>(testfromfront);
                return Ok(new ApiResponse<TestDTO>("test updated Successfully", testdto));
            }
           
            catch (Exception ex)
            {
                _uof.RollbackTransaction();
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                _uof.BeginTransaction();

                var result = await _ITestServices.Delete(id);
                _uof.CommitTransaction();
                return Ok(new ApiResponse<bool>("test deleted Successfully", true));


            }
            catch (Exception ex)
            {
                _uof.RollbackTransaction();
                return BadRequest(ex.Message);
            }
        }
    }
}
