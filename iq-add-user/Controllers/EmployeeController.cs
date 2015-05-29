using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iq_add_user.Models;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Web.Configuration;
using System.IO;
using iq_add_user.Helpers;

namespace iq_add_user.Controllers
{
    public class EmployeeController : Controller
    {
        private IQMS_Entities db = new IQMS_Entities();
        

        // GET: PR_EMP
        public ActionResult Index()
        {
            return View(db.PR_EMP.OrderByDescending(o => o.ID).ToList());
        }

        // GET: PR_EMP/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PR_EMP pR_EMP = db.PR_EMP.Find(id);
            if (pR_EMP == null)
            {
                return HttpNotFound();
            }
            return View(pR_EMP);
        }



        // GET: PR_EMP/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PR_EMP pR_EMP = db.PR_EMP.Find(id);
            if (pR_EMP == null)
            {
                return HttpNotFound();
            }
            return View(pR_EMP);
        }

        // POST: PR_EMP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PR_DEPARTMENT_ID,PR_PAYTYPE_ID,PR_PAYGROUP_ID,EMPNO,FIRST_NAME,MIDDLE_NAME,LAST_NAME,SSNMBR,ADDR1,ADDR2,STATE,COUNTRY,ZIP,PHONE_NUMBER,CITY,CHARGE_RATE,STATUS_CODE,DATE_HIRED,TERMINATION_DATE,LAST_REVIEW_DATE,NEXT_REVIEW_DATE,RACE,SEX,BASIS,RATE,BIRTH_DATE,CITIZEN,MARRIED_STATUS,EIC_MARRIED_STATUS,EIC,LABOR_INDICATOR,NICKNAME,HANDICAP,COMMENT1,PLUGIN_ENTITY_ID,CONTACT,CONTACT_PHONE,JOB_TITLE,CUSER1,CUSER2,CUSER3,CUSER4,CUSER5,CUSER6,NUSER1,NUSER2,NUSER3,NUSER4,NUSER5,RESET_BENEFITS_DATE,EMP_STATUS_ID,ELEMENTS_ID,TA_SETTINGS_ID,TA_SHIFT_ID,TURN_OFF_DD,SNDOP_ID,RESIDENT_VISA_EXPR,OT1_PR_PAYTYPE_ID,OT2_PR_PAYTYPE_ID,OT3_PR_PAYTYPE_ID,HR_JOB_DESCRIP_ID,BADGENO,PK_HIDE,WEB_USERID,EMP_LEVEL_ID,SUTA_TAX_STATE,SWT_STATE,EMAIL,SUPERVISOR,SUPERVISOR_ID,HR_MARRIED_STATUS,PR_HOLIDAY_PAYTYPE_ID,LAST_CALC_DATE,AUTO_PR_PAYTYPE_ID,MOBILE,PAGER,VETERAN_STATUS,MFGCELL_ID,EMP_PROV_CD,SUFFIX,EW2,CURRENCY_LANGUAGE_ID,CURRENCY_ID,DISTLIST,DISABLED,QUAL_DIS_VET,ARM_FORC_VET,OTHER_PROT_VET,NEW_SEP_VET,SEPARATION_DATE")] PR_EMP pR_EMP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pR_EMP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pR_EMP);
        }

        // GET: PR_EMP/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PR_EMP pR_EMP = db.PR_EMP.Find(id);
            if (pR_EMP == null)
            {
                return HttpNotFound();
            }
            return View(pR_EMP);
        }

        // POST: PR_EMP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            PR_EMP pR_EMP = db.PR_EMP.Find(id);
            db.PR_EMP.Remove(pR_EMP);
            db.SaveChanges();
            return RedirectToAction("Index");
        }





        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
