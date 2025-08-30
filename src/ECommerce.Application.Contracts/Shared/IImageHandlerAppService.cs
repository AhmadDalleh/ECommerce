using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared
{
    public interface IImageHandlerAppService
    {
        Task<List<string>> UploadAsync(IFormFileCollection files, string src);

        void DeleteImageAsync(string src);
    }
}
