﻿using ManagementCourse.IReposiory;
using ManagementCourse.Models.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementCourse.Reposiory
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected RTCContext db { get; set; }
        protected DbSet<T> table = null;

        public GenericRepository()
        {
            db = new RTCContext();
            table = db.Set<T>();
        }

        public GenericRepository(RTCContext db)
        {
            this.db = db;
            table = db.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public List<T> GetAllList()
        {
            return table.ToList();
        }
      

        public T GetByID(int id)
        {
            return table.Find(id);
        }
       

        public int Create(T item)
        {
            table.Add(item);
            return db.SaveChanges();
        }

        public int Update(T item)
        {
            table.Attach(item);
            db.Entry(item).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public int Delete(int id)
        {
            table.Remove(table.Find(id));
            return db.SaveChanges();
        }

        public int Delete(List<T> item)
        {
            try
            {
                table.RemoveRange(item);
                return 1;
            }
            catch
            {
                return 0;
            }

        }

        public int Confirm(T[] item)
        {
            for (int i = 0; i < item.Length; i++)
            {
                table.Attach(item[i]);
                db.Entry(item[i]).State = EntityState.Modified;
            }
            return db.SaveChanges();
        }

        public int RemoveRange(IEnumerable<T> items)
        {
            table.RemoveRange(items);
            return db.SaveChanges();
        }

        public int Remove(T item)
        {
            table.Remove(item);
            return db.SaveChanges();
        }
    }
}
