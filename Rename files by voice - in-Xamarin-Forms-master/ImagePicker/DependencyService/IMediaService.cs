using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImagePicker.Model;
using Xamarin.Forms;

namespace ImagePicker
{
    public interface IMediaService
    {
        event EventHandler<MediaEventArgs> OnMediaAssetLoaded;
        bool IsLoading { get; }
        Task<IList<MediaAssest>> RetrieveMediaAssetsAsync(CancellationToken? token = null);
        Task<string> StoreProfileImage(string path);
        Task<string> GetImageWithCamera();
        ImageSource GenerateThumbnailImageSource(string url, long usecond);

        byte[] ResizeImage(string imagePath, float width, float height);

        string UpdateGallery(string filePath);

    }
}
