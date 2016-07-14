using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarDataAccessLayer.AzureASModel
{
    public class AsignarBlobModel
    {
        private CloudStorageAccount _storageAccount;

        private CloudBlobClient _blobClient;

        public AsignarBlobModel()
        {
            string appSettings = CloudConfigurationManager.GetSetting("AsignarAzureStorage");
            _storageAccount = CloudStorageAccount.Parse(appSettings);
            _blobClient = _storageAccount.CreateCloudBlobClient();
        }

        public string GetOrCreateBlobBugContainer(string projectPrefix)
        {
            string containerNamePattern = "{0}-container";

            CloudBlobContainer container = _blobClient.GetContainerReference(string.Format(containerNamePattern, projectPrefix));

            container.CreateIfNotExists();

            return string.Format(containerNamePattern, projectPrefix);
        }

        public string GetUserPhotosContainerName()
        {
            return "asignar-user-photos";
        }
                
        public string GetContainerSasUri(string containerName)
        {
            var sasConstraints = new SharedAccessBlobPolicy();

            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);

            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddDays(1);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Delete | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List;

            string sasContainerToken = container.GetSharedAccessSignature(sasConstraints);

            return container.Uri + sasContainerToken;
        }

        public string GetBlobSasUri(string containerName, string fileName)
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
            CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

            var sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddYears(1);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Delete | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List;

            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            return blob.Uri + sasBlobToken;
        }

        public string GetDefaultAvatarSasUri()
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(GetUserPhotosContainerName());
            CloudBlockBlob blob = container.GetBlockBlobReference("avatar-default.jpg");

            var sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddYears(100);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Delete | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List;

            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            return blob.Uri + sasBlobToken;
        }

        public bool UploadBlob(string containerName, string fileName, Stream fileStream)
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            blockBlob.UploadFromStream(fileStream);

            return true;
        }

        public Stream DownloadBlob(string containerName, string fileName)
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            using (var fileStream = File.OpenWrite(fileName))
            {
                blockBlob.DownloadToStream(fileStream);
                return fileStream;
            }
        }

        public ICollection<IListBlobItem> GetListBlobsWithSas(string sasUrl)
        {
            var cloudBlobContainer = new CloudBlobContainer(new Uri(sasUrl));

            ICollection<IListBlobItem> list = cloudBlobContainer.ListBlobs(null, true).ToList();

            return list;
        }
    }
}

/*
            public CloudBlobContainer GetOrCreateBlobProjectContainer(string projectPrefix)
        {
            string containerNamePattern = "{0}-container";

            CloudBlobContainer container = _blobClient.GetContainerReference(string.Format(containerNamePattern, projectPrefix));

            container.CreateIfNotExists();

            return container;
        }

        public CloudBlobContainer GetUserPhotosContainer()
        {
            CloudBlobContainer container = _blobClient.GetContainerReference("asignar-user-photos");

            return container;
        }
                
        public string GetContainerSasUri(CloudBlobContainer container)
        {
            var sasConstraints = new SharedAccessBlobPolicy();

            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddDays(1);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Delete | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List;

            string sasContainerToken = container.GetSharedAccessSignature(sasConstraints);

            return container.Uri + sasContainerToken;
        }

        public string GetBlobSasUri(CloudBlobContainer container, string blobName)
        {
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            var sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddYears(1);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Delete | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List;

            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            return blob.Uri + sasBlobToken;
        }

        public bool UploadBlob(CloudBlobContainer container, string blobName, Stream fileStream)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            blockBlob.UploadFromStream(fileStream);

            return true;
        }

        public Stream DownloadBlob(CloudBlobContainer container, string blobName)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            using (var fileStream = File.OpenWrite(blobName))
            {
                blockBlob.DownloadToStream(fileStream);
                return fileStream;
            }
        }
     
 */
