using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Translation.Models.DataModels;
using Translation.Repositories.Interfaces;

namespace Translation.Repositories.Implementions
{
    public class TranslationRepository : ITranslationRepository
    {
        string connectionString;

        public TranslationRepository(IOptions<PostgreDatabaseConfiguration> options)
        {
            PostgreDatabaseConfiguration postgreDatabaseConfiguration = options.Value;
            connectionString = postgreDatabaseConfiguration.ToString();
        }

        public async Task Test()
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    var sql = "insert into languages(languagename) values ('VietNamese')";
                    var result = await conn.ExecuteAsync(sql);
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<int> InsertWord(WordData pWord)
        {
            string sql = "insert into Words (languageid, wordtext) values (@pLanguageId, @pWordText)";
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    int result = await conn.ExecuteAsync(sql, new
                    {
                        pLanguageId = pWord.LanguageId,
                        pWordText = pWord.WordText,
                    });
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<LanguageData> GetLanguageByName(string pName)
        {
            string sql = "select LanguageId, LanguageName from languages where lower(languagename) = lower(@pLanguageName)";
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    var result = await conn.QueryAsync<LanguageData>(sql, new
                    {
                        pLanguageName = pName,
                    });
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TranslationData> GetTranslationByPairWordId(int pWordId, int pTranslatedWordId)
        {
            string sql = "select * from Translations where WordId = @pWordId and TranslatedWordId = @pTranslatedWordId";
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    var result = await conn.QueryAsync<TranslationData>(sql, new
                    {
                        pWordId,
                        pTranslatedWordId,
                    });
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> InsertTranslation(KeyValuePair<WordData, WordData> pKeyValue)
        {
            string sql = @"
                insert into Words (languageid, wordtext) values (@pLanguageId, @pWordText);
                insert into Words (languageid, wordtext) values (@pTranslatedLanguageId, @pTranslatedWordText);

                insert into Translations (wordid, translatedwordid)
                values (
                    (SELECT WordId FROM Words WHERE WordText = @pWordText LIMIT 1),
                    (SELECT WordId FROM Words WHERE WordText = @pTranslatedWordText LIMIT 1)
                ) 
            ";
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    var result = await conn.ExecuteAsync(sql, new
                    {
                        pLanguageId = pKeyValue.Key.LanguageId,
                        pWordText = pKeyValue.Key.WordText,
                        
                        pTranslatedLanguageId = pKeyValue.Value.LanguageId,
                        pTranslatedWordText = pKeyValue.Value.WordText,
                    });
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<WordData> GetWordById(int pWordId)
        {
            string sql = "select * from Words where WordId = @pWordId";
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    WordData result = (await conn.QueryAsync<WordData>(sql, new
                    {
                        @pWordId = pWordId
                    })).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TranslationData> GetRandomTranslation()
        {
            string sql = @"
                SELECT TranslationId, WordId, TranslatedWordId, SelectionCount
                FROM translations
                ORDER BY (-log(random()) * (1.0 / CASE WHEN SelectionCount = 0 THEN 1 ELSE SelectionCount END)) DESC
                LIMIT 1;
            ";

            string sqlUpdateSelectionCount = @"
                UPDATE translations
                SET SelectionCount = SelectionCount + 1
                WHERE TranslationId = @pTranslationId;
            ";
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    TranslationData result = (await conn.QueryAsync<TranslationData>(sql)).FirstOrDefault();
                    result.Word = await GetWordById(result.WordId);
                    result.TranslatedWord = await GetWordById(result.TranslatedWordId);

                    await conn.ExecuteAsync(sqlUpdateSelectionCount, new
                    {
                        pTranslationId = result.TranslationId,
                    });

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}