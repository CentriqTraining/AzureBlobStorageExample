﻿using Microsoft.WindowsAzure.Storage;
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
            //  Set yours in the app.config first by using
            //  information from azure management portal
            string con = ConfigurationManager
                .ConnectionStrings["AzureConnection"]
                .ConnectionString;


            //  Create the storage client - this class has
            //  Methods related to managing the storage account
            var storageClient = CloudStorageAccount.Parse(con);

            //  Create the blob client - this class has 
            //  Methods related to working with a specific blob
            var blobclient = storageClient.CreateCloudBlobClient();

            //  Get a reference to a specific container
            var container = blobclient.GetContainerReference("20487-private-centriq");
            container.CreateIfNotExists();

            //  List out all of the items in this blob
            var AllItems = container.ListBlobs();

            //  Get a reference to the blockblob called buymoria
            var buymore = container.GetBlockBlobReference("buymoria");
            buymore.Delete();

            //  Upload the file by reading the entire thing into 
            //  an array of bytes (best if only used on small files)
            var bytes = File.ReadAllBytes(@"Content\buymore2.xml");

            //  Upload the file to the blob
            buymore.UploadFromByteArray(bytes, 0, bytes.Length);

            //  Upload the same file by using a filestream
            //  Best if the file is decently large (less memory)
            var strm = new FileStream(@"Content\buymore2.xml", FileMode.Open);
            buymore.UploadFromStream(strm);
            strm.Close();

            //  Now we prepare to download it.
            //  create an array of bytes of the length needed
            //  Use buymore.Property["length"]
            byte[] dlbytes = new byte[buymore.Properties.Length];
            var file = buymore.DownloadToByteArray(dlbytes, 0);
            File.WriteAllBytes(@"Content\DL-" + Guid.NewGuid().ToString() + ".xml", dlbytes);

            FileStream wstrm = new FileStream(@"Content\DL-" + Guid.NewGuid().ToString() + ".xml", FileMode.Create);
            buymore.DownloadToStream(wstrm);
            wstrm.Close();

            //  Look in the Bin\Debug folder for these files.
            //  Both DL-* files should look exactly like the 
            //  Original, also in the same folder
        }
    }
}
