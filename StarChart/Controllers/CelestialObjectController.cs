﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route(""), ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int Id)
        {
            var celestialObject = _context.CelestialObjects.Find(Id);
            if (celestialObject == null)
                return NotFound();
            celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == Id).ToList();
            return Ok(celestialObject);
        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Name == name).ToList();
            if (!celestialObjects.Any())
                return NotFound();
            foreach (var item in celestialObjects)
            {
                item.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == item.Id).ToList();
            }
            return Ok(celestialObjects);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects;
            foreach (var item in celestialObjects)
            {
                item.Satellites = celestialObjects.Where(e => e.OrbitedObjectId == item.Id).ToList();
            }
            return Ok(celestialObjects);
        }
        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var _celestialObject = _context.CelestialObjects.Find(id);
            if (_celestialObject == null)
                return NotFound();
            _celestialObject.Name = celestialObject.Name;
            _celestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            _celestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.CelestialObjects.Update(_celestialObject);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var _celestialObject = _context.CelestialObjects.Find(id);
            if (_celestialObject == null)
                return NotFound();
            _celestialObject.Name = name;
            _context.CelestialObjects.Update(_celestialObject);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var _celestialObjects = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id).ToList();
            if (!_celestialObjects.Any())
                return NotFound();
            _context.CelestialObjects.RemoveRange(_celestialObjects);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
