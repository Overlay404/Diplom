using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CloudDB.Utils
{
    public partial class FileUtil
    {
        public static async Task SaveToFile(IJSRuntime js, string nameFile, byte[] bytes)
        {
            await js.InvokeVoidAsync("saveAsFile", nameFile, Convert.ToBase64String(bytes));
        }
    }
}

