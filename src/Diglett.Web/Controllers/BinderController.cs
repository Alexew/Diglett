using Diglett.Core;
using Diglett.Core.Catalog.Collection;
using Diglett.Core.Data;
using Diglett.Web.Models.Collection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diglett.Web.Controllers
{
    [Authorize]
    public class BinderController : Controller
    {
        private readonly DiglettDbContext _db;
        private readonly IWorkContext _workContext;

        public BinderController(DiglettDbContext db, IWorkContext workContext)
        {
            _db = db;
            _workContext = workContext;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> List()
        {
            var user = _workContext.CurrentUser!;
            var binders = await _db.Binders
                .AsNoTracking()
                .Where(x => x.UserId == user.Id)
                .Select(x => PrepareBinderModel(x))
                .ToListAsync();

            var model = new BinderListModel
            {
                Binders = binders
            };

            return View(model);
        }

        public IActionResult CreateBinderModal()
        {
            return PartialView(PrepareBinderModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(BinderModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(List));

            var user = _workContext.CurrentUser!;
            var binder = new Binder
            {
                Name = model.Name,
                UserId = user.Id,
                PageCount = model.PageCount
            };

            _db.Binders.Add(binder);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> EditBinderModal(int id)
        {
            var binder = await _db.Binders
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id && x.UserId == _workContext.CurrentUser!.Id);

            if (binder == null)
                return NotFound();

            var model = PrepareBinderModel(binder);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BinderModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(List));

            var binder = await _db.Binders
                .SingleOrDefaultAsync(x => x.Id == model.Id && x.UserId == _workContext.CurrentUser!.Id);

            if (binder == null)
                return NotFound();

            binder.Name = model.Name;
            binder.PageCount = model.PageCount;

            _db.Binders.Update(binder);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> DeleteBinderModal(int id)
        {
            var binder = await _db.Binders
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id && x.UserId == _workContext.CurrentUser!.Id);

            if (binder == null)
                return NotFound();

            var model = new BinderModel
            {
                Id = binder.Id,
                Name = binder.Name
            };

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var binder = await _db.Binders
                .Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.Id == id && x.UserId == _workContext.CurrentUser!.Id);

            if (binder == null)
                return NotFound();

            _db.Binders.Remove(binder);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

        private static BinderModel PrepareBinderModel(Binder? binder = null)
        {
            var model = new BinderModel();

            if (binder != null)
            {
                model.Id = binder.Id;
                model.Name = binder.Name;
                model.PageCount = binder.PageCount;
            }

            return model;
        }
    }
}
