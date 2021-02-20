using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dotYT3.WebFramework.Models;
using MediaToolkit;
using MediaToolkit.Model;
using VideoLibrary;

namespace dotYT3.WebFramework.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HomeIndexViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var destinationFolder = viewModel.DestinationFolder;

                var result = destinationFolder.Substring(destinationFolder.Length - 1);
                if (result != "/")
                {
                    destinationFolder = destinationFolder + @"\";
                }

                var youtube = YouTube.Default;

                var youtubeSource = viewModel.YoutubeUrl;

                var recoveryChoice = viewModel.RecoverySelection;

                if (recoveryChoice.ToLower() == "audio")
                {
                    var vid = youtube.GetVideo(youtubeSource);
                    System.IO.File.WriteAllBytes(destinationFolder + vid.FullName, vid.GetBytes());

                    var inputFile = new MediaFile { Filename = destinationFolder + vid.FullName };
                    var outputFile = new MediaFile
                    { Filename = $"{destinationFolder + vid.FullName.Replace(".mp4", "")}.mp3" };

                    if (System.IO.File.Exists(outputFile.Filename))
                    {
                        System.IO.File.Delete(destinationFolder + vid.FullName);

                    }
                    else
                    {
                        using (var engine = new Engine())
                        {
                            engine.GetMetadata(inputFile);
                            engine.Convert(inputFile, outputFile);

                        }

                        System.IO.File.Delete(destinationFolder + vid.FullName);

                        if (System.IO.File.Exists(outputFile.Filename))
                        {
                            Process.Start(destinationFolder);
                        }
                        else
                        {
                            //Error downloading
                        }
                    }
                }
                else if (recoveryChoice.ToLower() == "video")
                {
                    var vid = youtube.GetVideo(youtubeSource);
                    System.IO.File.WriteAllBytes(destinationFolder + vid.FullName, vid.GetBytes());

                    var file = new MediaFile { Filename = destinationFolder + vid.FullName };
                    if (System.IO.File.Exists(file.Filename))
                    {
                        Process.Start(destinationFolder);
                    }
                    else
                    {
                        //Error downloading
                    }
                }
            }

            return View(viewModel);
        }
    }
}