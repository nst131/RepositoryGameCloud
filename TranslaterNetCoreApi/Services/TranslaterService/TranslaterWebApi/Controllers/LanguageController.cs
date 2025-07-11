using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TranslaterServiceBL.ModelsBL.LanguageBL;
using TranslaterServiceBL.ModelsBL.LanguageBL.Dto;
using TranslaterWebApi.Filters;
using TranslaterWebApi.ModelsUI.LanguageUI.Dto;

namespace TranslaterWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ValidateModelFilter]
    public class LanguageController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ILanguageCrudBL crud;

        public LanguageController(
            IMapper mapper,
            ILanguageCrudBL crud)
        {
            this.mapper = mapper;
            this.crud = crud;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ICollection<OutLanguageDtoUI>>> GetAll(
            CancellationToken token = default)
        {
            var responseFromBL = await this.crud.GetAllAsync(token);

            if (responseFromBL is null)
                throw new NullReferenceException($"{nameof(responseFromBL)} is null");

            return Ok(responseFromBL.Select(x => this.mapper.Map<OutLanguageDtoUI>(x)).ToList());
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<string>> InsertNewLanguage(
            [FromBody] InputInsertNewLanguageDtoUI input,
            CancellationToken token)
        {
            if (input is null)
                throw new ArgumentNullException("Input Data from UI is null");

            await this.crud.InsertNewLanguageAsync(mapper.Map<InputInsertNewLanguageDtoBL>(input), token);
            return Ok("Success");
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete]
        [Route("[action]/{id:Guid}")]
        public async Task<ActionResult<string>> Delete(
            [FromRoute] Guid id,
            CancellationToken token)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException($"{id} of language is empty");

            await this.crud.DeleteLanguageAsync(id, token);
            return Ok("Success");
        }
    }
}
