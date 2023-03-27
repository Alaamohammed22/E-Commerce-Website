using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.repo
{
    public class Reposatory<T> : Ireposatory<T> where T : class
    {
        Context c;
        public Reposatory(Context cc)
        {
            c = cc;
        }
        public List<T> getall()
        {
            return c.Set<T>().ToList();
        }
        public List<T> getall(string s)
        {
            return c.Set<T>().Include(s).ToList();
        }

        public List<T> getall(string s, string s2)
        {
            return c.Set<T>().Include(s).Include(s2).ToList();
        }

        public T getbyid(int id)
        {

            return c.Find<T>(id);
        }


        public void create(T t)
        {
            c.Set<T>().Add(t);
            c.SaveChanges();
        }
        public void update(T tt)
        {
            // T teemp = c.Find<T>(tt);
            c.Update<T>(tt);
            c.SaveChanges();
        }
        public void delete(T tt)
        {
            c.Remove<T>(tt);
            c.SaveChanges();
        }

    }
}
