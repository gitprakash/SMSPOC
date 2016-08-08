using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Repositorylibrary
{
    public class Genericrepository<TObject> : IGenericRepository<TObject> where TObject : class
    {
        protected DbContext _context;

        public Genericrepository(DbContext context)
        {
            _context = context;
        }

        public ICollection<TObject> GetAll()
        {
            return _context.Set<TObject>().AsNoTracking().ToList();
        }
        public virtual IQueryable<TObject> GetAllLazyLoad(Expression<Func<TObject, bool>> filter, params Expression<Func<TObject, object>>[] children)
        {
            children.ToList().ForEach(x => _context.Set<TObject>().Include(x).Load());
            return _context.Set<TObject>();
        }
        public async Task<ICollection<TObject>> GetAllAsync()
        {
            return await _context.Set<TObject>().AsNoTracking().ToListAsync();
        }
        public async Task<ICollection<TObject>> GetPagedResult(int skip, int take, string ordercolumn, bool desc, Expression<Func<TObject, bool>> match = null)
        {
            if (match == null)
            {
                return await _context.Set<TObject>().AsNoTracking().OrderByAscDsc(ordercolumn, desc).Skip(skip).Take(take).AsNoTracking().ToListAsync();
            }
            return await _context.Set<TObject>().AsNoTracking().Where(match).OrderByAscDsc(ordercolumn, desc).Skip(skip).Take(take).AsNoTracking().ToListAsync();
        }
        public async Task<ICollection<TResult>> GetPagedResult<TResult>(int skip, int take, string ordercolumn, bool desc, Expression<Func<TObject, TResult>> project,
            Expression<Func<TObject, bool>> match = null)
        {
            IQueryable<TObject> qry = _context.Set<TObject>().AsNoTracking();
            if (match == null)
            {
                return await qry.OrderByAscDsc(ordercolumn, desc).Skip(skip).Take(take).AsNoTracking().Select(project).ToListAsync();
            }
            return await qry.Where(match).AsNoTracking().Select(project).OrderByAscDsc(ordercolumn, desc).Skip(skip).Take(take).ToListAsync();
        }

        public TObject Get(int id)
        {
            return _context.Set<TObject>().Find(id);
        }

        public async Task<TObject> GetAsync(long id)
        {
            return await _context.Set<TObject>().FindAsync(id);
        }

        public TObject Find(Expression<Func<TObject, bool>> match)
        {
            return _context.Set<TObject>().SingleOrDefault(match);
        }

        public async Task<TObject> FindAsync(Expression<Func<TObject, bool>> match)
        {
            return await _context.Set<TObject>().SingleOrDefaultAsync(match);
        }

        public ICollection<TObject> FindAll(Expression<Func<TObject, bool>> match)
        {
            return _context.Set<TObject>().Where(match).ToList();
        }

        public async Task<ICollection<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match)
        {
            return await _context.Set<TObject>().Where(match).ToListAsync();
        }

        public async Task<ICollection<TResult>> FindAllAsync<TResult>(Expression<Func<TObject, bool>> match,
            Expression<Func<TObject, TResult>> project)
        {
            return await _context.Set<TObject>().Where(match).Select(project).ToListAsync();
        }

        public async Task<ICollection<TResult>> FindAllAsync<TResult, Tkey>(Expression<Func<TObject, bool>> match,
            Expression<Func<TObject, TResult>> select, Expression<Func<TResult, Tkey>> sort)
        {
            return await _context.Set<TObject>().Where(match).Select(select).OrderBy(sort).ToArrayAsync();
        }


        public TObject Add(TObject t)
        {
            _context.Set<TObject>().Add(t);
            _context.SaveChanges();
            return t;
        }

        public async Task<TObject> AddAsync(TObject t)
        {
            // _context.Database.Log = (data => Debug.WriteLine(data));
            _context.Set<TObject>().Add(t);
            await _context.SaveChangesAsync();
            return t;
        }
        public async Task<int> AddRangeAsync(List<TObject> t)
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
            _context.Database.Log = (data => Debug.WriteLine(data));
            _context.Set<TObject>().AddRange(t);
            int result = await _context.SaveChangesAsync();
            _context.Configuration.AutoDetectChangesEnabled = true;
            return result;
        }

        public TObject Update(TObject updated, int key)
        {
            if (updated == null)
                return null;

            TObject existing = _context.Set<TObject>().Find(key);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(updated);
                _context.SaveChanges();
            }
            return existing;
        }

        public async Task<TObject> UpdateAsync(TObject updated, long key)
        {
            if (updated == null)
                return null;

            TObject existing = await _context.Set<TObject>().FindAsync(key);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(updated);
                await _context.SaveChangesAsync();
            }
            return existing;
        }

        public void Delete(TObject t)
        {
            _context.Set<TObject>().Remove(t);
            _context.SaveChanges();
        }

        public async Task<int> DeleteAsync(TObject t)
        {
            _context.Set<TObject>().Remove(t);
            return await _context.SaveChangesAsync();
        }

        public int Count()
        {
            return _context.Set<TObject>().Count();
        }

        public async Task<int> CountAsync(Expression<Func<TObject, bool>> match = null)
        {
            if (match != null)
                return await _context.Set<TObject>().CountAsync(match);
            else
                return await _context.Set<TObject>().CountAsync();

        }

        public async Task<bool> AnyAsync(Expression<Func<TObject, bool>> match)
        {
            return await _context.Set<TObject>().AsNoTracking().AnyAsync(match);
        }
        public async Task<TResult[]> ToArrayAsync<TResult>(Expression<Func<TObject, TResult>> select)
        {
            return await _context.Set<TObject>().AsNoTracking().Select(select).ToArrayAsync();
        }

        public async Task<int> SaveAsync()
        {
            _context.Database.Log = (qury) => { Debug.WriteLine(qury); };
            return await _context.SaveChangesAsync();
        }
    }
}