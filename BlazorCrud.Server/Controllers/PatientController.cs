﻿using AutoMapper;
using BlazorCrud.Shared.Data;
using BlazorCrud.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BlazorCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientContext _context;
        private readonly IMapper _mapper;

        public PatientController(PatientContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a list of paginated patients with a default page size of 10.
        /// </summary>
        [HttpGet]
        public PagedResult<Patient> GetAll([FromQuery]string name, int page)
        {
            int pageSize = 10;
            if (name != null)
            {
                return _context.Patients
                .Where(p => p.Name.Contains(name, System.StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(p => p.Id)
                .GetPaged(page, pageSize);
            }
            else
            {
                return _context.Patients
                  .OrderBy(p => p.Id)
                  .GetPaged(page, pageSize);
            }
        }

        /// <summary>
        /// Gets a specific patient by Id.
        /// </summary>
        [HttpGet("{id}", Name = "GetPatient")]
        public ActionResult<Patient> GetById(int id)
        {
            var item = _context.Patients.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            item = _context.Patients
                .Include(patient => patient.Contacts)
                .Single(p => p.Id == id);
            return item;
        }

        /// <summary>
        /// Creates a patient.
        /// </summary>
        [HttpPost]
        [Authorize]
        public IActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.ModifiedDate = DateTime.Now;
                _context.Patients.Add(patient);
                _context.SaveChanges();
                return CreatedAtRoute("GetPatient", new { id = patient.Id }, patient);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Updates a patient with a specific Id.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(int id, Patient patient)
        {
            if (ModelState.IsValid)
            {
                // var pat = _context.Patients.Find(id);
                var pat = _context.Patients
                    .AsNoTracking()
                    .Include(pa => pa.Contacts)
                    .Single(p => p.Id == id);
                if (pat == null)
                {
                    return NotFound();
                }

                patient.ModifiedDate = DateTime.Now;
                _mapper.Map(patient, pat);
                _context.Patients.Update(pat);
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Deletes a specific patient by Id.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var patient = _context.Patients.Find(id);
                if (patient == null)
                {
                    return NotFound();
                }

                _context.Patients.Remove(patient);
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}