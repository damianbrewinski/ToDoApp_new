using System;
using System.Collections.Generic;
using ToDoApp.Areas.Zadania.Models;
using System.Linq;
using System.Data.Entity;

namespace ToDoApp.Services
{
    public class ZadanieRepository : IZadanieRepository, IDisposable
    {
        private ZadaniaContext _context;
        public ZadanieRepository(ZadaniaContext context)
        {
            _context = context;
        }

        public IEnumerable<ToDoModel> GetAll()
        {
            return _context.Zadania.ToList();
        }

        public ToDoModel GetById(int id)
        {
            return _context.Zadania.Find(id);
        }

        public int Add(ToDoModel item)
        {
            int result = -1;

            if (item !=null)
            {
                _context.Zadania.Add(item);
                _context.SaveChanges();
                result = item.Identifier;
            }
            else
            {
                throw new ArgumentNullException("item");
            }
            return result;
        }
        public int Edit(ToDoModel item)
        {
            int result = -1;
            if (item != null)
            {
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
                result = item.Identifier;
            }
            else
            {
                throw new ArgumentNullException("item");

            }
            return result;
        }
        public bool Delete(int id)
        {
            if (id > 0)
            {
                ToDoModel zadania = _context.Zadania.Find(id);
                _context.Zadania.Remove(zadania);
                _context.SaveChanges();

            }
            return true;
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}