using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Graph = Microsoft.Office.Interop.Graph;
using System.Runtime.InteropServices;
namespace AF.Services.FileIExport
{
    /// <summary>
    /// 文件操作Service
    /// </summary>
    public class ExportFileService : IExportFileService
    {
        #region 对象注入
        
        #endregion


        /// <summary>
        /// Creates the PPT.
        /// </summary>
        /// <param name="taskName">Name of the task.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void CreatePpt(string taskName)
        {

            //Application pptApplication = new Application();
            // Create the Presentation File
            //Presentation pptPresentation = pptApplication.Presentations.Add();

            //Microsoft.Office.Interop.PowerPoint.CustomLayout customLayout = pptPresentation.SlideMaster.CustomLayouts[Microsoft.Office.Interop.PowerPoint.PpSlideLayout.ppLayoutText];

            //// Create new Slide
            //var slides = pptPresentation.Slides;
            //_Slide slide = slides.AddSlide(1, customLayout);

            //// Add title
            //var objText = slide.Shapes[1].TextFrame.TextRange;
            //objText.Text = "FPPT.com";
            //objText.Font.Name = "Arial";
            //objText.Font.Size = 32;

            //objText = slide.Shapes[2].TextFrame.TextRange;
            //objText.Text = "Content goes here\nYou can add text\nItem 3";

            //slide.NotesPage.Shapes[2].TextFrame.TextRange.Text = "This demo is created by FPPT using C# - Download free templates from http://FPPT.com";

            //pptPresentation.SaveAs(@"c:\temp\fppt.pptx", Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsDefault, MsoTriState.msoTrue);
            //pptPresentation.Close();
            //pptApplication.Quit();

            throw new NotImplementedException();
        }
    }
}
