﻿namespace TranslaterServiceBL.ModelsBL.KeywordBL.Dto
{
    public class OutGetKeywordByLanguagesDtoBL
    {
        public Guid Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public ICollection<OutTranslationWithLanguageDtoBL> OutTranslationWithLanguages { get; set; } = new List<OutTranslationWithLanguageDtoBL>();
    }
}
