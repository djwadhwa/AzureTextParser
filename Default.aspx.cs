using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Program4
{
    public partial class _Default : Page
    {
        static string storageAccountName = ""; // enter storage account name
        static string storageAccountKey = ""; //enter storage account access key
        static string  blobcontainername = ""; //enter blob container name
        CloudStorageAccount acc = new CloudStorageAccount(new StorageCredentials(storageAccountName, storageAccountKey), true);

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void LoadData_Click(object sender, EventArgs e)
        {
            try
            {

                String FileURL = ObjectURL.Text;
                CloudBlobContainer cont = acc.CreateCloudBlobClient().GetContainerReference(blobcontainername); 
                var data = acc.CreateCloudTableClient().GetTableReference("data");
                cont.CreateIfNotExists();
                BlobContainerPermissions permission = new BlobContainerPermissions();
                permission.PublicAccess = BlobContainerPublicAccessType.Container;
                cont.SetPermissions(permission);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FileURL);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string filename = System.IO.Path.GetFileName(new Uri(FileURL).LocalPath);
                CloudBlockBlob cblob = cont.GetBlockBlobReference(filename);
                if (cblob.Exists())
                {
                    int counter = (int)(ViewState[cblob.Name] ?? 0);
                    cblob = cont.GetBlockBlobReference(System.IO.Path.GetFileName(filename.Insert(filename.IndexOf("."), (++counter).ToString())));
                    ViewState[cblob.Name] = counter;
                }
                cblob.UploadFromStream(response.GetResponseStream());
                data.CreateIfNotExists();

                foreach (string line in Regex.Split(cblob.DownloadTextAsync().Result, "\\n"))
                {
                    if (!String.IsNullOrEmpty(line))
                    {
                        string[] name_and_att = Regex.Split(line.Trim(), "\\s+");
                        IDictionary<string, EntityProperty> properties = new Dictionary<string, EntityProperty>();
                        for (int i = 2; i < name_and_att.Length; i++)
                        {
                            System.Diagnostics.Debug.WriteLine(name_and_att[i]);
                            int equalIndex = name_and_att[i].IndexOf('=');
                            properties.Add(name_and_att[i].Substring(0, equalIndex), new EntityProperty(name_and_att[i].Substring(equalIndex + 1)));
                        }
                        DynamicTableEntity entry = new DynamicTableEntity(name_and_att[0], name_and_att[1], "*", properties);
                        data.Execute(TableOperation.InsertOrReplace(entry));
                    }
                }
                ViewState["dataLoaded"] = true;
                Message.Text = "Data has been loaded.";
            }
            catch (Exception excep)
            {
                System.Diagnostics.Debug.WriteLine(excep.Message);
                ViewState["dataLoaded"] = false;
                Message.Text = "Failed to load data. Please try again in a few minutes or provide a new URL.";
            }
        }

        protected void ClearData_Click(object sender, EventArgs e)
        {
            try
            {
                CloudStorageAccount acc = new CloudStorageAccount(new StorageCredentials(storageAccountName, storageAccountKey), true);
                acc.CreateCloudBlobClient().GetContainerReference(blobcontainername).Delete();
                acc.CreateCloudTableClient().GetTableReference("data").DeleteIfExists();

                ViewState["dataLoaded"] = false;
                Message.Text = "Data has been cleared.";
            }
            catch (Exception excep)
            {
                System.Diagnostics.Debug.WriteLine(excep.Message);
                Message.Text = "Data was never loaded or has already been cleared. Try again in a few minutes.";
            }
        }

        protected void Query_Click(object sender, EventArgs e)
        {
            try
            {
                if ((Boolean)ViewState["dataLoaded"])
                {
                    var data = acc.CreateCloudTableClient().GetTableReference("data");
                    String FirstName = TextBox1.Text;
                    String LastName = TextBox2.Text;
                     if (!String.IsNullOrEmpty(LastName)) //if last name is given
                    {
                        if (!String.IsNullOrEmpty(FirstName)) //if first name is also given
                        {
                            DynamicTableEntity entry = (DynamicTableEntity)data.Execute(TableOperation.Retrieve(LastName, FirstName)).Result;
                            Dictionary<String, EntityProperty> val = (Dictionary<string, EntityProperty>)entry.Properties;
                            Output.Text = entry.PartitionKey + " " + entry.RowKey + " ";
                            foreach (KeyValuePair<string, EntityProperty> pair in val)
                            {
                                Output.Text += pair.Key + "=" + ((EntityProperty)pair.Value).StringValue + " ";
                            }
                        }
                        else //only have lastname
                        {
                            TableQuery partitionKeyQuery = new TableQuery().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, LastName));
                            Output.Text = "";
                            IEnumerable<DynamicTableEntity> partitionKeyFilter = data.ExecuteQuery(partitionKeyQuery);
                            foreach (DynamicTableEntity entry in partitionKeyFilter)
                            {
                                Dictionary<String, EntityProperty> val = (Dictionary<string, EntityProperty>)entry.Properties;
                                Output.Text += entry.PartitionKey + " " + entry.RowKey + " ";
                                foreach (KeyValuePair<string, EntityProperty> pair in val)
                                {
                                    Output.Text += pair.Key + "=" + ((EntityProperty)pair.Value).StringValue + " ";
                                }
                                Output.Text += "<br />";
                            }
                            if (Output.Text == "")
                                throw new NullReferenceException();
                        }
                    }
                    else
                    {
                        Message.Text = "";
                        Output.Text = "Please enter first and last name or only last name.";
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (NullReferenceException excep)
            {
                Message.Text = "";
                Output.Text = "Person not found.";
            }
            catch (Exception excep)
            {
                System.Diagnostics.Debug.WriteLine(excep.Message);
                Message.Text = "Please load data.";
            }
        }
    }
}