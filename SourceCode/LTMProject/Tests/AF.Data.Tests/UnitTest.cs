using System;
using System.Linq;
using AF.Domain.Domain.Knowledge;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AF.Data.Tests
{
    /// <summary>
    /// Summary description for UnitTest
    /// </summary>
    [TestClass]
    public class UnitTest : PersistenceTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var id = 3;

            //var uploadquestion = context.Set<UploadQuestionRecord>().FirstOrDefault(d => d.Id == 339);


            //var query =
            //    from timu in context.Set<TiMu>()
            //    from tmk in context.Set<TiMuKnowledge>()
            //    from tmknow in context.Set<KnowledgePiont>()
            //    where tmknow.Id == uploadquestion.KnowledgeId &&
            //          tmk.KPID == tmknow.KPID &&
            //          tmk.Is_MainKP &&
            //          context.Set<UserAnswer>().Any(d => d.Tmid != tmk.TMID) &&
            //          timu.TMID == tmk.TMID && timu.SubjectID==2
            //    select timu;

            ////var count = query.Count();

            //var timulist = query.Count();

          // var list= context.Set<TiMu>().Where(d => timulist.Contains(d.TMID)).ToList();











        }

        public class MyClass
        {
            public string TMID { get; set; }
        }

    }
}
