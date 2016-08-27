using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AF.Core;
using AF.Core.Extensions;
using AF.Core.Infrastructure;
using AF.Services.Message;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace AF.Services.Tests
{
    [TestClass]
    public class UnitTest1 : TestBase
    {

        [TestMethod]
        public void TiMuBuilder()
        {
            //[{"InputCode":"LY_201_UAA_1000_a","InputAnswer":"{"answer":["0|-1","1|-1","2|-1"]}","IsRight":0}]

           /* var _errorSetService = EngineContext.Current.Resolve<IErrorSetService>();
            var tiMuServices = EngineContext.Current.Resolve<ITiMuServices>();
            var timuInfos = _errorSetService.GetTiMuInfoByDate(3, new DateTime(2016, 2, 2));

            tiMuServices.GetTiMuByTmids(timuInfos.Select(d => d.TMID).ToArray());
           
            foreach (var item in timuInfos)
            {
                var tbbr = new TiMuBuiltRequest(item.TiMu);
                var tmbuder = new TiMuBuild(tbbr);
                tmbuder.BuildHtml();
            }*/


            /*            var tiMuServices = EngineContext.Current.Resolve<ITiMuServices>();
                        tiMuServices.GetInputsBytmId("ly_000_BJX_4108");

                        var inputs = EngineContext.Current.Resolve<IUploadQuestionRecordService>();
                        var errorSetService = EngineContext.Current.Resolve<IErrorSetService>();
                        var timiservice = EngineContext.Current.Resolve<ITiMuServices>();

                        var timis = timiservice.GetTiMuByTmids(new []{ "ly_000_BJX_4108", "LY_001_UBK_0769", "LY_001_UBK_0769", "LY_001_UBZ_0475", "ly_001_UAS_0589" });
                        foreach (var timi in timis)
                        {
                            var tbbr = new TiMuBuiltRequest(timi);
                            var tmbuder = new TiMuBuild(tbbr);
                            var timubuilder = tmbuder.BuildHtml();
                        }
                       */

            /* var uploadQuestionRecord = uploadQuestionRecordService.GetQuestionRecordById(339);
             var timus = errorSetService.GetMorePractice(uploadQuestionRecord);
             for (var i = 0; i < timus.Count; i++)
             {
                 var tbbr = new TiMuBuiltRequest(timus[i], (i + 1).ToString());
                 var tmbuder = new TiMuBuild(tbbr);
                 var timubuilder = tmbuder.BuildHtml();
             }*/


        }

        [TestMethod]
        public void TestMethod1()
        {
            var customerservice = EngineContext.Current.Resolve<IMessageServices>();
            customerservice.SendMsg("15007155738", "hello");

          var random=  CommonHelper.GenerateRandomDigitCode(6);
        }



    }


}
