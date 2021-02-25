using HNM.DataNC.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.RepositoriesBase
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected HanomaContext HanomaContext { get; set; }
        private DbSet<T> table = null;
        public RepositoryBase(HanomaContext HanomaContext)
        {
            this.HanomaContext = HanomaContext;
            table = HanomaContext.Set<T>();
        }

        public IQueryable<T> FindAll()
        {
            return this.HanomaContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.HanomaContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            this.HanomaContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.HanomaContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.HanomaContext.Set<T>().Remove(entity);
        }

        public ValueTask<T> FindAsync(object id)
        {
            return this.HanomaContext.Set<T>().FindAsync(id);
        }

        public T GetById(object id)
        {
            return table.Find(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return this.HanomaContext.Set<T>().FirstOrDefault(expression);
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await this.HanomaContext.Set<T>().SingleOrDefaultAsync(expression);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await this.HanomaContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public string FormatURL(string Title)
        {
            string StrSource = "";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] bytes = Encoding.UTF8.GetBytes(Title.ToLower());
            StrSource = encoder.GetString(bytes).Trim();

            StrSource = StrSource.Replace(((char)34).ToString(), "");
            StrSource = StrSource.Replace(((char)8220).ToString(), "");
            StrSource = StrSource.Replace(((char)8221).ToString(), "");
            StrSource = StrSource.Replace(" ", "-");


            var replaceChars = this.HanomaContext.ReplaceChar.ToList();
            foreach (var charItem in replaceChars)
            {
                StrSource = StrSource.Replace(charItem.OldChar, charItem.NewChar).ToString();
            }


            StrSource = RepairURL(StrSource);
            return StrSource;
        }
        public string RepairURL(string URL)
        {
            string tmp = URL;

            foreach (char chr in URL)
            {
                if (!((chr >= 'a' && chr <= 'z') || (chr == '-') || (chr >= '0' && chr <= '9')))
                {
                    tmp = tmp.Replace(chr, '-');
                    this.HanomaContext.Database.ExecuteSqlCommand("INSERT ReplaceChar (OldChar, NewChar) VALUES (@OldChar, @NewChar)",
                        new SqlParameter("@OldChar", chr),
                        new SqlParameter("@NewChar", '-')
                        );

                }
            }

            return tmp;
        }

        public string CutText(string TextInput, int NumberCharacter)
        {
            string Result = "";
            if (string.IsNullOrEmpty(TextInput)) return TextInput;
            TextInput = StripHTML(TextInput);
            TextInput = Regex.Replace(TextInput, @"\r\n?|\n|\t", " ");
            TextInput = Regex.Replace(TextInput, @"\s+", " ");

            int Length = TextInput.Length;
            if (Length < NumberCharacter)
                return TextInput;

            // Process
            TextInput = TextInput.Substring(0, NumberCharacter);
            int index = TextInput.LastIndexOfAny(new char[] { ' ' });
            Result = TextInput.Substring(0, index);
            return Result;
        }

        public string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        //private bool disposed = false;
        //// Protected implementation of Dispose pattern.
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposed)
        //        return;

        //    if (disposing)
        //    {
        //        HanomaContext.Dispose();              
        //    }

        //    // Free any unmanaged objects here.
        //    //
        //    disposed = true;
        //}
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
    }
}
