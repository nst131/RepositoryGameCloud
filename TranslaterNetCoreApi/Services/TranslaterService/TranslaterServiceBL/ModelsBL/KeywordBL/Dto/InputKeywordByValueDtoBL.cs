namespace TranslaterServiceBL.ModelsBL.KeywordBL.Dto
{
    public class InputKeywordByValueDtoBL
    {
        public int Page { get; set; } = 1;
        public string Value { get; set; } = string.Empty;
        public ICollection<Guid> LanguagesId { get; set; } = new List<Guid>();
    }
}
