using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TranslaterServiceBL.ModelsBL.TranslationBL;
using TranslaterServiceBL.ModelsBL.TranslationBL.Dto;
using TranslaterWebApi.Filters;
using TranslaterWebApi.ModelsUI.TranslationUI.Dto;

namespace TranslaterWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ValidateModelFilter]
    public class TranslationController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITranslationCrud crud;

        public TranslationController(
            IMapper mapper,
            ITranslationCrud crud)
        {
            this.mapper = mapper;
            this.crud = crud;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<string>> UpdateTranslationVlaueById(
            [FromBody] InputEditTranslationByIdDtoUI input,
            CancellationToken token = default)
        {
            await crud.EditTranslationValueByIdAsync(mapper.Map<InputEditTranslationByIdDtoBL>(input), token);

            return Ok("Success");
        }
    }
}
