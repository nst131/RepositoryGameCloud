using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text.Json;
using TranslaterServiceDL.Context;
using TranslaterServiceDL.Exceptions;
using TranslaterServiceDL.Models;

namespace TranslaterServiceDL.Initializer
{
    public static class InitialInitialization
    {
        public static async Task InitilizeDataFromJsonAsync(ITranslaterContext context, string pathToDataSeed)
        {
            if (context is null)
                throw new ArgumentNullException("Context is null");

            if (!await context.Database.CanConnectAsync())
                throw new DatabaseException("Cannot connect to datatabse");

            var isEmpty = !await context.Set<Language>().AnyAsync() && !await context.Set<Keyword>().AnyAsync();

            if (isEmpty)
            {
                //string pathToJson = Path.GetFullPath(pathToDataSeed);

                if (string.IsNullOrEmpty(pathToDataSeed))
                    throw new NullReferenceException("Path to DataSeed.json is null or empty");

                if (!File.Exists(pathToDataSeed))
                    throw new FileNotFoundException($"File is not found by {pathToDataSeed}");

                string json = string.Empty;

                try
                {
                    json = await File.ReadAllTextAsync(pathToDataSeed);
                }
                catch
                {
                    throw new FileLoadException("Cannot read all text from DataSeed.json by specified path");
                }

                var seed = TryDeserializeFromJson<SeedDataDto>(json);

                if (seed is null)
                    throw new NullReferenceException("Seed is null during the initialization");

                if (!seed.Languages.Any())
                    throw new NullReferenceException($"Initial data {seed.Languages} from json is null");

                var languages = seed.Languages.Select(name => new Language { Name = name }).ToList();
                var languageDict = languages.ToDictionary(x => x.Name, x => x);

                var keywords = new List<Keyword>();
                var translations = new List<Translation>();

                foreach (var keywordDto in seed.Keywords)
                {
                    var keyword = new Keyword { Value = keywordDto.Value };
                    keywords.Add(keyword);

                    foreach (var langPair in keywordDto.Translations)
                    {
                        if (!languageDict.TryGetValue(langPair.Key, out var language)) continue;

                        translations.Add(new Translation
                        {
                            Keyword = keyword,
                            Language = language,
                            Value = langPair.Value
                        });
                    }
                }

                await context.Set<Language>().AddRangeAsync(languages);
                await context.Set<Keyword>().AddRangeAsync(keywords);
                await context.Set<Translation>().AddRangeAsync(translations);
                await context.SaveChangesAsync();
            }

        }

        public static T? TryDeserializeFromJson<T>(string json) where T : class
        {
            T? seed;
            try
            {
                seed = JsonSerializer.Deserialize<T>(json);
                return seed;
            }
            catch
            {
                throw new JsonException("Cannot deserelize dataseed from json");
            }
        }
    }

    public class KeywordDto
    {
        public string Value { get; set; } = string.Empty;
        public Dictionary<string, string> Translations { get; set; } = new();
    }

    public class SeedDataDto
    {
        public List<string> Languages { get; set; } = new();
        public List<KeywordDto> Keywords { get; set; } = new();
    }
}
