using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBlobStorageSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //  Storage account information
            //  information from azure management portal
            string connectionInfo = AccountManager.GetAzureAccount();

            //  Create the storage client - this class has
            //  Methods related to managing the storage account
            CloudStorageAccount storageClient = CloudStorageAccount.Parse(connectionInfo);

            //  Create the blob client - this class has 
            //  Methods related to working with a specific blob
            CloudBlobClient blobclient = storageClient.CreateCloudBlobClient();

            //  Get a reference to a specific container
            CloudBlobContainer container = blobclient.GetContainerReference("20487-private-centriq");
            container.CreateIfNotExists();

            //  List out all of the items in this blob
            IEnumerable<IListBlobItem> AllItems = container.ListBlobs();

            //  Get a reference to the blockblob called buymoria
            CloudBlockBlob buymore = container.GetBlockBlobReference("buymoria");
            buymore.DeleteIfExists();

            //  Upload the file by reading the entire thing into 
            //  an array of bytes (best if only used on small files)
            byte[] bytes = File.ReadAllBytes(@"Content\buymore2.xml");

            //  Upload the file to the blob
            buymore.UploadFromByteArray(bytes, 0, bytes.Length);

            //  Upload the same file by using a filestream
            //  Best if the file is decently large (less memory)
            var strm = new FileStream(@"Content\buymore2.xml", FileMode.Open);
            buymore.UploadFromStream(strm);
            strm.Close();

            //  Now we prepare to download it.
            //  create an array of bytes of the length needed
            //  Use buymore.Properties.Length
            //  Best if used only on small files.
            byte[] dlbytes = new byte[buymore.Properties.Length];
            int file = buymore.DownloadToByteArray(dlbytes, 0);
            File.WriteAllBytes(@"Content\DL-" + Guid.NewGuid().ToString() + ".xml", dlbytes);

            //  Now do the same, but using a filestream 
            //  best if used on large files (less memory)
            FileStream wstrm = new FileStream(@"Content\DL-" + Guid.NewGuid().ToString() + ".xml", FileMode.Create);
            buymore.DownloadToStream(wstrm);
            wstrm.Close();

            //  Look in the Bin\Debug folder for these files.
            //  Both DL-* files should look exactly like the 
            //  Original, also in the same folder
        }
    }
}
