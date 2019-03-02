using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

public class UploadController : ApiController
{
    public async Task<HttpResponseMessage> PostFormData()
    {
        // Check if the request contains multipart/form-data.
        if (!Request.Content.IsMimeMultipartContent())
        {
            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        }

        string root = HttpContext.Current.Server.MapPath("~/App_Data");
        var provider = new MultipartFormDataStreamProvider(root);

        try
        {
            //var streamProvider = new MultipartMemoryStreamProvider();
            //await Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(streamProvider)
            //   .ContinueWith((tsk) =>
            // {

            //     foreach (HttpContent ctnt in streamProvider.Contents)
            //     {
            //          // You would get hold of the inner memory stream here
            //          Stream stream1 = ctnt.ReadAsStreamAsync().Result;

            //          // do something witht his stream now
            //      }
            // });


            MultipartMemoryStreamProvider stream = await this.Request.Content.ReadAsMultipartAsync();
            foreach (var st in stream.Contents)
            {
                var fileBytes = await st.ReadAsByteArrayAsync();
                string base64 = Convert.ToBase64String(fileBytes);
                var contentHeader = st.Headers;
                if(string.IsNullOrEmpty(contentHeader.ContentDisposition.FileName))
                {
                    string filename = contentHeader.ContentDisposition.FileName.Replace("\"", "");
                    string filetype = contentHeader.ContentType.MediaType;
                }
            }




            //// Read the form data.
            //await Request.Content.ReadAsMultipartAsync(provider);

            //// This illustrates how to get the file names.
            //foreach (MultipartFileData file in provider.FileData)
            //{
            //    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
            //    Trace.WriteLine("Server file path: " + file.LocalFileName);
            //}
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        catch (System.Exception e)
        {
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
        }
    }

    //public async Task<HttpResponseMessage> PostFile()
    //{
    //    // Check if the request contains multipart/form-data.
    //    if (!Request.Content.IsMimeMultipartContent())
    //    {
    //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
    //    }

    //    string root = HttpContext.Current.Server.MapPath("~/App_Data");
    //    var provider = new MultipartFormDataStreamProvider(root);

    //    try
    //    {
    //        StringBuilder sb = new StringBuilder(); // Holds the response body

    //        // Read the form data and return an async task.
    //        await Request.Content.ReadAsMultipartAsync(provider);

    //        // This illustrates how to get the form data.
    //        foreach (var key in provider.FormData.AllKeys)
    //        {
    //            foreach (var val in provider.FormData.GetValues(key))
    //            {
    //                sb.Append(string.Format("{0}: {1}\n", key, val));
    //            }
    //        }

    //        // This illustrates how to get the file names for uploaded files.
    //        foreach (var file in provider.FileData)
    //        {
    //            FileInfo fileInfo = new FileInfo(file.LocalFileName);
    //            sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));
    //        }
    //        return new HttpResponseMessage()
    //        {
    //            Content = new StringContent(sb.ToString())
    //        };
    //    }
    //    catch (System.Exception e)
    //    {
    //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
    //    }
    //}

}