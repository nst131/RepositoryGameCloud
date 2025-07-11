using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TranslaterServiceBL.ModelsBL.KeywordBL;
using TranslaterServiceBL.ModelsBL.KeywordBL.Dto;
using TranslaterWebApi.Filters;
using TranslaterWebApi.ModelsUI.KeywordUI.Dto;

namespace TranslaterWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ValidateModelFilter]
    public class KeywordController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IKeywordCrudBL crud;

        public KeywordController(
            IMapper mapper,
            IKeywordCrudBL crud)
        {
            this.mapper = mapper;
            this.crud = crud;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ICollection<OutKeywordByLanguagesDtoUI>>> GetKeywordByLanguages(
            [FromBody] InputKeywordByLanguagesDtoUI input,
            CancellationToken token = default)
        {
            if(input is null) 
                throw new ArgumentNullException("Input Data from UI is null");

            var responseFromBL = await this.crud.GetKeywordByValueAsync(mapper.Map<InputKeywordByValueDtoBL>(input), token);

            if (responseFromBL is null)
                throw new NullReferenceException($"{nameof(responseFromBL)} is null");

            return Ok(responseFromBL.Select(x => this.mapper.Map<OutKeywordByLanguagesDtoUI>(x)).ToList());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<OutNewKeywordDtoUI>> InsertKeyword(
            [FromBody] InputInsertNewKeywordDtoUI input,
            CancellationToken token = default)
        {
            if (input is null)
                throw new ArgumentNullException("Input Data from UI is null");

            var responseFromBL = await this.crud.InsertNewKeywordAsync(input.Name, token);

            if (responseFromBL is null)
                throw new NullReferenceException($"{nameof(responseFromBL)} is null");

            return Ok(mapper.Map<OutNewKeywordDtoUI>(responseFromBL));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<string>> DeleteKeywordByValue(
            [FromBody] InputDeleteKeywordByValueDtoUI input,
            CancellationToken token = default)
        {
            if (input is null)
                throw new ArgumentNullException("Input Data from UI is null");

            await this.crud.DeleteKeywordByValueAsync(input.Name, token);

            return Ok("Success");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ICollection<OutKeywordByLanguagesDtoUI>>> GetKeywordByValue(
           [FromBody] InputKeywordByValueDtoUI input,
           CancellationToken token = default)
        {
            if (input is null)
                throw new ArgumentNullException("Input Data from UI is null");

            var responseFromBL = await this.crud.GetKeywordByValueAsync(mapper.Map<InputKeywordByValueDtoBL>(input), token);

            if (responseFromBL is null)
                throw new NullReferenceException($"{nameof(responseFromBL)} is null");

            return Ok(responseFromBL.Select(x => this.mapper.Map<OutKeywordByLanguagesDtoUI>(x)).ToList());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ICollection<OutKeywordByLanguagesDtoUI>>> GetFilteredKeywordByValue(
            [FromBody] InputFilteredKeywordByValueDtoUI input,
            CancellationToken token = default)
        {
            if (input is null)
                throw new ArgumentNullException("Input Data from UI is null");

            var responseFromBL = await this.crud.GetKeywordByValueAsync(mapper.Map<InputKeywordByValueDtoBL>(input), token);

            if (responseFromBL is null)
                throw new NullReferenceException($"{nameof(responseFromBL)} is null");

            var result = responseFromBL.Select(x => this.mapper.Map<OutKeywordByLanguagesDtoUI>(x)).ToList();

            var filteredResult = result
                .OrderByDescending(x => x.Value.ToLower().StartsWith(input.FilterValue.ToLower()))
                .ThenBy(x => x.Value.ToLower());

            return Ok(filteredResult);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<int>> GetCountPage(
            CancellationToken token)
        {
            var responseFromBL = await this.crud.GetCountPageAsync(token);

            return Ok(responseFromBL);
        }
    }
}
