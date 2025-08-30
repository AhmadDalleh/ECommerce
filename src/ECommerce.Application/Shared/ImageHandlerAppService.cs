using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;


namespace ECommerce.Shared
{
    public class ImageHandlerAppService : IImageHandlerAppService , ITransientDependency
    {
        private readonly IFileProvider _fileProvider;
        public ImageHandlerAppService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public async Task<List<string>> UploadAsync(IFormFileCollection files, string src)
        {
            var saveImageSrc = new List<string>(); //the list of image the returns to controller
            var ImageDirectory = Path.Combine("wwwroot", "Images", src.Trim());//where the image directory will be store in the wwwroot directory
            if (Directory.Exists(ImageDirectory) is not true)//just check if the directory exist and created if it not by name of the product
            {
                Directory.CreateDirectory(ImageDirectory);
            }
            foreach (var item in files)//this for the all images that set to product
            {
                if (item.Length > 0)//if there is image in the item
                {
                    //get image name
                    var ImageName = item.FileName;//set image name to file name 

                    var ImageSrc = $"/Images/{src}/{ImageName}";//where image will be store
                    var root = Path.Combine(ImageDirectory, ImageName);//insert the image name to the Image directory
                    using (FileStream stream = new FileStream(root, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }//this copy the image from root in the pc and apply it in image src 
                    saveImageSrc.Add(ImageSrc);
                }
            }
            return saveImageSrc;
        }
        
        public void DeleteImageAsync(string src)
        {
            var info = _fileProvider.GetFileInfo(src);
            var root = info.PhysicalPath;
            File.Delete(root);

        }
    }
}
