using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Domain.Domain.Knowledge;
using AF.Services.Books;
using AF.Services.Knowledge;
using AF.Web.Framework.UI;
using AF.Web.Models.Knowledge;

namespace AF.Web.Controllers
{
    public class KnowledgeController : BasePublicController
    {
        private readonly IBookService _bookService;
        private readonly IKnowledgeService _knowledgeService;

        public KnowledgeController(IBookService bookService, IKnowledgeService knowledgeService)
        {
            _bookService = bookService;
            _knowledgeService = knowledgeService;
        }

        public ActionResult Index(KnowledgeModel model)
        {
            var degreeList = _bookService.GetDegreeList();
            model.DegreeItemList = degreeList.ToSelectItems();
            model.DegreeItemList.Insert(0, new SelectListItem() { Text = "请选择", Value = "0" });
            model.SubjectItemList.Insert(0, new SelectListItem() { Text = "请选择", Value = "0" });
            return View(model);
        }

        public JsonResult GetTreeData(int subjectId)
        {
            if (subjectId <= 0) return null;
            var knowledges = _knowledgeService.GetKnowledge(subjectId);
            var datas = knowledges.Select(t => new
            {
                id = t.Id,
                text = t.Name,
                parent = t.ParentId?.ToString() ?? "#"
            });
            return Json(datas, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddKnowledge(int subjectId, int? parentId, string name)
        {
            if (string.IsNullOrEmpty(name))
                return Json(new { state = false });

            var knowledge = new KnowledgePiont()
            {
                ParentId = parentId,
                Name = name,
                SubjectId = subjectId
            };
            _knowledgeService.InsertKnowledge(knowledge);
            return Json(new { state = true, data = knowledge.Id });
        }

        [HttpPost]
        public JsonResult UpdateKnowledge(int id, string name)
        {
            if (string.IsNullOrEmpty(name))
                return Json(new { state = false });

            var model = _knowledgeService.GetKnowledgeById(id);
            model.Name = name;
            _knowledgeService.UpdateKnowledge(model);
            return Json(new { state = true });
        }

        [HttpPost]
        public JsonResult DeleteKnowledge(int id)
        {
            if (id <= 0) return null;
            _knowledgeService.DeleteKnowledge(id);
            return Json(new { state = true });
        }
    }
}