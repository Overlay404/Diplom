using CloudDB.Http;
using CloudDB.Model;
using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;
using System.Net;
using System.Xml.Linq;

namespace CloudDB.Utils
{
    public class ServerRequest
    {
        /// <summary>
        /// Переменная хранящая значение HttpClient
        /// </summary>
        private static readonly HttpClient client = HttpClientExtension.GetClient();

        /// <summary>
        /// Получение баз данных
        /// </summary>
        public static async Task<ObservableCollection<DatabaseStructure>?> GetDatabases()
        {
            string json = await HttpClientExtension.GetRequestAsync(client, BuildUrl.GetUrl("api/get_databases"), new System.Net.Cookie("jwt_token", JwtToken.SecureToken));
            return HttpClientExtension.FromJson<ObservableCollection<DatabaseStructure>>(json);
        }

        /// <summary>
        /// Переименование базы данных
        /// </summary>
        public static async Task<string> RenameDatabase(DatabaseStructure database, string name) => await HttpClientExtension.GetRequestAsync(client, BuildUrl.GetUrl($"api/rename_database/?token={database.token}&name={name}"), new System.Net.Cookie("jwt_token", JwtToken.SecureToken));

        /// <summary>
        /// Удаление базы данных
        /// </summary>
        public static async Task<string> DeleteDatabase(DatabaseStructure database) => 
            await HttpClientExtension.GetRequestAsync(client, BuildUrl.GetUrl($"api/delete_database/?token={database.token}"), new System.Net.Cookie("jwt_token", JwtToken.SecureToken));

        /// <summary>
        /// Создание базы данных
        /// </summary>
        public static async Task<DatabaseStructure> CreateDatabase(string name)
        {
            string token = await HttpClientExtension.GetRequestAsync(client, BuildUrl.GetUrl($"api/create_database/?name_db={name}"), new System.Net.Cookie("jwt_token", JwtToken.SecureToken));
            return new DatabaseStructure { name = name, token = string.Concat(token.AsEnumerable().Where(ch => !ch.Equals('\"'))) };
        }

        /// <summary>
        /// Метод для получения списка имён таблиц
        /// </summary>
        public async static Task<List<NameTable>> GetTables(string tokenDatabase)
        {
            string json = await HttpClientExtension.GetRequestAsync(client, BuildUrl.GetUrl($"api/get_tables?token={tokenDatabase}"));
            return HttpClientExtension.FromJson<List<NameTable>>(json) ?? [];
        }

        /// <summary>
        /// Получение данных таблицы
        /// </summary>
        /// <param name="name">В качестве параметра передаётся имя таблицы</param>
        /// <returns>В качестве ответа возвращается список из словарей, со значения таблицы</returns>
        public async static Task<List<Dictionary<string, object>>?> GetTable(string name, string tokenDatabase)
        {
            string json = await HttpClientExtension.GetRequestAsync(client, BuildUrl.GetUrl($"api/{name}/get?token={tokenDatabase}")).ConfigureAwait(false);
            var data = HttpClientExtension.FromJson<List<Dictionary<string, object>>>(json);
            return data;
        }

        /// <summary>
        /// Метод для обнавления строки в таблице
        /// </summary>
        /// <param name="nameTable">Имя таблицы</param>
        /// <param name="newValue">Новые значения типа словарь</param>
        /// <param name="oldValue">Старые значения типа словарь</param>
        public async static Task UpdateTable(string nameTable, Dictionary<string, object> newValue, Dictionary<string, object> oldValue, string tokenDatabase) => 
            await HttpClientExtension.PostRequestAsync<UpdateRequestSerializeble>(client, BuildUrl.GetUrl($"api/{nameTable}/update?token={tokenDatabase}"), new UpdateRequestSerializeble { new_value = newValue, old_value = oldValue });

        /// <summary>
        /// Получение информации о таблице
        /// </summary>
        /// <param name="nameTable">Имя таблицы</param>
        /// <returns>Возврат списка с информацией о таблице</returns>
        public async static Task<List<InfoTable>> GetInfoTable(string nameTable, string tokenDatabase) => 
            await HttpClientExtension.GetRequestAsyncWithParameterToConvert<List<InfoTable>>(client, BuildUrl.GetUrl($"api/{nameTable}/get_table_info?token={tokenDatabase}"));

        /// <summary>
        /// Запрос к сереверу для создания новой строки в таблице
        /// </summary>
        /// <param name="name">Имя таблицы</param>
        /// <param name="addingRows">Данные для сохранения</param>
        public async static Task AddRowInTable(string name, Dictionary<string, object> addingRows, string tokenDatabase) => 
            await HttpClientExtension.PostRequestAsync<Dictionary<string, object>>(client, BuildUrl.GetUrl($"api/{name}/add?token={tokenDatabase}"), addingRows);

        /// <summary>
        /// Удаление строки по id
        /// </summary>
        /// <param name="nameTable"></param>
        /// <param name="id"></param>
        public async static Task DeleteRow(string nameTable, int id, string tokenDatabase) => 
            await HttpClientExtension.GetRequestAsync(client, BuildUrl.GetUrl($"api/{nameTable}/delete?id={id.ToString()}&token={tokenDatabase}"));

        /// <summary>
        /// Запрос на создание таблицы
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async static Task CreateTable(string name, List<TableStructure> tables, string tokenDatabase)
        {
            foreach (var item in tables)
            {
                if (item.type_field is null)
                {
                    item.type_field = TableStructure.Type.INTEGER.ToString();
                }
            }
            await HttpClientExtension.PostRequestAsync<List<TableStructure>>(client, BuildUrl.GetUrl($"api/create_table?token={tokenDatabase}&table_name={name}"), tables);
        }

        /// <summary>
        /// Удаление таблицы
        /// </summary>
        /// <param name="name">Имя таблицы</param>
        /// <param name="tokenDatabase">Токен базы данных</param>
        /// <returns></returns>
        public static async Task DeleteTable(string name, string tokenDatabase)
        {
            await HttpClientExtension.GetRequestAsync(client, BuildUrl.GetUrl($"api/delete_table?token={tokenDatabase}&table_name={name}")).ConfigureAwait(false);
        }

        /// <summary>
        /// Отправка на сервер sql запрос
        /// </summary>
        /// <param name="tokenDatabase">токен базы данных</param>
        /// <param name="query">запрос пользователя</param>
        /// <returns>Ответ от сервера</returns>
        public static async Task<string> ExecuteSqlQuery(string tokenDatabase, string query) => 
            await HttpClientExtension.GetRequestAsync(client, BuildUrl.GetUrl($"api/sql_query?token={tokenDatabase}&query={query}"), new System.Net.Cookie("jwt_token", JwtToken.SecureToken));

        /// <summary>
        /// Получение имени пользователя
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetUsernameUser() =>
             await HttpClientExtension.GetRequestAsync(client, BuildUrl.GetUrl($"api/get_user"), new System.Net.Cookie("jwt_token", JwtToken.SecureToken));

        public static async Task<string> ChangeUsernameUser(string username) =>
             await HttpClientExtension.GetRequestAsync(client, BuildUrl.GetUrl($"api/change_username?username={username}"), new System.Net.Cookie("jwt_token", JwtToken.SecureToken));
    }
}
