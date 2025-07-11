using TranslaterServiceDL.Initializer;

namespace TranslaterWebApi
{
    public static class WebExstensions
    {
        public static async Task Initilization(this WebApplication app, string connectiongString, string pathToDataSeed)
        {
            await FirstLunchApplication.InitilizationDatabase(app.Services, connectiongString, pathToDataSeed);
        }
    }
}
