using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Contexts;
using SolaceTK.Core.Models.WorkItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolaceTK.Core.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WorkController : ControllerBase
    {

        public WorkContext Context { get; set; }

        public WorkController(WorkContext context)
        {
            Context = context;
        }

        #region Work Projects

        [HttpGet("projects")]
        public ActionResult<IEnumerable<WorkProject>> GetProjects([FromQuery] bool includeElements = false)
        {
            if (!includeElements) return Ok(Context.Projects);

            return Ok(Context.Projects.Include(x => x.WorkItems).Include(x => x.Comments).Include(x => x.Payments));
        }

        [HttpGet("projects/{name}")]
        public async Task<ActionResult<WorkProject>> GetProject(string name)
        {
            return Ok(await Context.Projects.FirstOrDefaultAsync(x => x.Name == name));
        }

        [HttpPost("projects")]
        public async Task<ActionResult<WorkProject>> CreateProject([FromBody] WorkProject model)
        {
            if (model == null) return null;

            model.Created = DateTimeOffset.Now;
            model.Updated = DateTimeOffset.Now;

            Context.Projects.Add(model);
            await Context.SaveChangesAsync();

            return Created($"/api/v1/work/projects/{model.Id}", model);
        }

        [HttpPut("projects/{id}")]
        public async Task<ActionResult<WorkProject>> UpdateProject([FromBody] WorkProject model, int id)
        {
            if (model == null) return BadRequest();

            var tempModel = await Context.Projects.Include(x => x.Payments).Include(x => x.WorkItems).FirstOrDefaultAsync(x => x.Id == id);
            if (tempModel == null) return NotFound();

            if (tempModel.Payments != null && tempModel.Payments.Count > 0)
            {
                var totalAmountPaid = 0f;
                foreach (var payment in tempModel.Payments)
                {
                    totalAmountPaid += payment.Amount;
                }
                tempModel.TotalPaid = totalAmountPaid;
                tempModel.PaidHours = totalAmountPaid / 35f;
                tempModel.RemainingPayment = tempModel.TotalActualHours - tempModel.PaidHours;
            }


            if (model.Comments != null && model.Comments.Count > 0)
            {
                foreach (var comment in model.Comments)
                {
                    tempModel.Comments.Add(comment);
                }
            }

            // Apply Model Changes
            tempModel.Id = model.Id;
            tempModel.Name = model.Name;
            tempModel.Tags = model.Tags;
            tempModel.Description = model.Description;

            if (tempModel.WorkItems != null && tempModel.WorkItems.Count > 0)
            {
                var totalHoursEstimate = 0f;
                var totalHoursActual = 0f;

                foreach (var item in tempModel.WorkItems)
                {
                    totalHoursEstimate += item.HoursEstimate;
                    totalHoursActual += item.HoursActual;
                }
                tempModel.TotalEstimatedHours = totalHoursEstimate;
                tempModel.TotalActualHours = totalHoursActual;
            }

            await Context.SaveChangesAsync();

            return Created($"/api/v1/work/projects/{model.Id}", model);
        }

        [HttpDelete("projects/{id}")]
        public async Task<ActionResult> DeleteProject(int id)
        {
            var imm = await Context.Projects.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (imm == null) return NotFound();

            Context.Projects.Remove(imm);
            await Context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("projects/{id}/export")]
        public async Task<ActionResult> ExportProject(int id)
        {
            var model = await Context.Projects.Include(x => x.WorkItems).Include(x => x.Comments).Include(x => x.Payments).FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        #endregion

        #region Work Items

        [HttpGet("items")]
        public ActionResult<IEnumerable<WorkItem>> GetItems([FromQuery] bool includeElements = false)
        {
            if (!includeElements) return Ok(Context.WorkItems);

            return Ok(Context.WorkItems.Include(x => x.Artifact).Include(x => x.Comments).Include(x => x.Payment));
        }

        [HttpGet("items/{name}")]
        public async Task<ActionResult<WorkItem>> GetItem(string name)
        {
            return Ok(await Context.WorkItems.Include(x => x.Artifact)
                .Include(x => x.Comments)
                .Include(x => x.Payment)
                .FirstOrDefaultAsync(x => x.Name == name));
        }

        [HttpPost("items")]
        public async Task<ActionResult<WorkItem>> CreateItem([FromBody] WorkItem model)
        {
            if (model == null) return null;

            model.Created = DateTimeOffset.Now;
            model.Updated = DateTimeOffset.Now;

            Context.WorkItems.Add(model);
            await Context.SaveChangesAsync();

            return Created($"/api/v1/work/items/{model.Id}", model);
        }

        [HttpPut("items/{id}")]
        public async Task<ActionResult<WorkItem>> UpdateItem([FromBody] WorkItem model, int id)
        {
            if (model == null) return BadRequest();

            var tempModel = await Context.WorkItems.FirstOrDefaultAsync(x => x.Id == id);
            if (tempModel == null) return NotFound();

            if (model.Payment != null)
            {
                tempModel.Payment = model.Payment;
            }

            // Apply Model Changes
            tempModel.Id = model.Id;
            tempModel.Name = model.Name;
            tempModel.Tags = model.Tags;
            tempModel.Description = model.Description;
            tempModel.Updated = DateTimeOffset.Now;
            tempModel.HoursEstimate = model.HoursEstimate;
            tempModel.Artifact = model.Artifact;
            tempModel.HoursActual = model.HoursActual;
            tempModel.IsPaid = tempModel.Payment != null && tempModel.Payment.Id > 0;
            tempModel.WorkProjectId = model.WorkProjectId;

            if (model.Comments != null)
            {
                foreach (var comment in model.Comments)
                {
                    // New Comments:

                    // Add All Comments:
                    tempModel.Comments.Add(comment);
                }
            }

            await Context.SaveChangesAsync();

            return Created($"/api/v1/work/items/{model.Id}", model);
        }

        [HttpDelete("items/{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var imm = await Context.WorkItems.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (imm == null) return NotFound();

            Context.WorkItems.Remove(imm);
            await Context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("items/{id}/export")]
        public async Task<ActionResult> ExportItem(int id)
        {
            var model = await Context.WorkItems.Include(x => x.Artifact).Include(x => x.Comments).Include(x => x.Payment).FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        #endregion

        #region Work Payments

        [HttpGet("payments")]
        public ActionResult<IEnumerable<WorkPayment>> GetPayments([FromQuery] bool includeElements = false)
        {
            if (!includeElements) return Ok(Context.Payments);

            return Ok(Context.Payments.Include(x => x.WorkItems).Include(x => x.Project));
        }

        [HttpGet("payments/{name}")]
        public async Task<ActionResult<WorkPayment>> GetPayment(string name)
        {
            return Ok(await Context.Payments.Include(x => x.WorkItems).Include(x => x.Project).FirstOrDefaultAsync(x => x.Name == name));
        }

        [HttpPost("payments")]
        public async Task<ActionResult<WorkPayment>> CreatePayment([FromBody] WorkPayment model)
        {
            if (model == null || model.Project == null) return BadRequest("A valid model and Project must be provided.");

            model.Created = DateTimeOffset.Now;
            model.Updated = DateTimeOffset.Now;

            Context.Payments.Add(model);
            await Context.SaveChangesAsync();

            return Created($"/api/v1/work/payments/{model.Id}", model);
        }

        [HttpPut("payments/{id}")]
        public async Task<ActionResult<WorkPayment>> UpdatePayment([FromBody] WorkPayment model, int id)
        {
            if (model == null) return BadRequest();

            var tempModel = await Context.Payments.FirstOrDefaultAsync(x => x.Id == id);
            if (tempModel == null) return NotFound();

            // Apply Model Changes
            tempModel.Project = model.Project;
            tempModel.Updated = DateTimeOffset.Now;
            //tempModel.PaymentDate = model.PaymentDate;
            //tempModel.PaymentData = model.PaymentData;
            tempModel.Tags = model.Tags;
            tempModel.Amount = model.Amount;
            tempModel.Description = model.Description;
            tempModel.WorkItems = model.WorkItems;

            if (tempModel.WorkItems.Count > 0)
            {
                foreach (var item in tempModel.WorkItems)
                {
                    item.Payment = model;
                }
            }

            await Context.SaveChangesAsync();

            return Created($"/api/v1/work/payments/{model.Id}", model);
        }

        [HttpDelete("payments/{id}")]
        public async Task<ActionResult> DeletePayment(int id)
        {
            var imm = await Context.Payments.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (imm == null) return NotFound();

            Context.Payments.Remove(imm);
            await Context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("payments/{id}/export")]
        public async Task<ActionResult> ExportPayment(int id)
        {
            var model = await Context.Payments.Include(x => x.WorkItems).Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        #endregion

        #region Work Comments

        [HttpGet("comments")]
        public ActionResult<IEnumerable<WorkComment>> GetComment([FromQuery] bool includeElements = false)
        {
            if (!includeElements) return Ok(Context.Comments);

            return Ok(Context.Comments);
        }

        [HttpGet("comments/{name}")]
        public async Task<ActionResult<WorkComment>> GetComment(string name)
        {
            return Ok(await Context.Comments.FirstOrDefaultAsync(x => x.Name == name));
        }

        [HttpPost("comments")]
        public async Task<ActionResult<WorkComment>> CreateComment([FromBody] WorkComment model)
        {
            if (model == null) return null;

            model.Created = DateTimeOffset.Now;
            model.Updated = DateTimeOffset.Now;

            Context.Comments.Add(model);
            await Context.SaveChangesAsync();

            return Created($"/api/v1/work/comments/{model.Id}", model);
        }

        [HttpPut("comments/{id}")]
        public async Task<ActionResult<WorkComment>> UpdateComment([FromBody] WorkComment model, int id)
        {
            if (model == null) return BadRequest();

            var tempModel = await Context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (tempModel == null) return NotFound();

            // Apply Model Changes
            tempModel.Updated = DateTimeOffset.Now;
            tempModel.Tags = model.Tags;
            tempModel.Description = model.Description;
            tempModel.Comment = model.Comment;

            await Context.SaveChangesAsync();

            return Created($"/api/v1/work/comments/{model.Id}", model);
        }

        [HttpDelete("comments/{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var imm = await Context.Comments.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (imm == null) return NotFound();

            Context.Comments.Remove(imm);
            await Context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("comments/{id}/export")]
        public async Task<ActionResult> ExportComment(int id)
        {
            var model = await Context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        #endregion

    }
}
