using GadoCalc.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using GadoCalc.Models;

namespace GadoCalc
{
    public class ParametersDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public ParametersDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Parameters).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Parameters)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

        public Task<Parameters> GetItemAsync(int id)
        {
            return Database.Table<Parameters>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Parameters item)
        {
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> UpdateItemAsync(Parameters item)
        {
            return Database.UpdateAsync(item);
            
        }
        public Task<int> InsertItemAsync(Parameters item)
        {
            return Database.InsertAsync(item);
        }

        public Task<int> DeleteAllAsync()
        {
            return Database.ExecuteAsync("DELETE FROM Parameters; DELETE FROM SQLITE_SEQUENCE WHERE name='Parameters';");
        }
    }
}