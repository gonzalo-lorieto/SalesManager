﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesManager.Core.Interfaces;
using SalesManager.Core.Models;
using SalesManager.Core.Services;

namespace SalesManager.Controllers
{
    public class ProductsController : Controller
    {
        private AbstractService<Product> _service;

        public ProductsController(AbstractService<Product> service) => _service = service;

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View((await _service.ListAsync()).Data);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.GetByIdAsync(id.Value);

            if (result.Data == null)
            {
                return NotFound();
            }

            return View(result.Data);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CostPerKilo,PricePerKilo,Over10KilosPricePerKilo,FriendsPricePerKilo,Description")] Product product)
        {
            if (_service.ValidateEntity(product).Data)
            {
                var result = await _service.AddAsync(product);
                
                if (!result.IsSuccess)
                {

                }

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.GetByIdAsync(id.Value);
            if (result == null)
            {
                return NotFound();
            }

            return View(result.Data);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CostPerKilo,PricePerKilo,Over10KilosPricePerKilo,FriendsPricePerKilo,Description")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (_service.ValidateEntity(product).Data)
            {
                try
                {
                    var result = await _service.UpdateAsync(id, product);

                    if (!result.IsSuccess)
                    {

                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    var alreadyExists = await _service.EntityExistsAsync(product.Id);
                    if (!alreadyExists.Data)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var result = await _service.GetByIdAsync(id.Value);

            if (result.Data == null)
            {
                return NotFound();
            }

            return View(result.Data);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
