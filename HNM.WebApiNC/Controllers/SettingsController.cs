using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsPayment;
using HNM.RepositoriesNC.PaymentRepositoriesBase;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class SettingsController : ControllerBase, IDisposable
    {
        private readonly HanomaContext _context;
        private IRepositoryWrapper _repoWrapper;
        private IPaymentRepositoryWrapper _repoPaymentWrapper;
        public SettingsController(HanomaContext context, IRepositoryWrapper repoWrapper, IPaymentRepositoryWrapper repoPaymentWrapper)
        {
            _context = context;
            _repoWrapper = repoWrapper;
            _repoPaymentWrapper = repoPaymentWrapper;
        }
        /// <summary>
        /// Get Config
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<SettingDTO>> Getconfig()
        {
            var output = new SettingDTO();
            return output;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        private async Task<Setting> GetAllSetting()
        {
            var output = await _repoWrapper.Setting.GetSetting();
            return output;
        }
        
        // GET: api/Settings/5
        [HttpGet("{id}")]
        private async Task<ActionResult<Setting>> GetSetting(int id)
        {
            var setting = await _context.Setting.FindAsync(id);

            if (setting == null)
            {
                return NotFound();
            }

            return setting;
        }

        // PUT: api/Settings/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        private async Task<IActionResult> PutSetting(int id, Setting setting)
        {
            if (id != setting.Setting_ID)
            {
                return BadRequest();
            }

            _context.Entry(setting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Settings
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost]
        //private async Task<ActionResult<Setting>> PostSetting(Setting setting)
        //{
        //    _context.Setting.Add(setting);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetSetting", new { id = setting.Setting_ID }, setting);
        //}

        // DELETE: api/Settings/5
        [HttpDelete("{id}")]
        private async Task<ActionResult<Setting>> DeleteSetting(int id)
        {
            var setting = await _context.Setting.FindAsync(id);
            if (setting == null)
            {
                return NotFound();
            }

            _context.Setting.Remove(setting);
            await _context.SaveChangesAsync();

            return setting;
        }

        private bool SettingExists(int id)
        {
            return _context.Setting.Any(e => e.Setting_ID == id);
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="CurrentDate"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<XSMBDTO> GetSXMBByDate(string CurrentDate)
        //{
        //    var output = new XSMBDTO();
        //    var resultXSMB = new TblSXMB();
        //    if (String.IsNullOrEmpty(CurrentDate))
        //    {
        //        resultXSMB = await _context.TblSXMB.OrderByDescending(p => p.SXDate).FirstOrDefaultAsync();
        //    }
        //    else
        //    {
        //        resultXSMB = await _context.TblSXMB.SingleOrDefaultAsync(p => p.SXDate == CurrentDate);
        //        if (resultXSMB == null)
        //        {
        //            resultXSMB = await _context.TblSXMB.OrderByDescending(p => p.SXDate).FirstOrDefaultAsync();
        //        }
        //    }           

        //    output.XSMB = resultXSMB;
        //    output.XSMBDauDuoi = await _context.TblSXMBTKDauDuoi.SingleOrDefaultAsync(p => p.SXDate == resultXSMB.SXDate);

        //    var resultDienToan123 = new TblSXDienToan123();
        //    if(String.IsNullOrEmpty(CurrentDate))
        //    {
        //        resultDienToan123 = await _context.TblSXDienToan123.OrderByDescending(p => p.SXDate).FirstOrDefaultAsync();
        //    }
        //    else
        //    {
        //        resultDienToan123 = await _context.TblSXDienToan123.SingleOrDefaultAsync(p => p.SXDate == CurrentDate);
        //        if (resultDienToan123 == null)
        //        {
        //            resultDienToan123 = await _context.TblSXDienToan123.OrderByDescending(p => p.SXDate).FirstOrDefaultAsync();
        //        }
        //    }
            
        //    output.DienToan123 = resultDienToan123;

        //    var resultDienToan636 = new TblSXDienToan636();
        //    if(String.IsNullOrEmpty(CurrentDate))
        //    {
        //        resultDienToan636 = await _context.TblSXDienToan636.OrderByDescending(p => p.SXDate).FirstOrDefaultAsync();
        //    }
        //    else
        //    {
        //        resultDienToan636 = await _context.TblSXDienToan636.SingleOrDefaultAsync(p => p.SXDate == CurrentDate);
        //        if (resultDienToan636 == null)
        //        {
        //            resultDienToan636 = await _context.TblSXDienToan636.OrderByDescending(p => p.SXDate).FirstOrDefaultAsync();
        //        }
        //    }
            
        //    output.DienToan636 = resultDienToan636;

        //    var resultThanTai = new TblSXThantai();
        //    if(String.IsNullOrEmpty(CurrentDate))
        //    {
        //        resultThanTai = await _context.TblSXThantai.OrderByDescending(p => p.SXDate).FirstOrDefaultAsync();
        //    }
        //    else
        //    {
        //        resultThanTai = await _context.TblSXThantai.SingleOrDefaultAsync(p => p.SXDate == CurrentDate);
        //        if (resultThanTai == null)
        //        {
        //            resultThanTai = await _context.TblSXThantai.OrderByDescending(p => p.SXDate).FirstOrDefaultAsync();
        //        }
        //    }
            
        //    output.ThanTai = resultThanTai;

        //    return output;
        //}
        //[HttpGet]
        //public async Task<XSMTDTO> GetSXMTByDate(string CurrentDate)
        //{
        //    var output = new XSMTDTO();
        //    var resultXSMT = new List<TblSXMT>();
        //    if(String.IsNullOrEmpty(CurrentDate))
        //    {
        //        var lastDate = await _context.TblSXMT.OrderByDescending(p => p.SXDate).FirstOrDefaultAsync();
        //        resultXSMT = await _context.TblSXMT.Where(p => p.SXDate == lastDate.SXDate).ToListAsync();
        //        output.XSMTDauDuoi = await _context.TblSXMTTKDau.Where(p => p.SXDate == lastDate.SXDate).ToListAsync();
        //    }
        //    else
        //    {
        //        resultXSMT = await _context.TblSXMT.Where(p => p.SXDate == CurrentDate).ToListAsync();
        //        if (resultXSMT == null)
        //        {
        //            var lastDate = await _context.TblSXMT.OrderByDescending(p => p.SXDate).FirstOrDefaultAsync();
        //            resultXSMT = await _context.TblSXMT.Where(p => p.SXDate == lastDate.SXDate).ToListAsync();
        //            output.XSMTDauDuoi = await _context.TblSXMTTKDau.Where(p => p.SXDate == lastDate.SXDate).ToListAsync();
        //        }
        //        else
        //        {
        //            output.XSMTDauDuoi = await _context.TblSXMTTKDau.Where(p => p.SXDate == CurrentDate).ToListAsync();
        //        }
        //    }
            
        //    output.XSMT = resultXSMT;
        //    return output;
        //}
        //[HttpGet]
        //public async Task<XSMNDTO> GetSXMNByDate(string CurrentDate)
        //{
        //    var output = new XSMNDTO();
        //    var resultXSMN = new List<TblSXMN>();
        //    if (String.IsNullOrEmpty(CurrentDate))
        //    {
        //        var lastDate = await _context.TblSXMN.OrderByDescending(p => p.SXDate).FirstOrDefaultAsync();
        //        resultXSMN = await _context.TblSXMN.Where(p => p.SXDate == lastDate.SXDate).ToListAsync();
        //        output.XSMNDauDuoi = await _context.TblSXMNTKDau.Where(p => p.SXDate == lastDate.SXDate).ToListAsync();
        //    }
        //    else
        //    {
        //        resultXSMN = await _context.TblSXMN.Where(p => p.SXDate == CurrentDate).ToListAsync();
        //        if (resultXSMN == null)
        //        {
        //            var lastDate = await _context.TblSXMN.OrderByDescending(p => p.SXDate).FirstOrDefaultAsync();
        //            resultXSMN = await _context.TblSXMN.Where(p => p.SXDate == lastDate.SXDate).ToListAsync();
        //            output.XSMNDauDuoi = await _context.TblSXMNTKDau.Where(p => p.SXDate == lastDate.SXDate).ToListAsync();
        //        }
        //        else
        //        {
        //            output.XSMNDauDuoi = await _context.TblSXMNTKDau.Where(p => p.SXDate == CurrentDate).ToListAsync();
        //        }
        //    }

        //    output.XSMN = resultXSMN;
        //    return output;
        //}
        ////[HttpGet]
        ////public async Task<List<TblSoMo>> GetSomo(string keyword,int page , int pageSize)
        ////{
        ////    if(!String.IsNullOrEmpty(keyword))
        ////    {
        ////        return await _context.TblSoMo.Where(p => p.DreamNoteBoookName.Contains(keyword)).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        ////    }
        ////    return await _context.TblSoMo.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        ////}

        //[HttpGet]
        //public string GetTest()
        //{            
        //    return "";
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="topX"></param>
        /// <returns></returns>
        //[HttpGet]
        //public async Task ProcessProductBrand()
        //{
        //    var lstProductBrand = await _repoWrapper.Brand.GetAllBrand();
        //    foreach (var item in lstProductBrand) // Note tài liệu k nhắc đến statusTypeID
        //    {
        //        int ProductBrandTypeID = 1;
        //        var count = await _repoWrapper.Product.CountProductByBrand(item.ProductBrand_ID);
        //        if (count > 10)
        //        {
        //            ProductBrandTypeID = 2;
        //        }
        //        if (count > 100)
        //        {
        //            ProductBrandTypeID = 3;
        //        }
        //        if (ProductBrandTypeID != 1)
        //        {
        //            UpgrageBrandPackageDTO model = new UpgrageBrandPackageDTO();
        //            model.ServiceId = ProductBrandTypeID;
        //            model.Discount_ID = 0;
        //            model.ProductBrandId = item.ProductBrand_ID;
        //            var result = await _repoPaymentWrapper.PaymentUpgrade.ProcessOldBrand(model, item.Name, item.Mobile);
                    
        //        }
        //        //Upgrade Type
        //        await _repoWrapper.Brand.UpgradePackageBrand(item.ProductBrand_ID, ProductBrandTypeID);



        //    }
        //}
        
        
       
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
