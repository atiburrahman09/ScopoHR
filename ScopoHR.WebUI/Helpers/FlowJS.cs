using NtitasCommon.Core.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;

namespace ScopoHR.WebUI.Helpers
{
    public class FlowJS
    {
        private string documentRoot;
        public FlowJS(string docRoot)
        {
            documentRoot = docRoot;
        }

        public string GetChunkFileName(int chunkNumber, string identifier)
        {
            return HostingEnvironment.MapPath(Path.Combine(documentRoot, string.Format("{0}_{1}", identifier, chunkNumber.ToString())));
        }

        public void RenameChunk(MultipartFileData chunk, int chunkNumber, string identifier)
        {
            string generatedFileName = chunk.LocalFileName;
            string chunkFileName = GetChunkFileName(chunkNumber, identifier);
            if (File.Exists(chunkFileName)) File.Delete(chunkFileName);
            File.Move(generatedFileName, chunkFileName);

        }

        public string GetFileName(string identifier)
        {
            return HostingEnvironment.MapPath(Path.Combine(documentRoot, identifier));
        }

        public bool ChunkIsHere(int chunkNumber, string identifier)
        {
            string fileName = GetChunkFileName(chunkNumber, identifier);
            return File.Exists(fileName);
        }

        public bool AllChunksAreHere(string identifier, int totalChunks)
        {
            for (int chunkNumber = 1; chunkNumber <= totalChunks; chunkNumber++)
                if (!ChunkIsHere(chunkNumber, identifier)) return false;
            return true;
        }

        public bool TryAssembleFile(string identifier, int totalChunks, string filename)
        {

            if (AllChunksAreHere(identifier, totalChunks))
            {
                // Create a single file
                var consolidatedFileName = GetFileName(identifier);
                using (var destStream = File.Create(consolidatedFileName, 15000))
                {
                    for (int chunkNumber = 1; chunkNumber <= totalChunks; chunkNumber++)
                    {
                        var chunkFileName = GetChunkFileName(chunkNumber, identifier);
                        using (var sourceStream = File.OpenRead(chunkFileName))
                        {
                            sourceStream.CopyTo(destStream);
                        }
                    }
                    destStream.Close();
                }
                // Rename consolidated with original name of upload
                filename = Path.GetFileName(filename); // Strip to filename if directory is specified (avoid cross-directory attack)

                //Add Condition For Pdf Here
                //Declare Field for Pdf containing Pdf file Location
                //then set root to pdf location
               
                string realFileName = HostingEnvironment.MapPath(Path.Combine(documentRoot, filename));
                if (File.Exists(filename)) File.Delete(realFileName);
                File.Move(consolidatedFileName, realFileName);
                // Delete chunk files
                for (int chunkNumber = 1; chunkNumber <= totalChunks; chunkNumber++)
                {
                    //var chunkFileName = GetChunkFileName(chunkNumber, identifier);
                    //File.Delete(chunkFileName);

                    try
                    {
                        var chunkFileName = GetChunkFileName(chunkNumber, identifier);
                        if (File.Exists(chunkFileName))
                        {
                            File.Delete(chunkFileName);
                        }
                        else
                        {
                            Debug.WriteLine("File does not exist.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                return true;
            }
            return false;
        }
    }
}