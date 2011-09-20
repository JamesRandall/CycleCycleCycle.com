using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using CycleCycleCycle.Models;
using CycleCycleCycle.Utilities;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace CycleCycleCycle.Cacheing
{
    public class HeightMapImageCache : IHeightMapImageCache
    {
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _blobContainer;

        public HeightMapImageCache()
        {
            CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("imageCache");
            _blobClient = account.CreateCloudBlobClient();

            // Get and create the container
            _blobContainer = _blobClient.GetContainerReference("timebasedheightmapimages");
            _blobContainer.CreateIfNotExist();

            // Setup the permissions on the container to be public
            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            _blobContainer.SetPermissions(permissions);

        }

        private string GetFilename(Route route)
        {
            return string.Format("hm{0}.png", route.RouteID);
        }

        public Bitmap HeightMapImage(Route route, int width, int height)
        {
            Bitmap bitmap = null;
            CloudBlob blob = _blobContainer.GetBlobReference(GetFilename(route));
            if (blob.Exists())
            {
                MemoryStream memoryStream = new MemoryStream();
                blob.DownloadToStream(memoryStream);
                bitmap = new Bitmap(memoryStream);
            }
            return bitmap;
        }

        public void CacheHeightMapImage(Bitmap bitmap, Route route, int width, int height)
        {
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            memoryStream.Flush();
            memoryStream.Position = 0;

            CloudBlob blob = _blobContainer.GetBlobReference(GetFilename(route));
            blob.UploadFromStream(memoryStream);
            blob.Metadata["RouteID"] = route.RouteID.ToString();
            blob.SetMetadata();

            blob.Properties.ContentType = "image/png";
            blob.SetProperties();
        }
    }
}