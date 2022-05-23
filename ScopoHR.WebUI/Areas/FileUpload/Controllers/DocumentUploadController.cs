using NtitasCommon.Core.Common;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using ScopoHR.WebUI.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ScopoHR.WebUI.Areas.FileUpload.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DocumentUploadController : ApiController
    {
        private EmployeeService employeeService;
        private DocumentService documentService;
        private Config config;
        private FlowJS flowJS;

        private string documentRoot;      

        public DocumentUploadController()
        {
            this.config = new Config();
            documentRoot = config.DocumentRoot;
            flowJS = new FlowJS(documentRoot);
        }

        public DocumentUploadController(
            EmployeeService empService,
            DocumentService docService           
            )
        {
            employeeService = empService;
            documentService = docService;           
        }


        #region FlowJS


        [HttpGet]
        [AllowAnonymous]
        [Route("api/fileupload/documentsupload/upload")]
        public object Upload(int flowChunkNumber, string flowIdentifier)
        {
            if (flowJS.ChunkIsHere(flowChunkNumber, flowIdentifier))
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/fileupload/documentsupload/upload")]
        public async Task<IHttpActionResult> Upload()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }           

            try
            {
                documentRoot = HostingEnvironment.MapPath(documentRoot);
                if (!Directory.Exists(documentRoot))
                    Directory.CreateDirectory(documentRoot);
                var provider = new MultipartFormDataStreamProvider(documentRoot);                
                await Request.Content.ReadAsMultipartAsync(provider);
                int chunkNumber = Convert.ToInt32(provider.FormData["flowChunkNumber"]);
                int totalChunks = Convert.ToInt32(provider.FormData["flowTotalChunks"]);
                string identifier = provider.FormData["flowIdentifier"];

                Guid uniqueIdentifier = Guid.NewGuid();
                string extension = Path.GetExtension(provider.FormData["flowFilename"]);                
                string newFilename = uniqueIdentifier.ToString("N") + extension;
                DocumentCategory category =  (DocumentCategory)int.Parse(provider.FormData["category"]);
                // Rename generated file
                MultipartFileData chunk = provider.FileData[0]; // Only one file in multipart message
                flowJS.RenameChunk(chunk, chunkNumber, identifier);
                // Assemble chunks into single file if they're all here
                
                bool allAssembled = flowJS.TryAssembleFile(identifier, totalChunks, newFilename);

                if (allAssembled)
                {
                    DocumentViewModel document = new DocumentViewModel
                    {
                        EmployeeId = int.Parse(provider.FormData["employeeId"]),
                        Category = category,
                        Title = provider.FormData["flowFilename"],
                        UniqueIdentifier = newFilename
                    };

                    // Get document processor
                    DocumentProcessor processor = DocumentProcessorFactory.GetProcessor(category, documentService);
                    // Move to appropriate location    
                    if (!Directory.Exists(processor.Location))
                        Directory.CreateDirectory(processor.Location);
                    File.Move(Path.Combine(documentRoot + newFilename), Path.Combine(processor.Location + newFilename));
                    // Process the document
                    processor.Process(document, User.Identity.Name);
                }

                return Ok(new { FileName = provider.FormData["flowFilename"], Guid = newFilename });                
            }
            catch (Exception e)
            {   
                throw new HttpResponseException(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.ExpectationFailed,
                    ReasonPhrase = e.Message
                });
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }



        #endregion


        [HttpGet]
        [AllowAnonymous]
        [Route("api/fileupload/documentsupload/getdocumentlocations")]
        public Dictionary<DocumentCategory, string> GetDocumentLocations()
        {
            return DocumentHelper.GetLocations();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/fileupload/documentsupload/getdocumentcategories")]
        public Dictionary<DocumentCategory, object> GetDocumentCategories()
        {
            Dictionary<DocumentCategory, object> res = new Dictionary<DocumentCategory, object>();

            foreach (var c in Enum.GetValues(typeof(DocumentCategory)))
            {
                res.Add((DocumentCategory)c, c);
            }
            return res;
        }
    }
}
